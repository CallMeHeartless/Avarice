﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    static AudioController instance;

    private AudioSource[] sounds;

	// Use this for initialization
	void Start () {
        instance = this;
        sounds = GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    // Plays a single sound
    public static void PlaySingleSound(string sound) {
        AudioSource audio = instance.GetSource(sound);
        if (audio) {
            if (!audio.isPlaying) {// Stop looping sounds from starting up again
                audio.Play();
                Debug.Log("Playing: " + sound);
            } else {
                Debug.Log("Already playing sound");
            }
        } 
    }

    // Stops a sound (assumed to be looping)
    public static void StopSingleSound(string sound) {
        AudioSource audio = instance.GetSource(sound);
        if (audio) {
            if (audio.isPlaying) {
                audio.Stop();
                Debug.Log("Stopping: " + sound);
            }
        } else {
            Debug.Log("Sound not found - null reference.");
        }
    }

    private AudioSource GetSource(string sound) {
        foreach(AudioSource audio in sounds) {
            if(audio.clip.name == sound) {
                return audio;
            }
        }

        return null;
    }

    public static void ConfirmSkillAudio() {

        int index = Random.Range(1, 6);
        string audioName = "ConfirmSkill0" + index.ToString();
        PlaySingleSound(audioName);
    }

    public static void TurnInGoldAudio() {
        int index = Random.Range(1, 5);
        string audioName = "GoldDeposit0" + index.ToString();
        PlaySingleSound(audioName);
    }

    public static void GameStartAudio() {
        int index = Random.Range(1, 3);
        string audioName = "GameStart0" + index.ToString();
        PlaySingleSound(audioName);
    }

    public static void UpgradeMenuWelcome() {
        int index = Random.Range(1, 4);
        string audioName = "PerkMenu0" + index.ToString();
        PlaySingleSound(audioName);
    }

    public static void PlayerPain() {
        int index = Random.Range(0, 3);
        if(index == 0) {
            PlaySingleSound("GRUNT_Male_Quick_mono");
        }else if(index == 1) {
            PlaySingleSound("GRUNT_Male_Quick_Deep_mono");
        }else if(index == 2) {
            PlaySingleSound("GRUNT_Male_B_Hurt_Short_04_mono");
        }
    }

    public static void HitSkeletonAudio() {

    }

}
