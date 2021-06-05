using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameManager _gameManager;
    private CharacterController controller;
    private Animator anima;

    [Header("Config Player")]
    public float movimentSpeed = 5;
    private bool isWalk;
    public int hp;
    private Vector3 directions;

    [Header("AtackConfiguration")]
    public ParticleSystem fxAtack;
    private bool isAtack ;
    public Transform hitBox;
    [Range(0, 2)]
    public float hitRange = 0.5f;

    public Collider[] hitInfo;
    public LayerMask hitMask;

    public int DamgeAmount;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

        controller = GetComponent<CharacterController>();
        anima = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.gameState != GameState.GamePlay) { return; }

        Moviment();
        Inputs();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "TakeDamage")
        {
            GetHit(DamgeAmount);
        }
    }
    void Moviment()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        directions = new Vector3(hor, 0f, ver).normalized;
        if (directions.magnitude >0.1f)
        {
            float targetangles = Mathf.Atan2(directions.x, directions.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetangles, 0);
            isWalk = true;
        }
        else
        {
            isWalk = false;
        }
        anima.SetBool("IsWalk", isWalk);
        controller.Move(directions * movimentSpeed * Time.deltaTime);

    }

    void Inputs()
    {
        if (Input.GetButtonDown("Fire1") && isAtack == false)
        {
            Attack();
        }
    }
     void Attack()
    {

      
        anima.SetTrigger("Atack");
        fxAtack.Emit(1);
        isAtack = true;
        hitInfo = Physics.OverlapSphere(hitBox.position, hitRange, hitMask);
        foreach(Collider c in hitInfo)
        {
            c.gameObject.SendMessage("GetHit", DamgeAmount, SendMessageOptions.DontRequireReceiver);
        }


    }
       public void AtackIsDone()
    {
        isAtack = false;
    }
    void GetHit(int amount)
    {
        hp -= amount;
        if(hp >= 0)
        {
            anima.SetTrigger("Hit");
        }
        else
        {
            _gameManager.ChangerGameState(GameState.Die);
            anima.SetTrigger("Die");
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitBox.position, hitRange);
    }
}
