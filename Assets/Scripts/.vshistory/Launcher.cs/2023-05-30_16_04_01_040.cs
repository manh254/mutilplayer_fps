using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks
{
	public static Launcher Instance;

	[SerializeField] TMP_InputField roomNameInputField;
	[SerializeField] TMP_Text errorText;
	[SerializeField] TMP_Text roomNameText;
	[SerializeField] TMP_Text warningText;
	[SerializeField] Transform roomListContent;
	[SerializeField] GameObject roomListItemPrefab;
	[SerializeField] Transform playerListContent;
	[SerializeField] GameObject playerListItemPrefab;
	[SerializeField] GameObject startGameButton;
    void Awake()
	{
		Instance = this;
	}

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

	public override void OnConnectedToMaster()
	{
        Debug.Log("Connected to server");
		PhotonNetwork.JoinLobby();
		//Sync scene giua may master va nhung nguoi choi khac
		PhotonNetwork.AutomaticallySyncScene = true;
	}
		
	public override void OnJoinedLobby()
	{
        MenuManager.Instance.OpenMenu("Title");
		Debug.Log("Joined Lobby");
		// PhotonNetwork.NickName = "Player " + Random.Range(1, 20).ToString("00");
	}

	


	public void CreateRoom()
	{
		if(string.IsNullOrEmpty(roomNameInputField.text))
		{
			warningText.text = "Please input room's name!!!";
			Debug.Log("Invalid room name");
			return;
		}
		PhotonNetwork.CreateRoom(roomNameInputField.text);
		MenuManager.Instance.OpenMenu("Loading");
	}
	//Successful callback after JoinRoom()
	public override void OnJoinedRoom()
	{
		Debug.Log("Joined Room");
		roomNameText.text = PhotonNetwork.CurrentRoom.Name;
		MenuManager.Instance.OpenMenu("Room");
		Player[] players = PhotonNetwork.PlayerList;

		foreach(Transform child in playerListContent){
			Destroy(child.gameObject);
		}
		if(PhotonNetwork.IsMasterClient){
			PhotonNetwork.NickName += " (host)";
			startGameButton.SetActive(true);
		}
		for(int i = 0; i < players.Count() ; i++){
			Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
		}
		
		//Set start button active only if the user is master client
		
	}



	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		errorText.text = "Room creation failed" + message;
		MenuManager.Instance.OpenMenu("Error");
	}
	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
		MenuManager.Instance.OpenMenu("Loading");
	}

	public override void OnLeftRoom(){
        SceneManager.LoadScene(0);
        MenuManager.Instance.OpenMenu("Title");
		base.OnLeftRoom();
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		foreach(Transform trans in roomListContent)
        {
            if(trans!=null)
                if(trans.gameObject!=null)
                     Destroy(trans.gameObject);
        }
		for(int i = 0; i<roomList.Count;i++){
			//[Bug fixed]In photon, when a room be removed, it will set variable RemovedFromList be true but not completlty eliminate it in system instanly.
			if(roomList[i].RemovedFromList)
			{
				continue;
			}
			Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
		}
	}

	public void JoinRoom(RoomInfo info)
	{
		PhotonNetwork.JoinRoom(info.Name);
		MenuManager.Instance.OpenMenu("Loading");
		
	}

	

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
	}

	public void StartGame()
	{
		PhotonNetwork.LoadLevel(1);
	}	
	//Set start button active if master client changes
	public override void OnMasterClientSwitched(Player newMasterClient){
		if(PhotonNetwork.IsMasterClient){
			newMasterClient.NickName += " (host)";
			startGameButton.SetActive(true);
		}

	}


	public void LeaveGame(){
		Application. Quit();
	}


}
