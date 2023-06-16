using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject graphicObject;

    void Awake()
    {
        graphicObject.SetActive(false);
    }
}
