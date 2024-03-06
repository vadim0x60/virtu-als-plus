using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour {
	private Vector3 screenPoint; private Vector3 offset; private float _lockedYPosition;

	void OnClick() {
		//screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position); // I removed this line to prevent centring 
		//_lockedYPosition = screenPoint.y;
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		Cursor.visible = false;
		Debug.Log ("Cube click");
	}

	void OnMouseDrag() 
	{ 
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		//curPosition.x = _lockedYPosition;
		transform.position = curPosition;
		/*if (transform.position.y > 2f) {
			Vector3 newPos = new Vector3 (transform.position.x, 2f, transform.position.z);
			transform.position = newPos;
		} else if (transform.position.y < -2f) {
			Vector3 newPos = new Vector3 (transform.position.x, -2f, transform.position.z);
			transform.position = newPos;
		}*/
	}

	void OnMouseUp()
	{
		Cursor.visible = true;
	}
}
