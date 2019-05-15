using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;
	public Sprite sprSoundOn, sprSoundOff, sprMusicOn, sprMusicOff;
	Dictionary<AudioClipType,AudioClip> audioDic;
	public List<AudioStruct> listAudioStruct;

	void Awake ()
	{
		instance = this;
		audioDic = new Dictionary<AudioClipType, AudioClip> ();
		for (int i = 0; i < listAudioStruct.Count; i++) {
			if (!audioDic.ContainsKey (listAudioStruct [i].type)) {
				audioDic.Add (listAudioStruct [i].type, listAudioStruct [i].clip);
			}
		}
	}

	void Start ()
	{
		//PlayMusic (AudioClipType.AC_MAIN_MUSIC);
		//AS_SOUND.volume = GameManager.dataSave.isSoundOn == true ? 1F : 0;
		//AS_MUSIC.volume = GameManager.dataSave.isMusicOn == true ? 1F : 0;
	}

	public AudioSource AS_SOUND, AS_MUSIC;

	[System.Serializable]
	public struct AudioStruct
	{
		public AudioClipType type;
		public AudioClip clip;
	}



	public Sprite GetSoundStatus ()
	{
		Sprite spr = GameManager.dataSave.isSoundOn == true ? sprSoundOn : sprSoundOff;
		if (GameDefine.instance.bSound != null) {
			GameDefine.instance.bSound.image.sprite = spr;
		}
		return spr;
	}

	public Sprite GetMusicStatus ()
	{
		
		Sprite spr = GameManager.dataSave.isMusicOn == true ? sprMusicOn : sprMusicOff;
		if (GameDefine.instance.bSound != null) {
			GameDefine.instance.bSound.image.sprite = spr;
		}
		return spr;
	}

	public Sprite SetMusicStatus (bool isOn)
	{
		AS_MUSIC.volume = isOn == true ? 1 : 0;
		GameManager.dataSave.isMusicOn = isOn;
		return GetMusicStatus ();
	}

	public Sprite SetSoundStatus (bool isOn)
	{
		AS_SOUND.volume = isOn == true ? 1 : 0;
		GameManager.dataSave.isSoundOn = isOn;
		return GetMusicStatus ();
	}

	public  void ChangeSoundStatus ()
	{
		Debug.Log ("aaa");
		GameManager.dataSave.isSoundOn = !GameManager.dataSave.isSoundOn;
		AS_SOUND.volume = GameManager.dataSave.isSoundOn == true ? 1 : 0;
		GameManager.dataSave.isMusicOn = !GameManager.dataSave.isMusicOn;
		AS_MUSIC.volume = GameManager.dataSave.isMusicOn == true ? 1F : 0;
		GameManager.SaveData ();
		GetSoundStatus ();
	}

	public  void ChangeMusicStatus ()
	{
		GameManager.dataSave.isMusicOn = !GameManager.dataSave.isMusicOn;
		AS_MUSIC.volume = GameManager.dataSave.isMusicOn == true ? 1F : 0;
		GameManager.dataSave.isSoundOn = !GameManager.dataSave.isSoundOn;
		AS_SOUND.volume = GameManager.dataSave.isSoundOn == true ? 1 : 0;
		GameManager.SaveData ();
		GetSoundStatus ();
	}

	public void PlaySound (AudioClipType type)
	{
		AS_SOUND.PlayOneShot (audioDic [type]);
	}

	public void PlayMusic (AudioClipType type)
	{
		AS_MUSIC.Stop ();
		AS_MUSIC.clip = audioDic [type];
		AS_MUSIC.Play ();
	}

	public void PlaySoundTime (AudioClipType type, float startTime, float stopTime)
	{
		
		/* Create a new audio clip */
		AudioClip clip = audioDic [type]; 
		int frequency = clip.frequency;
		float timeLength = stopTime - startTime;
		int samplesLength = (int)(frequency * timeLength);
		AudioClip newClip = AudioClip.Create (clip.name + "-sub", samplesLength, 1, frequency, false);
		/* Create a temporary buffer for the samples */
		float[] data = new float[samplesLength];
		/* Get the data from the original clip */
		clip.GetData (data, (int)(frequency * startTime));
		/* Transfer the data to the new clip */
		newClip.SetData (data, 0);
		/* Return the sub clip */
		AS_SOUND.PlayOneShot (newClip);
		//return newClip;

	}
}


[System.Serializable]
public enum AudioClipType
{
	bgm,
	gameover,
	introtitle,
	kicktheball,
	mainbutton,
	pickupaflag,
	speedup
}