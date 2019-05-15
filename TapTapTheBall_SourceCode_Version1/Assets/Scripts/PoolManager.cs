using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{

	void Awake ()
	{
		poolDict = new Dictionary<string, List<Component>> ();
		CreateGameobject <Tile> (tilePrefap, 50);
	}

	public GameObject tilePrefap;

	public static Dictionary<string,List<Component>> poolDict;
	public static Vector3 poolPos = new Vector3 (10, 10, 0);

	public static void  CreateGameobject <T> (GameObject obj, int num) where T : Component
	{
		string t = typeof(T).ToString ();
		for (int i = 0; i < num; i++) {
			GameObject o = (GameObject)Instantiate (obj, poolPos, Quaternion.Euler (0, 0, 0));
			T scr = o.GetComponent<T> ();
			if (poolDict.ContainsKey (t)) {
				poolDict [t].Add (scr);
			} else {
				List<Component> l = new List<Component> ();
				l.Add (scr);
				poolDict.Add (t, l);
			}
			o.SetActive (false);
		}
		//Debug.Log ("So object la : " + poolDict [t].Count);
	}

	public static T Spawn<T> (Vector3 pos, Quaternion rot) where T : Component
	{
		string t = typeof(T).ToString ();
		//Debug.Log ("Ten cua t la : " + t);
		T scr = poolDict [t] [0] as T;
		//Debug.Log ("Scr la : " + scr.ToString ());
		scr.transform.position = pos;
		scr.transform.rotation = rot;
		scr.gameObject.SetActive (true);
		poolDict [t].Remove (scr);
		return scr;
	}

	public static void Clear (Component _scr)
	{
		string t = _scr.GetType ().ToString ();
		if (!poolDict [t].Contains (_scr)) {
			poolDict [t].Add (_scr);
			_scr.transform.position = poolPos;
			_scr.gameObject.SetActive (false);
		}
	}

}
