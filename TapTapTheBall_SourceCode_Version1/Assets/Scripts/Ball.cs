using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

	public SpriteRenderer render;


	void Awake ()
	{
		render = GetComponent<SpriteRenderer> ();
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.CompareTag ("TileFlag")) {
			AudioManager.instance.PlaySound (AudioClipType.pickupaflag);
			col.gameObject.SetActive (false);
			GameManager.dataSave.flag++;
			GameManager.SaveData ();
		}
	}

}
