using UnityEngine;
public enum ServerPackets
{
    SWelcomeMsg = 1,
    SOpenAccount,
    SStartPlayGame,
    SSetPlayersData,
}

internal static class NetworkReceive
{
    public static void InitPackets()
    {
        ClientHandleData.packets.Add((int)ServerPackets.SWelcomeMsg, NetworkReceive.HandleWelcomeMessage);      
        ClientHandleData.packets.Add((int)ServerPackets.SOpenAccount, NetworkReceive.HandleOpenAccount);
        ClientHandleData.packets.Add((int)ServerPackets.SStartPlayGame, NetworkReceive.HandleStartPlayGame);
        ClientHandleData.packets.Add((int)ServerPackets.SSetPlayersData, NetworkReceive.HandleServerSetPlayersData);
    }
    public static void HandleWelcomeMessage(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        int packetId = buffer.ReadInteger();
        string msg = buffer.ReadString();
        buffer.Dispose();
        Debug.Log(msg);
        NetworkSend.SendHelloServer();
    }
    public static void HandleOpenAccount(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        int packetId = buffer.ReadInteger();
        string name = buffer.ReadString();
        string password = buffer.ReadString();
        int[] values = buffer.ReadIntArray();
        buffer.Dispose();
        Debug.Log("Succesful open account");
        PlayerPrefs.SetString("AccountName", name);
        PlayerPrefs.SetString("AccountPassword", password);
        //ObjectsOnScene.a.currCanvas.AfterLogIn();
    }
    static void HandleStartPlayGame(byte[] data)//Recieve all players data
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        int packetId = buffer.ReadInteger();
        int connectionId = buffer.ReadInteger();

        //Transform player = ObjectsOnScene.a.currPlayerInGameScene.transform;
        //if (player.GetComponent<NetworkPlayer>() == null)
        //    player.gameObject.AddComponent<NetworkPlayer>();
        buffer.Dispose();
        //ObjectsOnScene.a.currCanvas.AfterStartGame();
    }
    static void HandleServerSetPlayersData(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        int packetId = buffer.ReadInteger();
        int connectionId = buffer.ReadInteger();
        string name = buffer.ReadString();
        int selectedRingId = buffer.ReadInteger();
        buffer.Dispose();
        Network.instance.InstantiateNetworkPlayer(connectionId, name, selectedRingId);
    }
}