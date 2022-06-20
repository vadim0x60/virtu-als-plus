using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectionCheck : MonoBehaviour {
    public GameObject introScreen;
    public GameObject playScreen;
    void Start()
    {
        if (PlayerPrefs.HasKey("Registered"))
        {
            playScreen.SetActive(true);
            this.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(CheckInternet());
        }
    }

    IEnumerator CheckInternet()
    {
        string url = "https://google.com";
        using (WWW www = new WWW(url))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.error);
            }
            else
            {
                introScreen.SetActive(true);
                this.gameObject.SetActive(false);
            }
        }
    }
}
