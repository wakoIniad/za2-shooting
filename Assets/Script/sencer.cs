using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sencer : MonoBehaviour
{
    public int sencerType;
    public GameObject gameManagerObject;
    public gameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameManagerObject.GetComponent<gameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "judge_bar") {
            Debug.Log("TESTAAAA");
            if(sencerType == 0) {
                gameManager.playerLose();
            } else if(sencerType == 1) {
                gameManager.playerWin();
            }
        }
    }
}
