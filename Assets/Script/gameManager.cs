using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using UnityEngine.Events;
using System.Collections.Generic;
public class gameManager : MonoBehaviour
{
    public float startTime;
    public float endTime;
    public float judgement = 0;

    public float playerPoint = 200;
    public float enemyPoint = 200;

    public GameObject judgeBar;
    // Start is called before the first frame update
    void Start()
    {
        judgeBarWidth = judgeBar.GetComponent<SpriteRenderer>().bounds.size.x;
    }

//    float judgeBarWidth = 16;
    float judgeBarWidth;

    void moveJudgeBar(float d) {
        if(judgement > 0) {
            judgeBar.transform.Translate(new Vector3(judgeBarWidth/4f/playerPoint*d,0,0));
        } else {
            judgeBar.transform.Translate(new Vector3(judgeBarWidth/4f/enemyPoint*d,0,0));
        }
    }

    public void damagePlayer(float d) {
        judgement -= d;
        moveJudgeBar(-d);
        if(judgement < -playerPoint) {
            //負け処理
            displayResult(false);
        }
        Debug.Log(judgement);
    }
    public void damageEnemy(float d, GameObject enemyObj, float hp) {
        judgement += d;
        moveJudgeBar(d);
        if(hp < 0) {
            Destroy(enemyObj);
        }
        if(judgement > enemyPoint) {
            //勝ち処理
            displayResult(true);
        }
        Debug.Log(judgement);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool result;
    void ActiveSceneChanged(Scene thisScene, Scene nextScene) {
        displayResult displayResult = GameObject.FindWithTag("ResultManager").GetComponent<displayResult>();
        displayResult.result = result;
        displayResult.score = (int) (endTime - startTime);
    }
    public void displayResult(bool win) {
        SceneManager.LoadScene("result");
        SceneManager.activeSceneChanged += ActiveSceneChanged;
        result = win;
        endTime = Time.time;;
    }
}
