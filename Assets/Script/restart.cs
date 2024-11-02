using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class restart : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnClick()
    {
        //Application.LoadLevel(Application.loadedLevel);
        //SceneManager.LoadScene(0);
        SceneManager.LoadScene("SampleScene");  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
