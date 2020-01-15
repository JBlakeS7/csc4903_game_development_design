using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
//using Konsole;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;

namespace AlphaCSharpServer
{
    public class Program
    {
        //Commands
        private static ConcurrentQueue<string> queuedServerCommands = new ConcurrentQueue<string>();
        private static ConcurrentDictionary<string, Func<string, string>> serverCommands = new ConcurrentDictionary<string, Func<string, string>>();
        private static ConcurrentDictionary<string, AlphaServerPlugin> plugins = new ConcurrentDictionary<string, AlphaServerPlugin>();

        //Networking Variables
        private static byte[] _buffer = new byte[1024];
        private static List<Socket> _clientSockets = new List<Socket>();
        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static bool running = true;
        static void Main(string[] args)
        {
            Console.Title = "C# Alpha Server";
            CheckArgs(args);
            StandardCommands();
            LoadDlls();
            StartCommandQueue();
            SetupServer();
            WriteLine("Enter help or ? for list of commands! ");


            while (running)
            {
                queuedServerCommands.Enqueue(Console.ReadLine()); // Pause
            }
        }

        public static void Indent()
        {
            if (Console.CursorLeft != 0)
                Console.Write("\n>");
            else
                Console.Write(">");
        }

        public static void StandardCommands()
        {
            //ProgressBar p = new ProgressBar(PbStyle.SingleLine, 5);
           // p.Refresh(1, "Adding Command: help");
            serverCommands.TryAdd("help", helpFunction);
           // p.Refresh(2, "Adding Command: shutdown");
            serverCommands.TryAdd("shutdown", ShutdownServer);
           // p.Refresh(3, "Adding Command: clear");
            serverCommands.TryAdd("clear", (string args) => { Console.Clear(); return ""; });
            //p.Refresh(4, "Adding Command: memory");
            serverCommands.TryAdd("memory", (string args) => {
                Process proc = Process.GetCurrentProcess();
                Console.WriteLine("Memory Used: " + proc.PrivateMemorySize64 / 1024 / 1024f + "Mb");
                return "";
            });
           // p.Refresh(5, "Adding Command: mem-gc");
            serverCommands.TryAdd("mem-gc", (string args) => {
                GC.Collect();
                return "";
            });
           // p.Refresh(6, "Adding Command: plugins");
            serverCommands.TryAdd("plugins", (string args) => {
                Console.Write("Loaded Plugins:");
                foreach (KeyValuePair<string, AlphaServerPlugin> entry in plugins)
                {
                    Console.Write(" " + entry.Key + ",");
                }
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                Console.Write(" \n");
                return "";
            });
            //p.Refresh(6, "Done Adding Default Commands");
        }

        private static void CheckArgs(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Checking Args: ");
            foreach (string arg in args)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(arg);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" Finished");
        }

        private static void LoadDlls()
        {
            Console.ForegroundColor = ConsoleColor.White;
            string[] files = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "/plugins/", "*.dll", SearchOption.AllDirectories);
            //ProgressBar p = new ProgressBar(PbStyle.SingleLine, Math.Max(files.Length, files.Length + 1));

