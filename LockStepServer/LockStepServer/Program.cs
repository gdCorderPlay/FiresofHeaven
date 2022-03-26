using Coldairarrow.Util.Sockets;
using Proto.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Timers;
using Google.Protobuf;
using LockStepServer.Common;
using Any =Google.Protobuf.WellKnownTypes.Any;
namespace LockStepServer
{
    class Program
    {
        static Dictionary<int, FrameData> frames = new Dictionary<int, FrameData>();
        static Queue<Command> commandBuffer = new Queue<Command>();
        static int frameCount;
        static SocketServer server;
        static object lockobj = new object();
        static void Main(string[] args)
        {
            //创建服务器对象，默认监听本机0.0.0.0，端口12345
            server = new SocketServer(2333);

            //处理从客户端收到的消息
            server.HandleRecMsg = new Action<byte[], SocketConnection, SocketServer>((bytes, client, theServer) =>
            {
                Client2ServerData data = Client2ServerData.Parser.ParseFrom(bytes);
                switch (data.CommandID)
                {
                    case MessageID.Login:
                        LoginRespond respond = new LoginRespond();
                        respond.ID = UIDHelper.GetUID();
                        Server2ClientData respondData = new Server2ClientData();
                        respondData.CommandID = MessageID.Login;
                        respondData.Data = Any.Pack(respond); ;
                        client.Send(respondData.ToByteArray());
                        break;
                }
                //int num = frameCount;
                //Command command = Command.Parser.ParseFrom(bytes);
                //Console.WriteLine(command.CommandID+"::"+command.PlayerID);
                //commandBuffer.Enqueue(command);
            });

            //处理服务器启动后事件
            server.HandleServerStarted = new Action<SocketServer>(theServer =>
            {
                Console.WriteLine("服务已启动************");
            });

            //处理新的客户端连接后的事件
            server.HandleNewClientConnected = new Action<SocketServer, SocketConnection>((theServer, theCon) =>
            {
                Console.WriteLine($@"一个新的客户端接入，当前连接数：{theServer.GetConnectionCount()}");
            });

            //处理客户端连接关闭后的事件
            server.HandleClientClose = new Action<SocketConnection, SocketServer>((theCon, theServer) =>
            {
                Console.WriteLine($@"一个客户端关闭，当前连接数为：{theServer.GetConnectionCount()}");
            });

            //处理异常
            server.HandleException = new Action<Exception>(ex =>
            {
                Console.WriteLine(ex.Message);
            });

            //服务器启动
            server.StartServer();

            //Timer aTimer = new Timer();
            //aTimer.Elapsed += new ElapsedEventHandler(StepLogic);
            //aTimer.Interval = 1000/20;    //配置文件中配置的秒数
            //aTimer.Enabled = true;
            Console.Read();
        }

        static void StepLogic(object sender, ElapsedEventArgs e)
        {
            int currentFrame = frameCount;
            frameCount++;
            lock (lockobj)
            {
                FrameData frame = new FrameData();
                frame.FrameCount = currentFrame;
                frames.Add(currentFrame, frame);
                while (commandBuffer.Count > 0)
                {
                    frame.Commands.Add(commandBuffer.Dequeue());
                }
            }

            Send(currentFrame);
        }
        static void Send(int frame)
        {
            byte[] data;
            if (frames.ContainsKey(frame))
            {
                data = frames[frame].ToByteArray();
            }
            else
            {
                FrameData frameData = new FrameData();
                frameData.FrameCount = frame;
                data = frameData.ToByteArray();
            }
            var clients = server.GetConnectionList();
            foreach (var client in clients)
            {
                client.Send(data);
            }
        }
    }
}
