using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
	public Text tBest, tFlag;

	void OnEnable ()
	{
		AudioManager.instance.PlaySound (AudioClipType.introtitle);
		tBest.text = GameManager.dataSave.bestScore.ToString ();
		tFlag.text = GameManager.dataSave.flag.ToString ();
	}

	public void ShowShop ()
	{
		AudioManager.instance.PlaySound (AudioClipType.mainbutton);
		GameManager.state = GameState.SHOPPING;
	}
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.Quit(); 
	}

}
