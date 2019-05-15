using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

	public SpriteRenderer render, surfaceRender, flagRender;
	int _type;

	public int type {
		get { return _type; }
		set {
			_type = value;
			if (type == 2) {
				render.sprite = GameDefine.instance.sprTile2;
				surfaceRender.sprite = GameDefine.instance.sprTileSurface2;
			} else if (type == 1) {
				render.sprite = GameDefine.instance.sprTile1;
				surfaceRender.sprite = GameDefine.instance.sprTileSurface1;
			}
			

		}
	}

	int sort;

	void OnEnable ()
	{
		bool activeFlag = Random.Range (0, 100) < GameDefine.instance.tileFlagPercent ? true : false;
		flagRender.gameObject.SetActive (activeFlag);
		flagRender.sprite = GameDefine.instance.listTileFlagSpr [Random.Range (0, GameDefine.instance.listTileFlagSpr.Count)];
	}

	void Start ()
	{
		int sort = -Mathf.RoundToInt (transform.position.y * 10);
		render.sortingOrder = sort;
		surfaceRender.sortingOrder = sort;
		flagRender.sortingOrder = sort;
	}

}
