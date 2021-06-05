using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gen : MonoBehaviour
{
    GameManager _gameManager;
    public int valor;
    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>() as GameManager;
    }
    private void Update()
    {
        float time = 20;
        time -= Time.deltaTime;
        if(time <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _gameManager.GetGens(valor);
            Destroy(gameObject);
        }
    }
}
