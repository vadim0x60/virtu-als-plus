using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour {
    public float speed = 10f;
    private float startTime = 0f;
    private float startY = 0f;
    private Vector3 lastPos;
	// Use this for initialization
	void Start () {
        startTime = Time.time;
        startY = transform.position.y;
        lastPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;
        x += Time.deltaTime * speed;
        float timeDiff = Time.time - startTime;
        if (timeDiff >= 0.25f)
        {
            startTime = Time.time;
            y = startY;
        } else
        {
            y = startY + (Mathf.Sin(timeDiff * 8 * Mathf.PI));
        }
        transform.position = new Vector3(x, y, z);
        DrawLine(lastPos, new Vector3(x, y, z), Color.white, 2f);
        lastPos = transform.position;
        Debug.Log("Framerate " + (1f / Time.deltaTime) + "fps");
    }
    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }

}
