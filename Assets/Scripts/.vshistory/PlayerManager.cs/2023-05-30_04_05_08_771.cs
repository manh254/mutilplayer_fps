using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System.Linq;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    GameObject controller;
    void Awake()
    {
        PV = GetComponent<PhotonView>();
        //If photon view is own by dat local player ...
        if (PV.IsMine)
        {
            CreateController();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    
        
    }
    void CreateController()
    {
        Transform spawnPoint =  SpawnManager.Instance.GetSpawnPoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"),spawnPoint.position, spawnPoint.rotation, 0, new object[] { PV.ViewID});
    }
    public IEnumerator Die()
    {
        PhotonNetwork.Destroy(controller);
        yield return new WaitForSeconds(0f);
        CreateController();
    }


    
}
