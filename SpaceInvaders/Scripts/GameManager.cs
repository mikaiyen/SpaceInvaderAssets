using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // graffiti
    public Text uiText;

    public Text bulletcount;

    public decimal acc;
    public int accOutput;

    public string nowstate;

    // Reference to the AudioSource component
    public AudioManager am;

    //states
    enum State { NotStarted, Playing, GameOver, WonGame }

    // current state
    State currState;

    // Enemy Manager
    PlayerController pc;

    // Enemy Manager
    EnemyManager enemyManager;
    BossManager bm;
    // Start is called before the first frame update
    void Start()
    {

        if (SceneManager.GetActiveScene().name=="Prison"){
            currState = State.GameOver;
        }
        else{
            // start as not playing
            currState = State.NotStarted;
        }

        // refresh UI
        RefreshUI();

        am = GameObject.FindObjectOfType<AudioManager>();

        // find the enemy manager
        enemyManager = GameObject.FindObjectOfType<EnemyManager>();
        bm = GameObject.FindObjectOfType<BossManager>();

        // find the player controller
        pc = GameObject.FindObjectOfType<PlayerController>();


        pc.numBullets=0;

        // log error if it wasn't found
        if(enemyManager == null)
        {
            Debug.LogError("there needs to be an EnemyManager in the scene");
        }
    }

    void RefreshUI()
    {
        // act according to the state
        switch(currState)
        {
            case State.NotStarted:
                uiText.text = "Shoot here to begin";
                nowstate="NotStarted";
                break;

            case State.Playing:
                uiText.text = "Enemies left: " + enemyManager.numEnemies;
                bulletcount.text = pc.numBullets + " bullets shoot";
                nowstate="Playing";
                break;

            case State.GameOver:
                //acc=Math.Round((decimal)(enemyManager.totalenemiescount-enemyManager.numEnemies)/ pc.numBullets,2)*100;
                //accOutput=Decimal.ToInt32(acc);
                uiText.text = "Game Over! Shoot here";
                //bulletcount.text = "Your accuracy is: "+ accOutput +"%";
                nowstate="GameOver";
                break;

            case State.WonGame:
                acc=Math.Round((decimal)(enemyManager.totalenemiescount-enemyManager.numEnemies)/ pc.numBullets,2)*100;
                accOutput=Decimal.ToInt32(acc);
                if(SceneManager.GetActiveScene().name=="FinalLevel"){
                    uiText.text = "YOU Complete the game!";
                }
                else{
                    uiText.text = "YOU WON! Shoot here to next level";
                }
                bulletcount.text = "Your accuracy is: "+ accOutput +"%";
                nowstate="WonGame";
                break;
        }  
    }

    public void InitGame()
    {
        //don't initiate the game if the game is already running!
        if (currState == State.Playing) return;

        // set the state
        currState = State.Playing;

        // create enemy wave
        enemyManager.CreateEnemyWave();


        pc.numBullets=0;

        am.switchbgm(am.fightbgm);

        // show text on the graffiti
        RefreshUI();
    }

    // game over
    public void GameOver()
    {
        // do nothing if we were already on game over
        if (currState == State.GameOver) return;

        // set the state to game over
        currState = State.GameOver;

        am.switchbgm(am.losebgm);

        // show text on the graffiti
        RefreshUI();

        // remove all enemies
        enemyManager.KillAll();

        //load to prison
        SceneManager.LoadScene("Prison");
    }

    // checks whether we've won, and if we did win, refresh UI
    public void HandleEnemyDead()
    {
        if (currState != State.Playing) return;

        RefreshUI();

        // have we won the game?
        if(enemyManager.numEnemies <= 0)
        {
            if(SceneManager.GetActiveScene().name!="FinalLevel"){
                // set the state of the game
                currState = State.WonGame;

                if(SceneManager.GetActiveScene().name=="FinalLevel"){
                    am.switchbgm(am.victorybgm);
                }
                else{
                    am.switchbgm(am.winbgm);
                }

                // show text on the graffiti
                RefreshUI();

                // remove all enemies
                enemyManager.KillAll();
            }
            else if(SceneManager.GetActiveScene().name=="FinalLevel"){
                //create boss
                bm.CreateBossWave();
                am.switchbgm(am.bossbgm);
            }

        }
    }
    public void HandleBossDead()
    {
        if (currState != State.Playing) return;

        RefreshUI();

        // have we won the game?
        if(enemyManager.numEnemies <= 0 && bm.numBosses<=0)
        {
            // set the state of the game
            currState = State.WonGame;

            if(SceneManager.GetActiveScene().name=="FinalLevel"){
                am.switchbgm(am.victorybgm);
            }
            else{
                am.switchbgm(am.winbgm);
            }

            // show text on the graffiti
            RefreshUI();

            // remove all enemies
            enemyManager.KillAll();
            bm.KillAll();

        }
    }
}
