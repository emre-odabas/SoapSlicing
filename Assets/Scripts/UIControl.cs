using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControl : MonoBehaviour {

    public static UIControl instance;

    [Header ("Referances")]
    public TextMeshProUGUI txtCurrentLevel;
    public TextMeshProUGUI txtEndLevel;
    public Slider levelBar;

    public Transform panelLevelComplete;
    public TextMeshProUGUI txtLevelCompleteLevel;

    public Transform panelSelectLevel;
    public GameObject refLevelItem;
    public Transform levelItemParent;

    void Awake () {
        instance = this;
    }

    void Start () {
        populateLevelSelectUI ();
    }

    private void populateLevelSelectUI () {
        for (int i = 1; i <= SceneManager.sceneCountInBuildSettings; i++) {
            GameObject item = Instantiate (refLevelItem, levelItemParent);
            item.name = i.ToString ();
            item.transform.localScale = Vector2.one;
            item.transform.GetChild (0).gameObject.GetComponent<TextMeshProUGUI> ().text = i.ToString ();

            Button btnTemp = item.GetComponent<Button> ();
            item.GetComponent<Button> ().onClick.AddListener (() => btnStartLevel (btnTemp));
        }
    }

    void Update () {
        txtCurrentLevel.text = Convert.ToInt16 (levelManager.instance.currentLevel + 1).ToString (); //level indexleri 0 dan başladığı için +1 UI da görünen kısmı için
        txtEndLevel.text = Convert.ToInt16 (levelManager.instance.endLevel + 1).ToString ();

        levelBar.value = gameManager.instance.sliceStep;

    }

    public void showCompletePanel () {
        panelLevelComplete.DOLocalMove (Vector2.zero, 1);
        txtLevelCompleteLevel.text = "LEVEL " + Convert.ToInt16 (levelManager.instance.currentLevel + 1).ToString ();
    }

    private IEnumerator IE_setNextLevel () {
        yield return new WaitForSeconds (1);
        levelManager.instance.setNextLevel ();
    }

    //Buttons
    public void btnStartLevel (Button btnLevel) {
        int levelIndex = Convert.ToInt16 (btnLevel.transform.name) - 1;
        levelManager.instance.setupLevels(levelIndex);
        SceneManager.LoadScene (levelIndex);
    }

    public void btnReload () {
        Scene currentScene = SceneManager.GetActiveScene ();
        string sceneName = currentScene.name;
        SceneManager.LoadScene (sceneName);
    }

    public void btnNextLevel () {
        panelLevelComplete.DOLocalMove (new Vector2 (-800, 0), 1);
        StartCoroutine ("IE_setNextLevel");
    }

    public void btnOpenSelectLevelsPanel () {
        panelSelectLevel.DOLocalMove (Vector2.zero, 0.5f);
    }

    public void btnCloseSelectLevelsPanel () {
        panelSelectLevel.DOLocalMove (new Vector2 (800, 0), 0.5f);
    }

}