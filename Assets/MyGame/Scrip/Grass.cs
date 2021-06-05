using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    public ParticleSystem fxGrass;
    private bool isCut;
    GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
    }
    void GetHit(int amout )
    {
        if (isCut == false)
        {
            transform.localScale = new Vector3(1f, 0.5f, 1f);
            fxGrass.Emit(10);
            isCut = true;
            if (_gameManager.Perc(_gameManager.percDrop) == true)
            {
                Instantiate(_gameManager.gensPrefabs, transform.position, Quaternion.identity);
            }
        }
    }
  
}
