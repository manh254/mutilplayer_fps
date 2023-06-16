using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using System.Linq;
using System;

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


    public static PlayerManager Find(Player player)
    {
        return FindObjectOfType<PlayerManager>().SingleOrDefault(x => x.PV.Owner == player);
    }
    
}
