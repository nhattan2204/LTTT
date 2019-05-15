using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
	public int price;
	public Text tPrice;
	public Image iBought;
	public Button bt;
	public int id;

	void Awake ()
	{
		price = 25;
		tPrice.text = "25";
		bt.onClick.AddListener (OnClick);
	}

	void OnEnable ()
	{
		SetUnlocked ();
	}

	public 	void SetUnlocked ()
	{
		if (GameManager.dataSave.listTeam [id] == true) {
			tPrice.gameObject.SetActive (false);
			iBought.gameObject.SetActive (true);
		} else {
			tPrice.gameObject.SetActive (true);
			iBought.gameObject.SetActive (false);
		}
	}

	public void OnClick ()
	{
		if (ShopControl.instance.selectedItem != this) {			
		
			if (GameManager.dataSave.listTeam [id] == false) {
				// neu du tien 
				if (GameManager.dataSave.flag >= price) {
					//mua 
					GameManager.dataSave.flag -= price;
					GameManager.dataSave.listTeam [id] = true;
					GameManager.SaveData ();
					ShopControl.instance.selectedItem = this;
					SetUnlocked ();
					ShopControl.instance.UpdateFlagText ();
				}
			} else {
				ShopControl.instance.selectedItem = this;
			}
		
		}

	}
}
