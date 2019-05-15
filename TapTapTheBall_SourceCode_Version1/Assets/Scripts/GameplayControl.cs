using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//using UnityEditor;
using UnityEngine.UI;

public class GameplayControl : MonoBehaviour
{
	public GameObject ballShadow;
	[HideInInspector]
	public  Vector3 UR = new Vector3 (0.847F, 0.415F, 0);
	[HideInInspector]
	public  Vector3 UL = new Vector3 (-0.818F, 0.399F, 0);
	public Tile lastTile;
	public GameObject tileGroup;
	List<Tile> listTiles;
	public Ball ball;
	public GameObject startPlatform;
	float vscale = 2.041F;
	float moveSpeed = 1.2F;
	int _score;
	public Text tFlag, tScore;
	public Image iTeam, iSpeedUp;
	public LayerMask tileSufaceLayer;

	public int score {
		get { return _score; }
		set {
			_score = value;
			tScore.text = value.ToString ();
			if (value > 0 && value % 50 == 0) {
				IncreaseSpeed ();
			}
			if (value > GameManager.dataSave.bestScore) {
				GameManager.dataSave.bestScore = value;
				GameManager.SaveData ();
			}
		}
	}

	int _flag;

	public int flag {
		get { return _flag; }
		set {
			_flag = value;
		}

	}
	// other
	Vector3 startPlatformPos, startTilePos, startBallPos;
	public static GameplayControl instance;

	void Awake ()
	{
		instance = this;
		startBallPos = ball.transform.position;
		startPlatformPos = startPlatform.transform.position;
		startTilePos = lastTile.transform.position;
		listTiles = new List<Tile> ();
		listTiles.Add (lastTile);
		InitNewGame ();
		CreateCloud ();
	}

	void IncreaseSpeed ()
	{
		AudioManager.instance.PlaySound (AudioClipType.speedup);
		moveSpeed += GameDefine.instance.ballSpeedIncrease;
		iSpeedUp.gameObject.SetActive (true);
	}

	public void InitNewGame ()
	{
		// clear all tile 
		for (int i = 0; i < listTiles.Count; i++) {
			PoolManager.Clear (listTiles [i]);
		}
		listTiles.Clear ();
		// create start tile
		lastTile = PoolManager.Spawn<Tile> (startTilePos, Quaternion.Euler (0, 0, 0));
		// set startPlatform pos
		startPlatform.transform.position = startPlatformPos;
		listTiles.Add (lastTile);
		// create tile
		CreateStartTile ();
		// other 
		ballShadow.SetActive (true);
		ball.transform.position = startBallPos;
		ball.render.sortingOrder = 100;
		score = 0;
		moveSpeed = GameDefine.instance.ballStartSpeed;
		iTeam.sprite = GameDefine.instance.listTeamFlagSpr [GameManager.dataSave.selectedTeam];
		GameManager.state = GameState.MENU;
	}

	void CreateStartTile ()
	{
		// tạo tile khi vào game
		while (lastTile.transform.position.y < 10) {
			CreateNewTile ();
		}
	}
	// vector speed , dc tính sau mỗi update
	public Vector3 speed;
	// sortingOder của tile , dc tính sau mỗi update
	int sort;
	// raycase
	int rc;
	// mặt tile cuối cùng mà bóng chạm vào
	GameObject lastTileSurfaceUnderBall;


	void CheckBallStayOnTile ()
	{
		
		RaycastHit2D hit;
		Vector3 ballCheckPoint = ball.transform.position + new Vector3 (0, -0.28F, 0);
		hit = Physics2D.Raycast (ballCheckPoint, Vector2.zero, 100F, (int)tileSufaceLayer);
		if (hit) {			
			lastTileSurfaceUnderBall = hit.collider.gameObject;
			return;
		} else {
			ballShadow.SetActive (false);
			Debug.Log ("Lose");
			AudioManager.instance.PlaySound (AudioClipType.gameover);
			GameManager.state = GameState.GAMEOVER;
			// tính sortingOder của bóng so với tile khi bóng rơi khỏi platform
			if (lastTileSurfaceUnderBall != null) {
				SpriteRenderer surfaceRender = lastTileSurfaceUnderBall.GetComponent<SpriteRenderer> ();

				if (ballCheckPoint.y > lastTileSurfaceUnderBall.transform.position.y) {
					ball.render.sortingOrder = surfaceRender.sortingOrder - 1;
				} else {
					ball.render.sortingOrder = surfaceRender.sortingOrder + 1;
				}
			}
			Invoke ("InitNewGame", 3F);
		}
	

	}
	// hướng di chuyển của bóng
	int dir = 1;
	Vector3 shadowPos = new Vector3 (0, -0.28F, 0);

