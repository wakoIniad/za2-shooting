using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class displayResult : MonoBehaviour
{
    public TextMeshProUGUI text;

    public int score;// 共有する変数
    public bool result;// 共有する変数
    // Start is called before the first frame update
    void Start()
    {
//        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    int calcScore(int n) {
        return (int) (100 * 10000/n);
    }
    void Update()
    {
        if(result) {
            text.text = "You Win, score="+calcScore(score);
        } else {
            text.text = "You Lose, score=--";
        }
    }
}
