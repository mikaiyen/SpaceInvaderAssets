using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour
{
    [Header("---------Audio Source---------")]
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource sfx;
    [Header("---------Audio Clip SF---------")]
    public AudioClip gun1shoot;
    public AudioClip gun2shoot;
    public AudioClip enemydeath;
    public AudioClip arrowShoot;
    public AudioClip teleportSound;

    [Header("---------Audio Clip BGM---------")]
    public AudioClip mainbgm;
    public AudioClip fightbgm;
    public AudioClip bossbgm;
    public AudioClip winbgm;
    public AudioClip losebgm;
    public AudioClip victorybgm;

    private void Start(){
        if (SceneManager.GetActiveScene().buildIndex==0){
            bgm.clip=losebgm;
        }
        else{
            bgm.clip=mainbgm;
        }
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

