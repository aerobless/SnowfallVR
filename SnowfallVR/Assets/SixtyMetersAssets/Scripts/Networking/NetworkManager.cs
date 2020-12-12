using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace SixtyMetersAssets.Scripts.Networking
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        private const string DefaultRoom = "SnowfallVR_Default";

        private string _selectedRoom;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            if (PhotonNetwork.IsConnected)
            {
                OnConnectedToMaster();
                Debug.Log("Already connected");
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = Application.version;
                Debug.Log("Connecting...");
            }

            _selectedRoom = DefaultRoom;
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            Debug.Log("Connected to master!");
            Debug.Log("Joining room: " + _selectedRoom);

            JoinOrCreateRoom(_selectedRoom);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            Debug.LogWarning("Failed to join room " + _selectedRoom + " " + message);

            // TODO: retry? needs logic to stop after a few retries
            //JoinOrCreateRoom(_selectedRoom);
        }

        private void JoinOrCreateRoom(string roomName)
        {
            Debug.Log("Creating room " + roomName);
            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions {MaxPlayers = 4, IsOpen = true, IsVisible = true},
                TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined room!");

            CreatePlayer();
        }
        
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            Debug.Log("Got " + roomList.Count + " rooms.");
            foreach(RoomInfo room in roomList)
            {
                Debug.Log("Room: " + room.Name + ", " + room.PlayerCount);
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            //OnPlayersChanged?.Invoke(); //TODO:
            Debug.Log("A new player enter the room with id" + newPlayer.UserId);
        }
        
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            //OnPlayersChanged?.Invoke(); //TODO:

        }

        private void CreatePlayer()
        {
            PhotonNetwork.Instantiate(Path.Combine("NetworkPrefabs", "NetworkHead"), Vector3.zero, Quaternion.identity, 0);
            PhotonNetwork.Instantiate(Path.Combine("NetworkPrefabs", "NetworkHandRight"),Vector3.zero, Quaternion.identity, 0);
            PhotonNetwork.Instantiate(Path.Combine("NetworkPrefabs", "NetworkHandLeft"), Vector3.zero, Quaternion.identity, 0);
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}