using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class Scoreboard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreboardItemPrefab;
    Dictionary<Player, ScoreboardItem> scoreboardItems = new Dictionary<Player, ScoreboardItem>();

    void Start() { 
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddScoreboardItem(player);
        }
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddScoreboardItem(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        RemoveScoreboardItem(newPlayer);

    }
    void AddScoreboardItem(Player player)
    {
        ScoreboardItem scoreboardItem = Instantiate(scoreboardItemPrefab, container).GetComponent<ScoreboardItem>();
        scoreboardItem.Initialize(player);
        scoreboardItems[player] = scoreboardItem;   
    }

    void RemoveScoreboardItem(Player player )
    {
        Destroy(scoreboardItems[player].gameObject);
        scoreboardItems.Remove(player);
    }

}
