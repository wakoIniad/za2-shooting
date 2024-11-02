using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class judge : MonoBehaviour
{
    public GameObject myPlayer;
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

    bool countdownNow = false;

    void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "ThisPlayer") {
            countdown(5);
            countdownNow = true;
        }
    }
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "ThisPlayer"){
            if(countdownNow)countdownNow = false;
        }
    }
    IEnumerator countdown(int time) {
        for(int t = 0;0 < time;t++) {
            Debug.Log(time-t+"!");
            yield return new WaitForSeconds(1);
            if(!countdownNow)return;
        }
        gameManager.lose();
    }
}
