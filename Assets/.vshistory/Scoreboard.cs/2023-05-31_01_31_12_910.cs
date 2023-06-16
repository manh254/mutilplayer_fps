using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreboardItemPrefab;

    void Start() { 
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddScoreboardItem(player);
        }
    }
}
