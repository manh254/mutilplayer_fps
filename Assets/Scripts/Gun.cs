using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Item
{
    public abstract override void Use();
    //public abstract void RecoilPatern();
    public GameObject bulletImpactPrefab;
}
