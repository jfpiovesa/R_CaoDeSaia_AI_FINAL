using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public  enum enemyStage
{
    Idle,
    Alert,
    Explore,
    Patrol,
    Fallow,
    Fury,
    Die

}
public enum GameState
{
    GamePlay , Die
}
public class GameManager : MonoBehaviour
{
    public GameState gameState;

    [Header("Slime ia")]
    public GameObject[] slimesWayPoints;
    public float slmileIsWaitTime;
    public float slimedistanceToAttack = 2.3f;
    public float slimeAlertTIme = 3f;
    public float slimeAtackDelay = 1f;
    public float slimeLookAtRotate = 1f;
    [Header("Player")]
    public GameObject player;
    public int gens;
    public Text textGens;

    [Header("Rain Manager")]
    public PostProcessVolume postNIght;
    public ParticleSystem rain;
    private ParticleSystem.EmissionModule rainMOdule;
    public int rainRateOverTime;
    public int rainIncrement;
    public float rainIncrementDelay;

    [Header("Drop")]
    public GameObject gensPrefabs;
    public int percDrop = 25;

    // Start is called before the first frame update
    void Start()
    {
        textGens.text = gens.ToString();
        rainMOdule = rain.emission;
        player = GameObject.FindGameObjectWithTag("Player");
        slimesWayPoints = GameObject.FindGameObjectsWithTag("SlimeWayPoints");
    }

   public void onOfRain(bool isRain)
    {
        StartCoroutine("RainManager",true);
        StartCoroutine("PosManager",true);
        StartCoroutine("RainManager",isRain);
        StartCoroutine("PosManager", isRain);
    }
    IEnumerator RainManager(bool isRain)
    {
        switch(isRain)
        {
            case true:

                for(float r =  rainMOdule.rateOverTime.constant; r < rainRateOverTime; r += rainIncrement)
                {
                    rainMOdule.rateOverTime = r;
                    yield return new WaitForSeconds(rainIncrementDelay);
                }
                rainMOdule.rateOverTime = rainRateOverTime;

                break;

            case false:
                for (float r = rainMOdule.rateOverTime.constant; r > 0; r -= rainIncrement)
                {
                    rainMOdule.rateOverTime = r;
                    yield return new WaitForSeconds(rainIncrementDelay);
                }
                rainMOdule.rateOverTime = 0;
                break;
        }


    }

    IEnumerator PosManager(bool isRain)
    {
        switch (isRain)
        {
            case true:

                for ( float w = postNIght.weight; w < 1; w += 1 * Time.deltaTime )
                {
                    postNIght.weight = w;
                    yield return new WaitForEndOfFrame();
                }
                postNIght.weight = 1;

                break;

            case false:
                for (float w = postNIght.weight; w > 0; w -= 1 * Time.deltaTime)
                {
                    postNIght.weight = w;
                    yield return new WaitForEndOfFrame();
                }
                postNIght.weight = 0;


                break;
        }
    }


    public void ChangerGameState(GameState newgameState)
    {
        gameState = newgameState;

    }
    public void GetGens(int amout)
    {
        gens += amout;
        textGens.text = gens.ToString();
    }

    public bool Perc(int p)
    {
        int temp = Random.Range(0, 100);
        bool retorno = temp <= p ? true : false;
        return retorno;

    }
}
