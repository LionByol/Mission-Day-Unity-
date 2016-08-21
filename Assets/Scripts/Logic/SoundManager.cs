using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {
	
	public enum eSoundType {
		AudioEffect,
		MusicSound
	}
	
	public static bool effectsEnabled { get { return (GameManager.instance.prefOptionEffects > 0); } }
	public static bool musicEnabled { get { return (GameManager.instance.prefOptionMusic > 0); } }
	
	public static Dictionary<eSoundType, List<AudioSource>> audioSources = new Dictionary<eSoundType, List<AudioSource>>();
	
	void Start() {
		InitSources();
	}
	
	public static void InitSources () {
		audioSources = new Dictionary<eSoundType, List<AudioSource>>();
		audioSources[eSoundType.AudioEffect] = new List<AudioSource>();
		audioSources[eSoundType.MusicSound] = new List<AudioSource>();
	}
	
	public static void AddEffect(AudioSource source) {
		if (source != null)
			audioSources[eSoundType.AudioEffect].Add(source);
	}
	public static void RemoveEffect(AudioSource source) {
		if (source != null)
			audioSources[eSoundType.AudioEffect].Remove(source);
	}
	public static void AddMusic(AudioSource source) {
		if (source != null)
			audioSources[eSoundType.MusicSound].Add(source);
	}
	public static void RemoveMusic(AudioSource source) {
		if (source != null)
			audioSources[eSoundType.MusicSound].Remove(source);
	}
	
	public static void ApplyVolume() {
		foreach (AudioSource source in audioSources[eSoundType.AudioEffect]) {
			source.volume = GameManager.instance.prefOptionEffects;
			if (source.volume == 0)
				source.Stop();
			else
				PlayEffect(source);
		}
		foreach (AudioSource source in audioSources[eSoundType.MusicSound]) {
			source.volume = GameManager.instance.prefOptionMusic;
			if (source.volume == 0)
				source.Stop();
			else
				PlayMusic(source);
		}
	}
	
	public static void PlayEffect (AudioClip audio) {
		//if (effectsEnabled)
		NGUITools.PlaySound(audio, GameManager.instance.prefOptionEffects);
	}
	public static void PlayEffect (AudioSource audio) {
		//if (effectsEnabled) {
		audio.volume = GameManager.instance.prefOptionEffects;
		if (!audio.isPlaying)
			audio.Play();
		//}
	}
	
	public static void PlayMusic (AudioSource audio) {
		//if (musicEnabled) {
		audio.volume = GameManager.instance.prefOptionMusic;
		if (!audio.isPlaying)
			audio.Play();
		//}
	}
}
