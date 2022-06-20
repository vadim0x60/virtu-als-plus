/*using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;

public class FBholder : MonoBehaviour {
	public Hub hub;
	public bool debugging = true;
	public GameObject DialogUsername;
	private List <object> scoresList;
	public Text FBscoresTest;
	public string scoresString = "";
	public bool publishAllowed = false;
	public bool loggedIn = false;
	bool loginButtonSwitcher = false;

	// Use this for initialization
	public void Awake() {
		if (FB.IsLoggedIn) {
			DealWithFacebook (true);
		}
	}

	public void FBlogin() {
		if (loginButtonSwitcher) {
			PostToFeed ();
		} else {
			FB.Init (SetInit, OnHideUnity);
		}
	}

	private void SetInit () {
		if (debugging) {
			Debug.Log ("FB init done.");
		}

		if (FB.IsLoggedIn) {
			if (debugging) {
				Debug.Log ("FB logged in.");
			}
			DealWithFacebook (true);
		} else {
			if (debugging) {
				Debug.Log ("FB not logged in.");
			}
			FBloginer ();
		}
	}

	private void OnHideUnity(bool showing) {
		if (!showing) {
			Time.timeScale = 0f;
		} else {
			Time.timeScale = 1f;
		}
	}

	public void FBloginer () {
		FB.LogInWithReadPermissions (new List<string> (){"public_profile" }, AuthCallBack);
	}

	public void PostToFeed () {
		FB.FeedShare(
			string.Empty, //toId
			new System.Uri("http://www.virtu-als.com"), //link
			"The Life Support Simulator", //linkName
			"Take a look at this innovative little app that lets you manage a cardiac arrest on your phone! Perfect preparation for an ALS / ACLS course.", //linkCaption
			"A great way to practise CPR skills!", //linkDescription
			new System.Uri("http://www.virtu-als.com/ScreenShot.png"), //picture
			string.Empty, //mediaSource
			FeedCallback //callback
		);
	}

	void FeedCallback (IResult result) {

	}

	private void AuthCallBack (IResult result) {
		if (result.Error != null) {
			if (debugging) {
				Debug.Log (result.Error);
			}
		} else {
			
			if (FB.IsLoggedIn) {
				if (debugging) {
					Debug.Log ("FB logged worked.");
				}
			} else {
				if (debugging) {
					Debug.Log ("FB logged didn't work.");
				}
			}

			DealWithFacebook (FB.IsLoggedIn);
		}
	}

	private void DealWithFacebook (bool isLoggedIn) {
		loggedIn = isLoggedIn;
		if (debugging) {
			Debug.Log ("Deal with FB");
		}
		if (isLoggedIn) {
			FB.API ("/me?fields=first_name", HttpMethod.GET, DisplayUserName);
		}
	}

	private void DisplayUserName(IResult result) {
		Text UserName = DialogUsername.GetComponent<Text> ();

		if (result.ResultDictionary != null) {
			if (result.ResultDictionary.Count == 0) {
				Debug.Log ("No results!");
			} else {
				foreach (string key in result.ResultDictionary.Keys) {
					Debug.Log (key + " : " + result.ResultDictionary [key].ToString ());
				}
			}
		}

		if (result.Error == null) {
			UserName.text = "Hi there, " + result.ResultDictionary["first_name"] + "! Fancy telling your friends how awesome the app is?";
			loginButtonSwitcher = true;
			//QueryScores ();
			foreach(string perm in AccessToken.CurrentAccessToken.Permissions) {
				// log each granted permission
				Debug.Log(perm);
				if (perm == "publish_actions") {
					publishAllowed = true;
				}
			}
		} else {
			if (debugging) {
				Debug.Log (result.Error);
			}
		}
	}

	public void QueryScores (IResult result) {
		//FB.API ("/app/scores?fields=score,user.limit(5)", HttpMethod.GET, ScoreCallback);
		FB.API ("/app/scores?fields=score", HttpMethod.GET, ScoreCallback);
	}

	void ScoreCallback (IResult result) {
		if (debugging) {
			if (result.Error != null) {
				Debug.Log (result.Error);
			} else {
				Debug.Log ("No error");
				Debug.Log (result.RawResult);
			}
			scoresList = Util.DeserializeScores (result.RawResult);
			scoresList.Reverse ();
			//FBscoresTest.text = "";
			scoresString = "LEADERBOARD\n\n";
			int i = 0;
			foreach (object score in scoresList) {
				if (i < 4) {
					i++;
					var entry = (Dictionary<string, object>)score;
					var user = (Dictionary<string, object>)entry ["user"];
					scoresString += user ["name"] + " scored " + entry ["score"] + " milliseconds\n\n";
					if (debugging) {
						Debug.Log (user ["name"] + " scored " + entry ["score"]);
						//FBscoresTest.text += user ["name"] + " scored " + entry ["score"] + "\n";
						//SetScore ();
					}
				}
			}
			hub.shockOrNotMessageText.text = scoresString;
			hub.submitFBbutton.SetActive (false);
			hub.shockOrNotOKButton.SetActive (true);
		}
	}

	public void SetScore (string thisScore) {
		if (!publishAllowed) {
			FB.LogInWithPublishPermissions (new List<string> (){ "publish_actions" }, AuthCallBack);
		}
		var scoreData = new Dictionary<string,string> ();
		scoreData ["score"] = thisScore; //Random.Range (10, 200).ToString ("0");
		FB.API ("me/scores", HttpMethod.POST, QueryScores, scoreData);
	}
}*/
