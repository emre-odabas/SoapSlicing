using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knifeControl : MonoBehaviour {

    public GameObject sliceClearObj; // objeleri aşağı sürüklemek için geçici box collider in parent objesi

    Vector3 dist;
    Vector3 startPos;
    float posX;
    float posZ;
    float posY;

    Vector3 prevLoc = Vector3.zero;

    //Bıçak objesini kaydırma işlemleri
    void OnMouseDown () {
        startPos = transform.position;
        dist = Camera.main.WorldToScreenPoint (transform.position);
        posX = Input.mousePosition.x - dist.x;
        posY = Input.mousePosition.y - dist.y;
        posZ = Input.mousePosition.z - dist.z;
    }

    void OnMouseDrag () {
        float disX = Input.mousePosition.x - posX;
        float disY = Input.mousePosition.y - posY;
        float disZ = Input.mousePosition.z - posZ;
        Vector3 lastPos = Camera.main.ScreenToWorldPoint (new Vector3 (disX, disY, disZ));

        if (gameManager.instance.startGame) {
            transform.position = new Vector3 (startPos.x, startPos.y, lastPos.z);
        }

    }

    void FixedUpdate () {
        //bıçak eğer başlangıçta değilse step tamamlanmış olsa bile alt satıra geçmemesi için
        if (transform.position.z > 16) {
            float soapLine = gameManager.instance.currentLine;
            transform.position = new Vector3 (transform.position.x, soapLine, transform.position.z);
        }

        //bıçağın yukarıya mı yoksa aşağıya mı kaydırıldını anlamak için
        Vector3 curVel = (transform.position - prevLoc) / Time.deltaTime;
        if (curVel.z > 0) {

            sliceClearObj.GetComponent<BoxCollider> ().enabled = true; //eğer yukarı kaydırılıyorsa geçici box collider i etkinleştirip objeleri aşağıya sürükler
            //Quaternion target = Quaternion.Euler (40, 0, 0);
            //transform.rotation = Quaternion.Slerp (transform.rotation, target, Time.deltaTime * 100f); //objenin yukarı aşağı hareket ederken rotasyon alması için. bu kısımda ufak bir sorun yaşadığım için devredışı bırakmak zorunda kaldım
        } else {

            sliceClearObj.GetComponent<BoxCollider> ().enabled = false;
            // Quaternion target = Quaternion.Euler (0, 0, 0);
            //transform.rotation = Quaternion.Slerp (transform.rotation, target, Time.deltaTime * 100f);
        }
        prevLoc = transform.position;
    }

    void Start () {

    }

    void Update () {

    }

}