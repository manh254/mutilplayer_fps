using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class holds necessary info for every guns
[CreateAssetMenu(menuName = "FPS/New Gun")]
public class GunInfo : ItemInfo
{
   public float damage;
   public float range = 100f;
   public float fireRate;

   public int maxAmmo;
   
}