	void MoveBall ()
	{
		
		Vector3 ballSpeed = new Vector3 (moveSpeed * Time.deltaTime * vscale, 0, 0);
		switch (dir) {
		case 1:
			ball.transform.position += ballSpeed;
			ball.transform.Rotate (new Vector3 (0, 0, -4 - moveSpeed * 2));
			break;
		case -1:
			ball.transform.position -= ballSpeed;
			ball.transform.Rotate (new Vector3 (0, 0, 4 + moveSpeed * 2));
			break;
		}
		ballShadow.transform.position = ball.transform.position + shadowPos;
	}

	void ChangeBallDirection ()
	{
		if (Input.GetMouseButtonDown (0)) {
			AudioManager.instance.PlaySound (AudioClipType.kicktheball);
			dir = -dir;
			score++;
		}
	}

	void Update ()
	{
		// check nếu không bấm vào button thì play game nếu đang ở menu hoặc pause 
		if (Input.GetMouseButtonDown (0) && (GameManager.state == GameState.MENU || GameManager.state == GameState.PAUSING)) {
			bool canPlay = true;
			#if UNITY_EDITOR
			{
				if (EventSystem.current.IsPointerOverGameObject ()) {
					canPlay = false;
				}
			}
			#else
			if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
			{
				canPlay = false;
			}
			#endif
			if (canPlay) {
				GameManager.state = GameState.PLAYING;
			}
		}

		// 
		if (GameManager.state == GameState.PLAYING) {

			// di chuyển startPlatform và các tile
			speed = new Vector3 (0, -Time.deltaTime * moveSpeed, 0);
			if (startPlatform.transform.position.y < -6F) {
				startPlatform.transform.position += speed * 2;
			} else {
				startPlatform.transform.position += speed;
			}

			foreach (Tile t in listTiles.ToArray()) {
				if (t.transform.position.y < -5.5F) {
					t.transform.position += speed * 2;
				} else {
					t.transform.position += speed;
				}
				sort = -Mathf.RoundToInt (t.transform.position.y * 10);
				t.render.sortingOrder = sort;
				t.surfaceRender.sortingOrder = sort;
				t.flagRender.sortingOrder = sort + 1;
				if (t.transform.position.y < -7) {
					listTiles.Remove (t);
					PoolManager.Clear (t);
				}
			}
			// nếu thiếu tile thì tạo thêm trên cùng
			if (lastTile.transform.position.y < 10) {
				CreateNewTile ();
			}
			// check thay đổi hướng bóng di chuyển 
			ChangeBallDirection ();
			// di chuyển bóng
			MoveBall ();
			// kiểm tra bóng có trên mặt platform hay ko
			CheckBallStayOnTile ();
		} else if (GameManager.state == GameState.GAMEOVER) {
			// nêu thua thì cho bóng di chuyển xuống
			speed = new Vector3 (0, -Time.deltaTime * 5, 0);
			ball.transform.position += speed;
		}
	}

	void CreateNewTile ()
	{
		// chọn PointLeft hoặc PointRight
		Vector3 pR = lastTile.transform.position + UR;
		Vector3 pL = lastTile.transform.position + UL;

		Vector3 p = Random.Range (0, 100) > 50 ? pR : pL;
		p = p.x > 2.622F ? pL : p;
		p = p.x < -2.622F ? pR : p;

		// 
		Tile scr = PoolManager.Spawn<Tile> (p, Quaternion.Euler (0, 0, 0));
		// set màu cho tile
		scr.type = lastTile.type == 1 ? 2 : 1;

		scr.transform.SetParent (tileGroup.transform);
		listTiles.Add (scr);
		lastTile = scr;
	}

	public void Pause ()
	{
		GameManager.state = GameState.PAUSING;
	}

	public List<Cloud> listCloud;

	void CreateCloud ()
	{
		listCloud = new List<Cloud> ();
		for (int i = 0; i < 4; i++) {
			GameObject cl = (GameObject)Instantiate (GameDefine.instance.cloudPrefap, new Vector3 (10, 10, 10), Quaternion.Euler (0, 0, 0));
			Cloud _cl = cl.GetComponent<Cloud> ();
			listCloud.Add (_cl);
		}
	}
}
