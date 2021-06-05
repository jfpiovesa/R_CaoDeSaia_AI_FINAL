using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

public class SlimeA : MonoBehaviour
{
    private GameManager _gameManager;
    private NavMeshAgent _nav;
    private Vector3 desti;
    private int idWayPoints;

    Animator anima;
    public ParticleSystem fxSmile;  
    private int hp =2;
    private bool isDied =  true;
    private bool isAlert;
    private bool isWalk;
    private bool isPlayVisible;
    private bool isAtack;

    public enemyStage state;
   

    private int rang;
    // Start is called before the first frame update
    void Start()
    {
       
        _nav = GetComponent<NavMeshAgent>();
        _gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        anima = GetComponent<Animator>();
        ChangerState(state);
    }

    // Update is called once per frame
    void Update()
    {
       

        if(_nav.velocity.magnitude > 0.1f)
        {
            isWalk = true;
        }
        else
        {
            isWalk = false;
        }

        anima.SetBool("isWalk", isWalk);
        anima.SetBool("isAlert", isAlert);
    }

    private void LateUpdate()
    {
        StateManager();
    }

    #region meu metodos

    IEnumerator Morte()
    {
        isDied = false;
        yield return new WaitForSeconds(1.5f);
        if(_gameManager.Perc(_gameManager.percDrop) == true)
        {
            Instantiate(_gameManager.gensPrefabs, transform.position,Quaternion.identity);
        }

        Destroy(this.gameObject);

    }
    void GetHit(int amout)
    {
        if(isDied == false)  return; 

        hp -= amout;

        if (hp > 0)
        {
           
            anima.SetTrigger("GetHit");
            fxSmile.Emit(10);
            ChangerState(enemyStage.Fury);
        }
        else
        {
            ChangerState(enemyStage.Die);
            anima.SetTrigger("Die");
            StartCoroutine("Morte");
          

        }
    
    }

    void StateManager()
    {
        switch(state)
        {

            case enemyStage.Alert:

                LookAt();
                break;
            case enemyStage.Fallow:
                LookAt();

                desti = _gameManager.player.transform.position;
                _nav.destination = desti;

                if(_nav.remainingDistance <= _nav.stoppingDistance)
                {
                    Atack();
                }


                break;
                   
            case enemyStage.Fury:
                LookAt();

                desti = _gameManager.player.transform.position;
                _nav.destination = desti;
                if (_nav.remainingDistance <= _nav.stoppingDistance)
                {
                    Atack();
                }

                break;


        }
    }
    void ChangerState( enemyStage newenemyStage)
    {
        StopAllCoroutines();// encerra todas coroutines 
       
        isAlert = false;
        switch (newenemyStage)
        {

            case enemyStage.Idle:
                _nav.stoppingDistance = 0;
                desti = transform.position;
                _nav.destination = desti;

                StartCoroutine("Idle");

                break;
            case enemyStage.Alert:

                _nav.stoppingDistance = 0;
                desti = transform.position;
                _nav.destination = desti;
                isAlert = true;
                StartCoroutine("Alert");


                break;
            case enemyStage.Patrol:
                _nav.stoppingDistance = 0;
                idWayPoints = Random.Range(0, _gameManager.slimesWayPoints.Length);
                desti = _gameManager.slimesWayPoints[idWayPoints].transform.position;
                _nav.destination = desti;
                StartCoroutine("Patrol");
                break;
            case enemyStage.Fury:

                desti = transform.position;
                _nav.stoppingDistance = _gameManager.slimedistanceToAttack  ;
                _nav.destination = desti;
                break;

            case enemyStage.Fallow:

                _nav.stoppingDistance = _gameManager.slimedistanceToAttack;


                break;
            case enemyStage.Die:
               desti = transform.position;
                _nav.destination = desti;

                break;

        }
        state = newenemyStage;

    }
    IEnumerator Idle()
    {
        yield return new WaitForSeconds(_gameManager.slmileIsWaitTime);
        StateStill(50);
    }
    IEnumerator Patrol()
    {
        yield return new WaitUntil(() => _nav.remainingDistance <= 0);
        StateStill(30);
    }
    IEnumerator Alert()
    {
        yield return new WaitForSeconds(_gameManager.slimeAlertTIme);
        if( isPlayVisible == true)
        {
            ChangerState(enemyStage.Fallow);
        }
        else
        {
            StateStill(10);
        }
    }
    IEnumerator AtackDelay()
    {

        yield return new WaitForSeconds(_gameManager.slimeAtackDelay);
        isAtack = false;
    }

    int Range()
    {
        rang = Random.Range(0, 100);
        return rang;
    }
    void StateStill(int yes)
    {
        if (Range() <= yes)
        {
            ChangerState(enemyStage.Idle);

        }
        else
        {
            ChangerState(enemyStage.Patrol);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
       if (_gameManager.gameState == GameState.Die && (state  == enemyStage.Fallow|| state == enemyStage.Fury|| state == enemyStage.Alert )) { ChangerState(enemyStage.Idle); }

        if (other.gameObject.tag == ("Player"))
        {

            isPlayVisible = true;
            if (state == enemyStage.Idle || state == enemyStage.Patrol)
            {
                ChangerState(enemyStage.Alert);
            }
          
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            isPlayVisible = false;
        }
    }
    void Atack()
    {
        if (isAtack == false && isPlayVisible ==true)
        {
            isAtack = true;
            anima.SetTrigger("Atack");
        }
    }
    public void AtackIsDone()
    {
        StartCoroutine("AtackDelay");
    }

    void LookAt()
    {
        
        Vector3 lookdirection = (_gameManager.player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(lookdirection);
     
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _gameManager.slimeLookAtRotate * Time.deltaTime);

    }
    #endregion
}
