using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public float judgement = 0;

    public float playerPoint = 200;
    public float enemyPoint = 200;

    public GameObject judgeBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    float judgeBarWidth = 16;

    void moveJudgeBar(float d) {
        if(judgement > 0) {
            judgeBar.transform.Translate(new Vector3(judgeBarWidth*3.5f/playerPoint*d,0,0));
        } else {
            judgeBar.transform.Translate(new Vector3(judgeBarWidth*3.5f/enemyPoint*d,0,0));
        }
    }

    public void damagePlayer(float d) {
        judgement -= d;
        moveJudgeBar(-d);
        if(judgement < -playerPoint) {
            //負け処理
        }
        Debug.Log(judgement);
    }
    public void damageEnemy(float d) {
        judgement += d;
        moveJudgeBar(d);
        if(judgement > enemyPoint) {
            //勝ち処理
        }
        Debug.Log(judgement);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void playerWin() {
        Debug.Log("win!!");
    }
    public void playerLose() {
        Debug.Log("lose..");
    }
}
