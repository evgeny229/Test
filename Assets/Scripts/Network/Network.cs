using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;
using System.Threading;
using System.Net;
using System.Threading.Tasks;
using System.Text;

public class Network : MonoBehaviour
{
    public static Network instance;
    public GameObject playerPref;
    public Transform parentPlayers;
    public Dictionary<int, GameObject> playerList = new Dictionary<int, GameObject>();
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //TcpClient client = new TcpClient();
        //client.Connect(new IPAddress(new byte[] { 127, 0, 0, 1 }), 55);
        DontDestroyOnLoad(this);
        UnityThread.initUnityThread();

        ClientHandleData.InitializePackets();
        ClientTCP.InitializeNetworking();
    }
    private void OnApplicationQuit()
    {
        ClientTCP.Disconnect();
    }
    public void InstantiateNetworkPlayer(int connectionId, string name, int selectedRingId)
    {
        GameObject go = Instantiate(playerPref, parentPlayers);
        go.name = "Player (" + name + "): " + connectionId;
        //go.transform.Find("TextName").GetComponent<TMP_Text>().text = name;
        if (playerList.ContainsKey(connectionId))
            playerList.Remove(connectionId);
        playerList.Add(connectionId, go);
    }
}
//public GameObject playerPref;
//public Transform parentPlayers;
///*

//public static Network instance;
//public int myConnectionId;
//private void Awake()
//{
//    instance = this;
//    TryConnect();
//}
//async void TryConnect()
//{
//    TcpClient client = new TcpClient();
//    //try
//    client.Connect(new IPAddress(new byte[] { 192, 168, 100, 8 }), 55);
//}
//*/
//private void Start()
//{
//    DontDestroyOnLoad(this);
//    NetworkConfig.InitNetwork();
//    NetworkConfig.ConnectToServer();
//}
//private void OnApplicationQuit()
//{
//    NetworkConfig.DisconnectFromServer();
//}
//public void InstantiateNetworkPlayer(int connectionId, string name, int selectedRingId)
//{
//    GameObject go = Instantiate(playerPref, parentPlayers);
//    go.name = "Player (" + name + "): " + connectionId;
//    go.transform.Find("TextName").GetComponent<TMP_Text>().text = name;
//    if (GameManager.instance.playerList.ContainsKey(connectionId))
//        GameManager.instance.playerList.Remove(connectionId);
//    GameManager.instance.playerList.Add(connectionId, go);
//}
//public void DestroyNetworkPlayer(int connectionId)
//{
//    Destroy(GameManager.instance.playerList[connectionId]);
//    GameManager.instance.playerList.Remove(connectionId);
//}