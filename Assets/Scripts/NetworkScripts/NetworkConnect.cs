using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkConnect : MonoBehaviourPunCallbacks
{
    [SerializeField] private string gameVersion = "0.0.1";
    [SerializeField] private string playerName = string.Empty;
    [SerializeField] private int maxPlayer = 2;
    private RoomOptions roomOptions = null;


    private void Awake()
    {
        PhotonNetwork.GameVersion = gameVersion;

        if(!PhotonNetwork.IsConnected) PhotonNetwork.ConnectUsingSettings();

        roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsOpen = true;
        roomOptions.BroadcastPropsChangeToAll = true;
        roomOptions.CleanupCacheOnLeave = true;
    }



    /// <summary>
    /// Connecting to PhotonNetwork Master Server
    /// When Connecting to Server, Initialize NickName;
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.LogError("Connecting To Master Server...");

        PhotonNetwork.LocalPlayer.NickName = playerName;
        PhotonNetwork.JoinLobby();
    }
    
    /// <summary>
    /// Connecting Sequence
    /// 1. On Connected to Master.
    /// 2. On Joined to Lobby.
    /// 3. On Joined to Room.(if joinRandomRoom has Failed CallBack CreateRoom())
    /// </summary>
    public override void OnJoinedLobby()
    {
        Debug.LogError("Joined in Lobby....");
        base.OnJoinedLobby();
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnCreatedRoom()
    {
        Debug.LogError($"Create New Room...");
        base.OnCreatedRoom();
    }
    public override void OnJoinedRoom()
    {
        Debug.LogError("Hello World...");
        base.OnJoinedRoom();
    }


    /// <summary>
    /// Method Callbacks when Method return has Failed
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError($"Join Room has Failed.. {returnCode} : {message}");
         //If joinRoom() has Failed, Create new Room
        PhotonNetwork.CreateRoom("testRoom", roomOptions);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
    }



    /// <summary>
    /// CallBack Method 
    /// when client disconnect from Master server
    /// </summary>
    /// <param name="_cause"></param>
    /// 
      public override void OnLeftLobby()
    {
        Debug.LogError("Leaved Lobby...");
        base.OnLeftLobby();
    }

    public override void OnDisconnected(DisconnectCause _cause)
    {
        Debug.LogError($"Disconnected from Master Server...: {_cause}");
        base.OnDisconnected(_cause);
    }


}