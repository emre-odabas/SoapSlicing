using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIControl : MonoBehaviour
{ 

    public TextMeshProUGUI txtTotalSlicedLine;

    void Start()
    {
        
    }
 
    void Update()
    {
        txtTotalSlicedLine.text = "Total Sliced Line: " + gameManager.instantiate.sliceStep.ToString();
    }

    //Buttons
    public void btnReload(){
        SceneManager.LoadScene("Level");
    }
}
