using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class phpTest : MonoBehaviour {
    void Start()
    {
        StartCoroutine(CheckInternet());
        
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
                StartCoroutine(Upload());
            }
        }       
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("tablenamepost", "myData");

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/index.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            Debug.Log(www.downloadHandler.text);
        }
    }
}
