using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] GunInfo gunInfo;
    [SerializeField] SingleShotGun singleShotGun;
    [SerializeField] TMP_Text ammo;

    // Start is called before the first frame update
    void Awake()
    {
        
    }
    void UpdateAmmoUI(){
        ammo.text = singleShotGun.currentAmmo.ToString() + "/" + gunInfo.maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAmmoUI();
    }

    
}
