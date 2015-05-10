using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine;

public abstract class BaseLogin : MonoBehaviour
{
	public int mtimeout = 10;
	protected System.Action<bool> mAuthCallback;
	protected bool mAuthOnStart = true;
	protected bool mInMatch = false;
	// Use this for initialization
	void Start ()
	{
		Screen.orientation = ScreenOrientation.Portrait;
		mAuthCallback = (bool success) => {
			EndStandBy ();
			if (success && !mInMatch) {
				SwitchToMain ();
			}
		};

		PlayGamesClientConfiguration config = 
			new PlayGamesClientConfiguration.Builder ()
				.WithInvitationDelegate (OnGotInvitation)
				.WithMatchDelegate (OnGotMatch)
				.Build ();


		
		PlayGamesPlatform.InitializeInstance (config);
		// make Play Games the default social implementation
		PlayGamesPlatform.Activate ();
		
		// enable debug logs (note: we do this because this is a sample; on your production
		// app, you probably don't want this turned on by default, as it will fill the user's
		// logs with debug info).
		PlayGamesPlatform.DebugLogEnabled = true;
		
		// try silent authentication
		if (mAuthOnStart) {
			setDisplayText ("Please wait signing in...");
			PlayGamesPlatform.Instance.Authenticate (mAuthCallback, true);
			StartCoroutine ("timeout");
		}
	}

	private IEnumerator  timeout ()
	{
	
		yield return new WaitForSeconds (mtimeout);
		EndStandBy ();
		SwitchToMain ();
	
	}

	protected abstract void EndStandBy ();

	protected abstract void setDisplayText (string strDisplay);

	protected abstract void SwitchToMain ();

	protected abstract  void OnGotInvitation (Invitation invitation, bool shouldAutoAccept) ;

	protected abstract  void OnGotMatch (TurnBasedMatch match, bool shouldAutoLaunch) ;
	// Update is called once per frame
}
