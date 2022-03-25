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
//public class Command
//{
//    public int playerID;
//    public int commandID;
//    public Command()
//    {

//    }
//    public Command(byte[] data)
//    {
//        playerID = BitConverter.ToInt32(data, 0);
//        commandID = BitConverter.ToInt32(data, 4);
//    }
//    public static Command Convert(byte[] data, ref int offset)
//    {
//        Command c = new Command();

//        c.playerID = BitConverter.ToInt32(data, offset);
//        offset += 4;
//        c.commandID = BitConverter.ToInt32(data, offset);
//        offset += 4;

//        return c;
//    }
//    public void InsertArray(byte[] data, ref int offset)
//    {
//        BitConverter.GetBytes(playerID).CopyTo(data, offset);
//        offset += 4;
//        BitConverter.GetBytes(commandID).CopyTo(data, offset);
//        offset += 4;
//    }

//    public byte[] GetData()
//    {
//        int offset = 0;

//        byte[] data = new byte[8];
//        InsertArray(data, ref offset);
//        return data;
//    }
//}
//public class FrameData
//{
//    public int frameCount;
//    public List<Command> commands;

//    public FrameData()
//    {
//        commands = new List<Command>();

//    }
//    public FrameData(byte[] data, int length)
//    {
//        int offset = 0;
//        frameCount = BitConverter.ToInt32(data, offset);
//        commands = new List<Command>();
//        offset += 4;
//        while (offset < length)
//        {
//            Command command = Command.Convert(data, ref offset);
//            commands.Add(command);
//        }

//    }
//    public byte[] GetData()
//    {
//        int offset = 0;

//        byte[] data = new byte[4 + commands.Count * 8];
//        BitConverter.GetBytes(frameCount).CopyTo(data, 0);
//        offset += 4;
//        for (int i = 0; i < commands.Count; i++)
//        {
//            commands[i].InsertArray(data, ref offset);
//        }
//        return data;
//    }
//}

public class SimpleSocket
{

    private Socket socketClient;

    // Use this for initialization
    public void Init()
    {
        //创建实例
        socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ip = IPAddress.Parse("127.0.0.1");
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
            //FrameData frame = new FrameData(buffer,effective);
            FrameData frame = FrameData.Parser.ParseFrom(data);
            Loom.AddNetMsgHandle(() =>
            {
                MessageMgr.Instance.SendMsg<FrameData>("LockStepLogic",frame);
            });
        }
    }
    public static int playerID=1;
    public void sendBattleRecordToServer(int commandID)
    {
        Command command = new Command();
        command.CommandID = commandID;
        command.PlayerID = playerID;
        byte[] data = command.ToByteArray();
        Debug.Log(commandID+"  length:" +data.Length);
        socketClient.Send(data);
        // var temp = socketClient.Send(command.GetData());
    }
}