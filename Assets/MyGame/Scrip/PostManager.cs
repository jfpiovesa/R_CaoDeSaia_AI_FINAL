using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostManager : MonoBehaviour
{
    private GameManager manager;

    public bool isRain;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType(typeof(GameManager)) as GameManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            manager.onOfRain(isRain);
        }
    }
}
