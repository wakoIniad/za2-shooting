using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using UnityEngine.Events;
using System.Collections.Generic;
public class gameManager : MonoBehaviour
{

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    bool result;
    void ActiveSceneChanged(Scene thisScene, Scene nextScene) {
        SceneManager.activeSceneChanged -= ActiveSceneChanged;
        displayResult displayResult = GameObject.FindWithTag("ResultManager").GetComponent<displayResult>();
        displayResult.result = result;
        displayResult.score = (int) (endTime - startTime);
    }
    public void displayResult(bool win) {
        SceneManager.LoadScene("result");
        SceneManager.activeSceneChanged += ActiveSceneChanged;
        result = win;
        endTime = Time.time;
    }

    public void lose() {
        displayResult(false);
        //相手に結果を送る通信
    }
}
