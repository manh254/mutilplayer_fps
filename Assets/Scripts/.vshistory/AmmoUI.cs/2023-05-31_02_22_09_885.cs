using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;


public class AmmoUI : MonoBehaviourPunCallbacks
{
    //private PhotonView PV;
    [SerializeField] GunInfo gunInfo;
    [SerializeField] SingleShotGun singleShotGun;
    [SerializeField] TMP_Text ammo;
    [SerializeField] GameObject riffleSelection;
    [SerializeField] GameObject pistolSelection;

    // Start is called before the first frame update
    void Awake()
    {
        //PV = GetComponent<PhotonView>();
    }
    void UpdateAmmoUI(){
        ammo.text = singleShotGun.currentAmmo.ToString() + "/" + gunInfo.maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        selectGunHUD();
        UpdateAmmoUI();
    }

    void selectGunHUD()
    {
        if ( gunInfo.itemName == "Riffle" && riffleSelection == false)
        {
            riffleSelection.SetActive(true);
            pistolSelection.SetActive(false);
        }
        else if(gunInfo.itemName == "Pistol")
        {
            riffleSelection.SetActive(false);
            pistolSelection.SetActive(true);
        }
    }

    
}
