using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{

	public SpriteRenderer render;
	int _id;

	public int id {
		get { return _id; }
		set {
			_id = value;
			render.sprite = GameDefine.instance.listCloudSpr [value];
		}
	}

	float dir = 1;
	float speed = 0;

	void Start ()
	{
		float x = Random.Range (0, 100) < 50 ? 7 : -7;
		transform.position = new Vector3 (x, 0, 0);
		Init ();
	}

	Vector3 moveDir = new Vector3 (0, 0, 0);

	void Init ()
	{
		id = Random.Range (0, GameDefine.instance.listCloudSpr.Count);
		float _y = Random.Range (-7, 7);
		transform.position = new Vector3 (transform.position.x, _y, 0);
		if (transform.position.x < -6) {
			dir = 1;
		} else if (transform.position.x > 6) {
			dir = -1;
		}
		speed = Random.Range (0.5F, 2F);
		moveDir = new Vector3 (dir * speed * 2.041F * Time.deltaTime, dir * speed * Time.deltaTime, 0);		
		float scaleRnd = Random.Range (0.2F, 1);
		transform.localScale = new Vector3 (scaleRnd, scaleRnd, 1);
	}

	void Update ()
	{
		if ( GameManager.state == GameState.PLAYING)
		{
		transform.position += moveDir+GameplayControl.instance.speed;
		}else
		{
			transform.position += moveDir;
		}
		if (transform.position.x > 7 || transform.position.x < -7) {
			Init ();
		}

	}
}
