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
    private WebSocket _webSocket;
    void Start()
    { // websocket
        _webSocket = new WebSocket("wss://earwig-ruling-forcibly.ngrok-free.app/");
        _webSocket.OnOpen += (sender, e) => Debug.Log("WebSocket Open");
        _webSocket.OnMessage += (sender, e) => {
            Debug.Log("WebSocket Message Type: " + e.GetType() + ", Data: " + e.Data);            
            JsonNode json = JsonNode.Parse(e.Data);
            JsonNode type = json.get("type");
            JsonNode value = json.get("value");
            if(type == "result") {
                win();
            } else if(type == "pos") {
                JsonNode[] pos = value.get("pos");
                JsonNode[] ag = value.get("ag");
                thisPlayer.transform.Translate(Single.Parse(pos.get("x")),Single.Parse(pos.get("y")));
                
                Vector3 worldAngle = thisPlayer.transform.eulerAngles;
                worldAngle.x = Single.Parse(ag.get("x"));
                worldAngle.y = Single.Parse(ag.get("y"));
                worldAngle.z = Single.Parse(ag.get("z"));
                thisPlayer.transform.eulerAngles = worldAngle;
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
        _webSocket.Send("{\"type\":\"result\"}");
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
}
