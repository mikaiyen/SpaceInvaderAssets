using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------Audio Source---------")]
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource sfx;
    [Header("---------Audio Clip---------")]
    public AudioClip shoot;
    public AudioClip mainbgm;
    public AudioClip fightbgm;
    public AudioClip winbgm;
    public AudioClip losebgm;

    private void Start(){
        bgm.clip=mainbgm;
        bgm.Play();
    }

    public void playSFX(AudioClip clip){
        sfx.PlayOneShot(clip);
    }

    public void switchbgm(AudioClip music){
        bgm.clip=music;
        bgm.Play();
    }


}

