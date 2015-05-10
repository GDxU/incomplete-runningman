using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.UI;
public class LoginGame : BaseLogin {

	public Text textview;
	protected override void EndStandBy ()
	{
	//	throw new System.NotImplementedException ();
	}

	protected override void OnGotInvitation (Invitation invitation, bool shouldAutoAccept)
	{
		//throw new System.NotImplementedException ();
	}

	protected override void OnGotMatch (GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, bool shouldAutoLaunch)
	{
		//throw new System.NotImplementedException ();
	}

	protected override void setDisplayText (string strDisplay)
	{
		textview.text = strDisplay;
	}
	protected override void SwitchToMain ()
	{
		//throw new System.NotImplementedException ();
	}
}
