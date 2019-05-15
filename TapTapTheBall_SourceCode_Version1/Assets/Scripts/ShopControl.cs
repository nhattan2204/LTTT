using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopControl : MonoBehaviour
{
	public GameObject iSelect;
	public List<ShopItem> listItem;
	public Text tFlag;
	public static ShopControl instance;
	ShopItem _selectedItem;

	public ShopItem selectedItem {
		get { return _selectedItem; }
		set {
			_selectedItem = value;
			iSelect.transform.position = value.transform.position;
			GameplayControl.instance.iTeam.sprite = GameDefine.instance.listTeamFlagSpr [value.id];
			GameManager.dataSave.selectedTeam = value.id;
		}
	}

	void Awake ()
	{
		instance = this;
		for (int i = 0; i < listItem.Count; i++) {
			listItem [i].id = i;
			listItem [i].SetUnlocked ();
			listItem [i].bt.image.sprite = GameDefine.instance.listTeamFlagItemSpr [i];
		}
	}

	public void UpdateFlagText ()
	{
		tFlag.text = GameManager.dataSave.flag.ToString ();
	}

	void OnEnable ()
	{
		
		UpdateFlagText ();
	}

	public void CloseShop ()
	{
		AudioManager.instance.PlaySound (AudioClipType.mainbutton);
		GameManager.state = GameState.MENU;
	}
}
