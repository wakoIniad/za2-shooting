using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Net.Sockets;
using WebSocketSharp;

public class gameManager : MonoBehaviour
{

    public GameObject thisPlayer;
    public GameObject opponent;
    private WebSocket _webSocket;
    void Start()
    { // websocket
        _webSocket = new WebSocket("wss://earwig-ruling-forcibly.ngrok-free.app/");
        _webSocket.OnOpen += (sender, e) => Debug.Log("WebSocket Open");
        _webSocket.OnMessage += (sender, e) => {
            Debug.Log("WebSocket Message Type: " + e.GetType() + ", Data: " + e.Data);            
            string[] param = e.Data.Split("|");
            string type = param[0];
            if(type == "result") {
                win();
            } else if(type == "pos") {
                string[] pos = param[1].Split(",");
                string[] ag = param[2].Split(",");
                opponent.transform.Translate(Single.Parse(pos[0]),Single.Parse(pos[1]),0);
                
                Vector3 worldAngle = opponent.transform.eulerAngles;
                worldAngle.x = Single.Parse(ag[0]);
                worldAngle.y = Single.Parse(ag[1]);
                worldAngle.z = Single.Parse(ag[2]);
                opponent.transform.eulerAngles = worldAngle;
            }
        };
        _webSocket.OnError += (sender, e) => Debug.Log("WebSocket Error Message: " + e.Message);
        _webSocket.OnClose += (sender, e) => Debug.Log("WebSocket Close");
        _webSocket.Connect();
        StartCoroutine("sendInfo");
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
    }
    public void displayResult(bool win) {
        SceneManager.LoadScene("result");
        SceneManager.activeSceneChanged += ActiveSceneChanged;
        result = win;
    }

    public void lose() {
        displayResult(false);
        //相手に結果を送る通信
        
        Debug.Log("I lose, you win.");
        _webSocket.Send("result");
    }

    public void win() {
        displayResult(true);
        Debug.Log("enemy lose.I win");
    }
    

    private void OnDestroy()
    {
        _webSocket.Close();
        _webSocket = null;
    }

    IEnumerator sendInfo()
    {
        while(true) {
            yield return new WaitForSeconds(0.5f);
            
            Vector3 worldAngle = thisPlayer.transform.eulerAngles;
            Vector3 worldPosition = thisPlayer.transform.position;
            _webSocket.Send("pos|"+worldPosition.x+","+worldPosition.y+"|"+worldAngle.x+","+worldAngle.y+","+worldAngle.z);

        }
    }
}
