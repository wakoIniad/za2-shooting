using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public float hp = 0;

    public float playerHP = 200;
    public float enemyHP = 200;

    public GameObject hpBer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    float hpBerWidth = 16;

    void moveHpBer(float d) {
        if(hp > 0) {
            hpBer.transform.Translate(new Vector3(hpBerWidth*3.5f/playerHP*d,0,0));
        } else {
            hpBer.transform.Translate(new Vector3(hpBerWidth*3.5f/enemyHP*d,0,0));
        }
    }

    public void damagePlayer(float d) {
        hp -= d;
        moveHpBer(-d);
        if(hp < -playerHP) {
            //負け処理
        }
        Debug.Log(hp);
    }
    public void damageEnemy(float d) {
        hp += d;
        moveHpBer(d);
        if(hp > enemyHP) {
            //勝ち処理
        }
        Debug.Log(hp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
