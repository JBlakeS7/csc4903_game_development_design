using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchMatchmaking : MonoBehaviour
{
    public Text text;
    public bool working = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!working)
            text.text = "MatchMaking Servers Offline";
        else
            StartCoroutine(GetMatches());
        text.gameObject.SetActive(true);
    }

    IEnumerator GetMatches()
    {
        text.text = "Retreiving Matches";
        yield return new WaitForSeconds(3f);
        if (true)
        {
            text.text = "No Servers Online";
        }
        else
            StartCoroutine(JoinServer());
    }
    IEnumerator JoinServer()
    {
        text.text = "Joining Server";
        yield return new WaitForSeconds(3f);
    }
}
