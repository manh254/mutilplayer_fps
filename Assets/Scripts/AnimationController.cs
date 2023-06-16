using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject pistol;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            StartCoroutine(StartRecoil());
        }
    }

    IEnumerator StartRecoil()
    {
        pistol.GetComponentInChildren<Animator>().Play("RecoilAni");
        yield return new WaitForSeconds(0.20f);
        pistol.GetComponentInChildren<Animator>().Play("New State");
    }
}
