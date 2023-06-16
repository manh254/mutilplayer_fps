using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using Photon.Realtime;
using Photon.Pun;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text text;
    Player player;
    public void SetUp(Player player){
        this.player = player;
        text.text = player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer){
        if(player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom(){
        Destroy(gameObject);
    }
}
