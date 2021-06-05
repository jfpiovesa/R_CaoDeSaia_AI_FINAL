using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private GameManager _gameManager;

    [Header("Camera")]
    public GameObject vcam1;
    private void Start()
    {
        _gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "camTrigger":
                vcam1.SetActive(true);
                break;
            case "Coletavel":
                Destroy(other.gameObject);
                _gameManager.GetGens(1);
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "camTrigger":
                vcam1.SetActive(false);
                break;
        }
    }
}
