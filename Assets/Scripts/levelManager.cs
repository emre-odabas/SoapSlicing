using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelManager : MonoBehaviour {
    public static levelManager instance;

    //properties
    public int currentLevel { get; set; } // aktif olan level in indexi
    public int endLevel { get; set; } // aktif olan levelden sonraki levelin indexi

    void Awake () {
        if (instance != null) {
            Destroy (gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad (gameObject);
        }

        currentLevel = Convert.ToInt16 (SceneManager.GetActiveScene ().buildIndex);
        endLevel = Convert.ToInt16 (SceneManager.GetActiveScene ().buildIndex) + 1;
    }

    void Start () {
        Debug.Log ("Total Level Count: " + SceneManager.sceneCountInBuildSettings);
    }

    public void setNextLevel () {
        var nextLevel = currentLevel + 1;
        if (SceneManager.sceneCountInBuildSettings <= nextLevel) {
            Debug.Log ("Coming Soon! :)");
        } else {
            SceneManager.LoadScene (nextLevel);

            currentLevel = nextLevel;
            endLevel++;
        }
    }

    public void setupLevels (int index) {
        levelManager.instance.currentLevel = index;
        levelManager.instance.endLevel = index + 1;
    }

}