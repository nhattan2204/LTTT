using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDefine : MonoBehaviour
{

	public static GameDefine instance;

	void Awake ()
	{
		instance = this;
		GameManager.LoadDataSave ();
	}

	void Start ()
	{
		GameManager.state = GameState.MENU;
		AudioManager.instance.GetSoundStatus ();
		AudioManager.instance.PlayMusic (AudioClipType.bgm);
	}

	[Range (0, 100)]
	[Tooltip ("Phần trăm xuất hiện cờ trên tile")]
	public int tileFlagPercent = 30;
	[Range (1F, 3F)]
	[Tooltip ("Tốc độ di chuyển của bóng khi bắt đầu game")]
	public float ballStartSpeed = 1.2F;
	[Range (0.1F, 1F)]
	[Tooltip ("Gia tốc của bóng sau mỗi 50 điểm")]
	public float ballSpeedIncrease = 0.2F;

	public List<Sprite> listTeamFlagItemSpr;
	public List<Sprite> listTeamFlagSpr;
	public List<Sprite> listTileFlagSpr;
	public List<Sprite> listCloudSpr;
	public Button bSound;
	public GameObject cloudPrefap;
	public GameObject popupGameOver, popupPause, popupShop, popupGameplay, popupMenu;
	public GameObject tilePrefap;
	public Sprite sprTile1, sprTileSurface1, sprTile2, sprTileSurface2;
}
