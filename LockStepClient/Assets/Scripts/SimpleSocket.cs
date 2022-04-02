using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Proto.Data;
using Google.Protobuf;

public class SimpleSocket
{

    private Socket socketClient;

    // Use this for initialization
    public void Init()
    {
        //创建实例
        socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ip = IPAddress.Parse("192.168.0.103");
        IPEndPoint point = new IPEndPoint(ip, 2333);
        //进行连接
        socketClient.Connect(point);

        //不停的接收服务器端发送的消息
        Thread thread = new Thread(Recive);
        thread.IsBackground = true;
        thread.Start(socketClient);
    }

    /// <summary>
    /// 接收消息
    /// </summary>
    /// <param name="o"></param>
    static void Recive(object o)
    {
        var send = o as Socket;
        byte[] buffer = new byte[1024 * 1024 * 2];
        while (true)
        {
            //获取发送过来的消息
            var effective = send.Receive(buffer);
            if (effective == 0)
            {
                //break;
            }
            byte[] data = new byte[effective];
            Array.Copy( buffer,data, effective);
            Server2ClientData serverData = Server2ClientData.Parser.ParseFrom(data);
            switch (serverData.CommandID)
            {
                case MessageID.Login:
                    LoginRespond respond = serverData.Data.Unpack<LoginRespond>();
                    Loom.AddNetMsgHandle(() =>
                    {
                        MessageMgr.Instance.SendMsg<LoginRespond>("OnLoginRespond", respond);
                    });
                    break;
                case MessageID.MatchResond:
                    MatchRespond matchRespond = serverData.Data.Unpack<MatchRespond>();
                    StaticDef.playerMode = matchRespond.Mode;
                    Loom.AddNetMsgHandle(() =>
                    {
                        MessageMgr.Instance.SendMsg("OnMatchRespond");
                    });
                    break;
                case MessageID.RemoteFuction:
                    FrameData frame = serverData.Data.Unpack<FrameData>();
                    Loom.AddNetMsgHandle(() =>
                    {
                        MessageMgr.Instance.SendMsg("LockStepLogic", frame);
                    });
                    break;
            }
        }
    }
    public static int playerID=1;
    
    public void SendData2Server(Client2ServerData sendData)
    {
        byte[] data = sendData.ToByteArray();
        socketClient.Send(data);
    }
    public void Close()
    {
        try
        {
            socketClient.Shutdown(SocketShutdown.Both);
        }
        finally
        {
            socketClient.Close();
        }
    }
}