using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ViridaxGameStudios.Controllers
{
    [CustomEditor(typeof(AIController))]
    public class AIController_Editor : Editor
    {

        #region variables
        private AIController character;

        #endregion

        #region Main Methods
        void OnEnable()
        {
            //Store a reference to the AI Controller script
            character = (AIController)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
        void OnSceneGUI()
        {
            if (character != null)
            {
                //Call the necessary methods to draw the discs and handles on the editor
                Color color = new Color(1f, 0f, 0f, 0.15f);
                DrawDiscs(color, character.transform.position, Vector3.up, -character.transform.forward, ref character.m_DetectionRadius, "Detection Radius");
                color = new Color(0f, 0f, 1f, 0.15f);
                DrawDiscs(color, character.transform.position, Vector3.up, character.transform.forward, ref character.m_AttackRange, "Attack Range");
            }

        }

        protected void DrawDiscs(Color color, Vector3 center, Vector3 normal, Vector3 direction, ref float radius)
        {
            //
            //Method Name : void DrawDiscs(Color color, Vector3 center, Vector3 normal, Vector3 direction, ref float radius)
            //Purpose     : This method draws the necessary discs and slider handles in the editor to adjust the attack range and detection radius.
            //Re-use      : none
            //Input       : Color color, Vector3 center, Vector3 normal, Vector3 direction, ref float radius
            //Output      : none
            //
            //Draw the disc that will represent the detection radius
            Handles.color = color;
            Handles.DrawSolidDisc(center, normal, radius);
            Handles.color = new Color(1f, 1f, 0f, 0.75f);
            Handles.DrawWireDisc(center, normal, radius);

            //Create Slider handles to adjust detection radius properties
            color.a = 0.5f;
            Handles.color = color;
            radius = Handles.ScaleSlider(radius, character.transform.position, direction, Quaternion.identity, radius, 1f);
            radius = Mathf.Clamp(radius, 1f, float.MaxValue);



        }
        protected void DrawDiscs(Color color, Vector3 center, Vector3 normal, Vector3 direction, ref float radius, string label)
        {
            //
            //Method Name : void DrawDiscs(Color color, Vector3 center, Vector3 normal, Vector3 direction, ref float radius, string label)
            //Purpose     : Overloaded method of DrawDiscs(Color color, Vector3 center, Vector3 normal, Vector3 direction, ref float radius)
            //              that adds the necessary labels. 
            //Re-use      : DrawDiscs(Color color, Vector3 center, Vector3 normal, Vector3 direction, ref float radius)
            //Input       : Color color, Vector3 center, Vector3 normal, Vector3 direction, ref float radius, string label
            //Output      : none
            //

            DrawDiscs(color, center, normal, direction, ref radius);
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontSize = 12;
            color.a = 0.8f;
            labelStyle.normal.textColor = color;
            labelStyle.alignment = TextAnchor.UpperCenter;
            Handles.Label(character.transform.position + (direction * radius), label, labelStyle);
        }
        #endregion
    }//end class
}//end namespace

