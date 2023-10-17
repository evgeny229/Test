using UnityEngine;
//enum ClientPacket
//{
//    CPing = 1,
//    CDestroyPlayer,
//    CTrySingUp,
//    CTryLogIn,
//    CTryFindGame,
//    CSendUpdatePosition,
//}
public enum ClientPacket
{
    CHelloServer = 1,
    CTryCreateAccount,
    CTryOpenAccount,
    CTryFindGame,
}
internal static class NetworkSend
{
    //public static void SendPing()
    //{
    //    ObjectsOnScene.a.currCameraInGameScene.t2.text = "Send Ping0";
    //    ByteBuffer buffer = new ByteBuffer(4);
    //    ObjectsOnScene.a.currCameraInGameScene.t2.text = "Send Ping1";
    //    buffer.WriteInt32((int)ClientPacket.CPing);
    //    ObjectsOnScene.a.currCameraInGameScene.t2.text = "Send Ping2";
    //    buffer.WriteString("hello from client");
    //    ObjectsOnScene.a.currCameraInGameScene.t2.text = "Send Ping3";
    //    NetworkConfig.socket.SendData(buffer.Data, buffer.Head);
    //    ObjectsOnScene.a.currCameraInGameScene.t2.text = "Send Ping4";
    //    buffer.Dispose();
    //    ObjectsOnScene.a.currCameraInGameScene.t2.text = "Send Ping5";
    //}
    //public static void DestroyNetworkPlayer()
    //{
    //    ByteBuffer buffer = new ByteBuffer(4);
    //    buffer.WriteInt32((int)ClientPacket.CDestroyPlayer);
    //    NetworkConfig.socket.SendData(buffer.Data, buffer.Head);
    //    buffer.Dispose();
    //}
    //public static void TrySingUp(string name, string password)
    //{
    //    ByteBuffer buffer = new ByteBuffer(4);
    //    buffer.WriteInt32((int)ClientPacket.CTrySingUp);
    //    buffer.WriteString(name);
    //    buffer.WriteString(password);
    //    NetworkConfig.socket.SendData(buffer.Data, buffer.Head);
    //    buffer.Dispose();
    //    Debug.Log("try create acc. name: " + name + " password: " + password);
    //}
    //public static void TryLogIn(string name, string password)
    //{
    //    ByteBuffer buffer = new ByteBuffer(4);
    //    buffer.WriteInt32((int)ClientPacket.CTryLogIn);
    //    buffer.WriteString(name);
    //    buffer.WriteString(password);
    //    NetworkConfig.socket.SendData(buffer.Data, buffer.Head);
    //    buffer.Dispose();
    //    Debug.Log("try log in. name: " + name + " password: " + password);
    //}
    //public static void StartFindGame(int gameId)
    //{
    //    ByteBuffer buffer = new ByteBuffer(4);
    //    buffer.WriteInt32((int)ClientPacket.CTryFindGame);
    //    buffer.WriteInt32(gameId);
    //    NetworkConfig.socket.SendData(buffer.Data, buffer.Head);
    //    buffer.Dispose();
    //    Debug.Log("try find game: " + gameId);
    //}
    //public static void SendUpdatePosition(Vector2 position)
    //{
    //    if (NetworkConfig.socket != null)
    //    {
    //        ByteBuffer buffer = new ByteBuffer(4);
    //        buffer.WriteInt32((int)ClientPacket.CSendUpdatePosition);
    //        buffer.WriteSingle(position.x);
    //        buffer.WriteSingle(position.y);
    //        NetworkConfig.socket.SendData(buffer.Data, buffer.Head);
    //        buffer.Dispose();
    //    }
    //}
    public static void SendHelloServer()
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPacket.CHelloServer);
        buffer.WriteString("Thank you? i now connectid to you");
        ClientTCP.SendData(buffer.ToByteArray());
        buffer.Dispose();
    }
    public static void TryCreateAccount(string name, string password)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPacket.CTryCreateAccount);
        buffer.WriteString(name);
        buffer.WriteString(password);
        ClientTCP.SendData(buffer.ToByteArray());
        buffer.Dispose();
    }
    public static void TryOpenAccount(string name, string password)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPacket.CTryOpenAccount);
        buffer.WriteString(name);
        buffer.WriteString(password);
        ClientTCP.SendData(buffer.ToByteArray());
        buffer.Dispose();
    }
    public static void StartFindGame(int gameId)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPacket.CTryFindGame);
        buffer.WriteInteger(gameId);
        ClientTCP.SendData(buffer.ToByteArray());
        buffer.Dispose();
        Debug.Log("try find game: " + gameId);
    }
}
