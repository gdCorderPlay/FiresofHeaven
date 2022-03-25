using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;
using Proto.Data;
using Google.Protobuf;
namespace LockStepServer
{
    //public class Command
    //{
    //    public int playerID;
    //    public int commandID;
    //    public Command()
    //    {

    //    }
    //    public Command(byte[] data)
    //    {
    //        playerID = BitConverter.ToInt32(data,0);
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
    //    public void InsertArray(byte[] data ,ref int offset)
    //    {
    //        BitConverter.GetBytes(playerID).CopyTo(data, offset);
    //        offset += 4;
    //        BitConverter.GetBytes(commandID).CopyTo(data, offset);
    //        offset += 4;
    //    }

    //    public byte[] GetData()
    //    {
    //       int offset = 0;

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
    //    public byte[] GetData()
    //    {
    //        int offset = 0;

    //        byte[] data = new byte[4 + commands.Count * 8];
    //        BitConverter.GetBytes(frameCount).CopyTo(data,0);
    //        offset += 4;
    //        for (int i = 0; i < commands.Count; i++)
    //        {
    //            commands[i].InsertArray(data, ref offset);
    //        }
    //        return data;
    //    }
    //}
    public  class NetworkServer
    {
        private List<Socket> sockets = new List< Socket>();
        Dictionary<int, FrameData> frames = new Dictionary<int, FrameData>();
        int frameCount;
        public void Init(string ip, int port)
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IPAddress ip = IPAddress.Any;
            //IPEndPoint point = new IPEndPoint(ip, 2333);
            IPEndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);
            //socket绑定监听地址
            serverSocket.Bind(point);
            Console.WriteLine("Listen Success");
            //设置同时连接个数
            serverSocket.Listen(10);

            //利用线程后台执行监听,否则程序会假死
            Thread thread = new Thread(Listen);
            thread.IsBackground = true;
            thread.Start(serverSocket);
        }

        /// <summary>
        /// 监听连接
        /// </summary>
        /// <param name="o"></param>
        public void Listen(object o)
        {
            var serverSocket = o as Socket;
            while (true)
            {
                //等待连接并且创建一个负责通讯的socket
                var send = serverSocket.Accept();
                //获取链接的IP地址
                var sendIpoint = send.RemoteEndPoint.ToString();
                sockets.Add(send);
                Console.WriteLine($"{sendIpoint}Connection");
                //开启一个新线程不停接收消息
                Thread thread = new Thread(Recive);
                thread.IsBackground = true;
                thread.Start(send);
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="o"></param>
        public void Recive(object o)
        {
            var send = o as Socket;
            byte[] buffer = new byte[1024 * 1024 * 2];

            while (true)
            {
                //获取发送过来的消息容器
                var effective = send.Receive(buffer);

                Console.WriteLine("effective: " + effective);

                //有效字节为0则跳过
                if (effective == 0)
                {
                    break;
                }
                Command command = Command.Parser.ParseFrom(buffer);
                //Command command = new Command(buffer);
                if (frames.ContainsKey(frameCount))
                {
                    frames[frameCount].Commands.Add(command);
                }
                else
                {
                    FrameData frame = new FrameData();
                    frame.FrameCount = frameCount;
                    frame.Commands.Add(command);
                    frames.Add(frameCount,frame);
                }
            }
        }

        public void StepLogic(object sender, ElapsedEventArgs e)
        {
            int currentFrame = frameCount;
            frameCount++;

            Send(currentFrame);
        }
        public void Send(int frame)
        {
            byte[] data;
            if (frames.ContainsKey(frame))
            {
                // data = frames[frame].GetData();
                data = frames[frame].ToByteArray();
            }
            else
            {
                FrameData frameData = new FrameData();
                frameData.FrameCount = frame;
                //data = frameData.GetData();
                data = frameData.ToByteArray();
            }
            for (int i = 0; i < sockets.Count; i++)
            {
                if (sockets[i] != null)
                {
                    sockets[i].Send(data);
                }

            }
        }
        public void Send(int player, byte[] data)
        {


        }

        public void Send(int player, int messageID, byte[] data)
        {
            Socket send = sockets[player];
            if (send!=null)
            {
                send.Send(data);
            }
        }
    }
}
