using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class ClientHandleData
{
    private static ByteBuffer playerBuffer;
    public delegate void Packet(byte[] data);
    public static Dictionary<int, Packet> packets = new Dictionary<int, Packet>();

    public static void InitializePackets()
    {
        NetworkReceive.InitPackets();
    }
    public static void HandleData(byte[] data)
    {
        byte[] buffer = (byte[])data.Clone();
        int pLength = 0;
        if (playerBuffer == null)
            playerBuffer = new ByteBuffer();
        playerBuffer.WriteBytes(buffer);
        if (playerBuffer.Count() == 0)
        {
            playerBuffer.Clear();
            return;
        }
        if (playerBuffer.Length() >= 4)
        {
            pLength = playerBuffer.ReadInteger(false);
            if (pLength <= 0)
            {
                playerBuffer.Clear();
                return;
            }
        }

        while (pLength > 0 & pLength <= playerBuffer.Length() - 4)
        {
            if (pLength <= playerBuffer.Length() - 4)
            {
                playerBuffer.ReadInteger();
                data = playerBuffer.ReadBytes(pLength);
                HandleDataPackets(data);
            }

            pLength = 0;
            if (playerBuffer.Length() >= 4)
            {
                if (playerBuffer.Length() >= 4)
                {
                    pLength = playerBuffer.ReadInteger(false);
                    if (pLength <= 0)
                    {
                        playerBuffer.Clear();
                        return;
                    }
                }
            }
        }

        if (pLength <= 1)
        {
            playerBuffer.Clear();
        }
    }
    private static void HandleDataPackets(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        int packetId = buffer.ReadInteger();
        buffer.Dispose();
        if (packets.TryGetValue(packetId, out Packet packet))
        {
            packet.Invoke(data);
        }
    }
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ClientHandleData : MonoBehaviour
//{
//    //public static ClientHandleData instance;
//    //private void Awake()
//    //{
//    //    instance = this;
//    //}

//    //void HandleMessage(int PacketNum, byte[] data)
//    //{
//    //    switch (PacketNum)
//    //    {
//    //        case 1:
//    //            HandleImage(PacketNum, data);
//    //            break;
//    //        case 2:

//    //            break;
//    //    }
//    //}
//    //public void HandleData(byte[] data)
//    //{
//    //    //int packetNum;
//    //    int packetNum = 0;
//    //    /////
//    //    ////
//    //    //////
//    //    //////
//    //    if (packetNum == 0)
//    //        return;

//    //    HandleMessage(packetNum, data);
//    //}

//    //void HandleImage(int packetNum, byte[] data)
//    //{
//    //    MenuManager.instance._menu = MenuManager.Menu.InGame;
//    //}
//}