            int i = 0;
            foreach (string file in files)
            {
                i++;
                //p.Refresh(i, "Loading: " + file.Split("\\").Last());
                try
                {
                    var DLL = Assembly.LoadFile(@file);

                    foreach (Type type in DLL.GetExportedTypes())
                    {
                        if (type.GetInterfaces().Contains(typeof(AlphaServerPlugin)))
                        {
                            AlphaServerPlugin c = (AlphaServerPlugin)Activator.CreateInstance(type);
                            c.Init(new string[] { "" });
                            plugins.TryAdd(c.name, c);
                        }
                    }
                }
                catch (Exception e)
                {
                    Error("Error Loading DLL: ", file);
                    Error("Error Info: ", e.Message);
                }
            }
            //p.Refresh(files.Length + 1, "Finished Loading DLLs");

        }

        private static void StartCommandQueue()
        {
            ThreadStart childref = new ThreadStart(_CommandQueue);
            WriteLine("Creating Command Thread");
            Thread childThread = new Thread(childref);
            childThread.Start();
        }
        private static void _CommandQueue()
        {
            while (running)
            {
                if (!queuedServerCommands.IsEmpty)
                {
                    string value;
                    queuedServerCommands.TryDequeue(out value);
                    if (value != null)
                    {
                        string[] args = value.Split(' ');
                        if (args.Length == 1)
                        {
                            args = new string[2] { args[0], "" };
                        }
                        Func<string, string> function;
                        if (serverCommands.TryGetValue(args[0], out function))
                        {
                            function(args[1]);
                        }
                        else
                        {
                            Alert("Command: ", args[0] + " Not Found");
                        }
                    }
                }
            }
        }

        private static void WriteLine(string msg)
        {
            if (Console.CursorLeft == 1)
                Console.CursorLeft = 0;
            Console.WriteLine(msg);
            Indent();

        }
        private static void Alert(string title, string msg)
        {
            if (Console.CursorLeft == 1)
                Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(title);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(msg);
            Indent();
        }
        private static void Error(string error, string msg)
        {
            if (Console.CursorLeft == 1)
                Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(error);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(msg);
            Indent();
        }


        private static void SetupServer()
        {
            int port = 25566;
            WriteLine("Opening Port");
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            _serverSocket.Listen(5);
            WriteLine("Listening on Port: " + port);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void AcceptCallback(IAsyncResult AR)
        {
            try
            {
                Socket socket = _serverSocket.EndAccept(AR);
                _clientSockets.Add(socket);
                WriteLine("Client Connected");
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            catch (Exception e)
            {
                if (running)
                    Error("Error onClientConnecting: ", e.Message);
            }
        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            int received = socket.EndReceive(AR);
            byte[] dataBuffer = new byte[received];
            Array.Copy(_buffer, dataBuffer, received);
            string text = Encoding.ASCII.GetString(dataBuffer);
            WriteLine("Text received: " + text);

            if (text.ToLower() == "get time")
            {
                byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongDateString());
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            }
            else
            {
                byte[] data = Encoding.ASCII.GetBytes("Invalid Request");
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            }
        }

        private static void SendCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
        }

        private static string ShutdownServer(string arg)
        {
            Console.WriteLine("Calling Shutdown");
           //ProgressBar p = new ProgressBar(PbStyle.DoubleLine, 1);
            //p.Refresh(1, "Closing Port");
            running = false;
            _serverSocket.Dispose();
            _serverSocket.Close();
            //p.Refresh(1, "Server Closed");
            return "";
        }

        private static string helpFunction(string command)
        {
            if (command == "")
            {
                Console.Write("Commands:");
                foreach (KeyValuePair<string, Func<string, string>> entry in serverCommands)
                {
                    Console.Write(" " + entry.Key + ",");
                }
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                WriteLine(" ");
            }
            else if (command == "or ? for list of commands!")
            {
                Error("Woah! ", "Someone is smart");
            }
            else
            {

            }
            return "";
        }
    }

    internal class ProgressBar
    {
    }

    public class ConsoleSpinner
    {
        int counter;

        public void Turn()
        {
            counter++;
            switch (counter % 4)
            {
                case 0: Console.Write("/"); counter = 0; break;
                case 1: Console.Write("-"); break;
                case 2: Console.Write("\\"); break;
                case 3: Console.Write("|"); break;
            }
            System.Threading.Thread.Sleep(100);
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
        }
    }
    public interface AlphaServerPlugin
    {
        string name { get; }
        string description { get; }
        string version { get; }
        string author { get; }

        void Init(string[] args); // Register Commands
        void Enable();
        void Disable();
        void Close();
        void Reload();
        void NewConnection();
        void CloseConnection();
        ConnectionStatus StatusConnection();
    }

    public class ConnectionStatus
    {
        public int ping;
        public ConnectionStatusEnum statusEnum;
    }

    public enum ConnectionStatusEnum
    {
        Connecting,
        Connected,
        Loading,
        Playing,
        Disconnecting,
        Disconnected
    }

}