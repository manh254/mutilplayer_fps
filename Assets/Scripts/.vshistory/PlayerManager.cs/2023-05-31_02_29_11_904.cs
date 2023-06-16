using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System.Linq;
using UnityEditor.PackageManager;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    GameObject controller;
    void Awake()
    {
        PV = GetComponent<PhotonView>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        //If photon view is own by dat local player ...
        if (PV.IsMine)
        {
            CreateController();
        }

    }
    public void CreateController()
    {
        Transform spawnPoint =  SpawnManager.Instance.GetSpawnPoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"),spawnPoint.position, spawnPoint.rotation, 0, new object[] { PV.ViewID});
    }
    public void Die()
   {
        
        PhotonNetwork.Destroy(controller);

        CreateController();
    }

    void selectGunHUD()
    {
        if (gunInfo.itemName == "Riffle" && riffleSelection.activeSelf == false)
        {
            riffleSelection.SetActive(true);
            pistolSelection.SetActive(false);
        }
        else if (gunInfo.itemName == "Pistol" && pistolSelection.activeSelf == false)
        {
            riffleSelection.SetActive(false);
            pistolSelection.SetActive(true);
        }
    }


}
