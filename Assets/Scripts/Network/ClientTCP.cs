using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

static class ClientTCP
{
    private static Socket clientSocket2;
    //private static TcpClient clientSocket;
    private static NetworkStream myStream;
    private static byte[] recBuffer;

    public static void InitializeNetworking()
    {
        //clientSocket = new TcpClient();
        //clientSocket.ReceiveBufferSize = 4096;
        //clientSocket.SendBufferSize = 4096;
        //clientSocket.BeginConnect("localHost", 5555, new AsyncCallback(ClientConnectCallback), clientSocket);
        return;
        recBuffer = new byte[4096 * 2];
        clientSocket2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket2.ReceiveBufferSize = 4096;//185.6.24.112//169.254.246.215
        clientSocket2.SendBufferSize = 4096;//"192.168.0.107"
        const string ip = "169.254.246.215";
        const int port = 14732;
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        clientSocket2.BeginConnect(ipPoint, new AsyncCallback(ClientConnectCallback), clientSocket2);
    }
    public static void ClientConnectCallback(IAsyncResult result)
    {
        clientSocket2.EndConnect(result);
        if (clientSocket2.Connected == false) return;
        else clientSocket2.NoDelay = true;
        //myStream = clientSocket2.GetStream();        
        const string ip = "192.168.100.166";//"185.6.24.112";//"192.168.0.107";//"185.6.24.112";
        const int port = 14732;
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        clientSocket2.Bind(ipPoint);
        myStream = new NetworkStream(clientSocket2);
        myStream.BeginRead(recBuffer, 0, 8192, ReceiveCallback, null);
    }
    private static void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            int length = myStream.EndRead(result);
            if (length <= 0)
            {
                return;
            }
            byte[] newBytes = new byte[length];
            Array.Copy(recBuffer, newBytes, length);
            UnityThread.executeInFixedUpdate(() =>
            {
                ClientHandleData.HandleData(newBytes);
            });
            myStream.BeginRead(recBuffer, 0, 8192, ReceiveCallback, null);
        }
        catch (Exception e)
        {
            return;
        }

    }
    public static void SendData(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
        buffer.WriteBytes(data);
        myStream.BeginWrite(buffer.ToByteArray(), 0, buffer.ToByteArray().Length, null, null);
        buffer.Dispose();
    }
    public static void Disconnect()
    {
        clientSocket2.Close();
    }
}
