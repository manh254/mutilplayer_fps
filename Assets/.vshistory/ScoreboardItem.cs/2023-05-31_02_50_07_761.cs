using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;

public class ScoreboardItem : MonoBehaviourPunCallbacks
{
    public TMP_Text usernameText;
    public TMP_Text killsText;
    public TMP_Text deathsText;

    Player player;

    public void Initialize(Player player)
    {
        usernameText.text = player.NickName;
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
    }
}
