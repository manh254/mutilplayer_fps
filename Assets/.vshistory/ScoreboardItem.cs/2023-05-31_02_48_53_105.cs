using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
public class ScoreboardItem : MonoBehaviourPunCallback
{
    public TMP_Text usernameText;
    public TMP_Text killsText;
    public TMP_Text deathsText;

    public void Initialize(Player player)
    {
        usernameText.text = player.NickName;
    }
}
