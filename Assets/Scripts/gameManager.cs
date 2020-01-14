using System;
using System.Collections;
using System.Collections.Generic;
using EzySlice;
using UnityEngine;

public class gameManager : MonoBehaviour {

    public static gameManager instantiate;

    [Header ("References")]
    public GameObject refSoapParticle;
    public GameObject refSuprise;
    public GameObject knife;
    public GameObject knifeBlade;
    public Material material;
    public Transform soapParentTransform;
    public GameObject smileEffect;
    public GameObject finalEffect;

    [Header ("Settings")]
    public LayerMask layerMask; // kesilebilen objelerin layerini ayarlar (editörder)
    public float lineLower = 0.5f; // her basamakta alçalma derecesi
    public int soapX = 10; //sabunun x uzunluğu
    public int soapY = 20; //sabunun y uzunluğu

    private GameObject[] lastFloor; // en sona kalan satır
    private int sliced = 0; // başlangıçta kesilen obje olmadığı için 0

    //Properties
    public float currentLine { get; set; } = 2.5f; //aktif satır
    public int sliceStep { get; set; } = 0; //tamamlanan basamak sayısı

    //Editör debugging için
    public void OnDrawGizmos () {
        EzySlice.Plane cuttingPlane = new EzySlice.Plane ();

        cuttingPlane.Compute (knifeBlade.transform);
        cuttingPlane.OnDebugDraw ();
    }

    void Awake () {
        instantiate = this;
    }

    void Start () {
        populateSoap ();
    }

    void Update () {
        if (Input.GetMouseButton (0)) {
            //kesilebilen objeleri layer ile ayırıp bir dizi yaptık
            Collider[] slicedObjects = Physics.OverlapBox (knifeBlade.transform.position, new Vector3 (knifeBlade.transform.localScale.x * 5, knifeBlade.transform.localScale.y, knifeBlade.transform.localScale.z), knifeBlade.transform.rotation, layerMask);

            foreach (Collider obj in slicedObjects) {
                SlicedHull slicedObj = Slice (obj.gameObject, material); 
                if (slicedObj != null) {
                    GameObject slicedUpper = slicedObj.CreateUpperHull (obj.gameObject, material);//oluşan üst nesne
                    GameObject slicesLower = slicedObj.CreateLowerHull (obj.gameObject, material);//oluşan alt nesne

                    addComponents (slicedUpper, slicesLower); // nesnelerin componentlerini ayalar

                    Destroy (obj.gameObject);// orjinal nesneyi siler
                }
            }
        }
    }

    private void addComponents (GameObject _objUpper, GameObject _objLower) {
        //Upper
        _objUpper.AddComponent<BoxCollider> ();
        _objUpper.AddComponent<Rigidbody> ();
        _objUpper.GetComponent<Rigidbody> ().AddForceAtPosition (new Vector3 (1, 5, 2) * 4, _objUpper.transform.position);
        _objUpper.GetComponent<Rigidbody> ().mass = 5;
        _objUpper.GetComponent<Rigidbody> ().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        //Lower
        _objLower.AddComponent<BoxCollider> ();
        _objLower.layer = 8; // altta kalan kısmı tekrar kesilebilir katman haline getirdik

        //Other
        FindObjectOfType<AudioManager> ().Play ("Cutting");
  
        sliced++; //her obje kesildiğinde +1 artırıyoruz
        if (sliced == 150) // 10x15 lik bir yüzeyde en az 150 obje kesilmeli.     
            lineSetup ();
    }

    private void lineSetup () {
        sliceStep++; //tamamlanan satırı 1 artırıyoruz
        sliced = 0; //kesilen obje sayısını tekrar sıfırlıyoruz
        currentLine -= lineLower; //aktif satırı lineLower kadar alçaltıyoruz
        Instantiate (smileEffect, new Vector3 (4.5f, 6.5f, 4.5f), Quaternion.identity);

        if (sliceStep == 6) 
            Finish();
    }

    void Finish () {
        Instantiate (finalEffect, new Vector3 (4.5f, 6.5f, 4.5f), Quaternion.identity);
        FindObjectOfType<AudioManager> ().Play ("Finish");
    }

    public SlicedHull Slice (GameObject obj, Material material = null) {
        return obj.Slice (knifeBlade.transform.position, -knifeBlade.transform.up, material); // bıçak objesi ters olduğu için -transtorm.up değerini almak zorunda kaldım
    }

    //başlangıçta sabun particle lerinin konumunu ayarlar
    private void populateSoap () {
        for (int i = 0; i < soapX; i++) {
            for (int j = 0; j < soapY; j++) {
                GameObject particle = Instantiate (refSoapParticle, new Vector3 (i, 0, j), Quaternion.identity);
                particle.transform.parent = soapParentTransform;
            }
        }
        GameObject surprise = Instantiate (refSuprise, new Vector3 (4.5f, -2, 6.5f), Quaternion.identity);
        surprise.transform.Rotate (new Vector3 (90, 0, 0));
    }

}