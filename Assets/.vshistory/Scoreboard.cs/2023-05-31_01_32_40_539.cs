using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

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
    void AddScoreboardItem(Player player)
    {
        ScoreboardItem scoreboardItem= Instantiate(scoreboardItemPrefab, container).GetComponent<ScoreboardItem>();
        scoreboardItem.Initialize(player);
        
    }
}
