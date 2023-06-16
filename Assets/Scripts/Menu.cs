using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string menuName;
    public bool isOpening;
   
    public void Open(){
        isOpening = true;
        gameObject.SetActive(true);
    }

    public void Close(){
        isOpening = false;
        gameObject.SetActive(false);
    }
}
