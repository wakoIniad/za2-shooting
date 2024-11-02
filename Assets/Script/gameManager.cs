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

    private WebSocket _webSocket;
    void Start()
    { // websocket
        _webSocket = new WebSocket("wss://earwig-ruling-forcibly.ngrok-free.app/");
        _webSocket.OnOpen += (sender, e) => Debug.Log("WebSocket Open");
        _webSocket.OnMessage += (sender, e) => {
            Debug.Log("WebSocket Message Type: " + e.GetType() + ", Data: " + e.Data);
            if(e.Data == "you_win") {
                win();
            }
        };
        _webSocket.OnError += (sender, e) => Debug.Log("WebSocket Error Message: " + e.Message);
        _webSocket.OnClose += (sender, e) => Debug.Log("WebSocket Close");
        _webSocket.Connect();
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
        _webSocket.Send("you_win");
    }

    public void win() {
        displayResult(ture);
        //相手に結果を送る通信
        
        Debug.Log("enemy lose.I win");
    }

    

    private void OnDestroy()
    {
        _webSocket.Close();
        _webSocket = null;
    }
}
