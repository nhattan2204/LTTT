using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	GameState _state;
	public static DataSaveModule dataSave;

	void Awake ()
	{
		instance = this;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.Quit(); 
	}
	public static GameState state {
		get { return instance._state; }
		set {
			GameState oldValue = instance._state;
			instance._state = value;
			switch (value) {
			case GameState.GAMEOVER: 
				GameManager.SaveData ();
				GameDefine.instance.popupGameOver.SetActive (true);
				AdsControl.Instance.showAds ();
				AdsControl.Instance.HideBanner ();
				break;
			case GameState.PAUSING:
				GameDefine.instance.popupPause.SetActive (true);
				AdsControl.Instance.showAds ();
				break;
			case GameState.PLAYING: 				
				GameDefine.instance.popupGameOver.SetActive (false);
				GameDefine.instance.popupMenu.SetActive (false);
				GameDefine.instance.popupPause.SetActive (false);
				GameDefine.instance.popupGameplay.SetActive (true);

				break;
			case GameState.SHOPPING:
				
				GameDefine.instance.popupMenu.SetActive (false);
				GameDefine.instance.popupShop.SetActive (true);

				break;
			case GameState.MENU:
				GameDefine.instance.popupGameOver.SetActive (false);
				GameDefine.instance.popupMenu.SetActive (true);
				GameDefine.instance.popupPause.SetActive (false);
				GameDefine.instance.popupGameplay.SetActive (false);
				GameDefine.instance.popupShop.SetActive (false);
				AdsControl.Instance.ShowBanner ();
				break;
			}
		}
	}

	public static void LoadDataSave ()
	{
		string json = PlayerPrefs.GetString ("Data", "");
		//json = "";
		if (json != "") {
			dataSave = JsonUtility.FromJson<DataSaveModule> (json);
		} else {
			dataSave = new DataSaveModule ();
			dataSave.bestScore = 0;
			dataSave.flag = 0;
			dataSave.listTeam = new List<bool> ();
			dataSave.listTeam.Add (true);
			for (int i = 0; i < 10; i++) {
				dataSave.listTeam.Add (false);
			}
			dataSave.selectedTeam = 0;
			SaveData ();
		}
	}

	public static void SaveData ()
	{
		string json = JsonUtility.ToJson (dataSave);
		PlayerPrefs.SetString ("Data", json);
		PlayerPrefs.Save ();
	}

}

[System.Serializable]
public enum GameState
{
	PLAYING,
	PAUSING,
	GAMEOVER,
	SHOPPING,
	MENU
}