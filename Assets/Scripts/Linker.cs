using UnityEngine;
using System.Collections;

public class Linker : MonoBehaviour {

	public void PlayTutorial() {
		Application.OpenURL("http://www.virtu-ALS.com/tutorial.html");
	}

	public void Website() {
		Application.OpenURL("http://www.virtu-ALS.com");
	}

	public void RCUKsite() {
		Application.OpenURL("https://www.resus.org.uk/resuscitation-guidelines/");
	}

	public void LITFLsite() {
		Application.OpenURL("http://lifeinthefastlane.com/ecg-library/basics/");
	}
}
