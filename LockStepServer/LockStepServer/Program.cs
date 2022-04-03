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
using System.Threading;

namespace LockStepServer
{
    class Program
    {
        static Dictionary<int, FrameData> frames = new Dictionary<int, FrameData>();
        static Dictionary<int, long> matchPools = new Dictionary<int, long>();
        static Dictionary<int, SocketConnection> connections = new Dictionary<int, SocketConnection>();
        static Queue<Command> commandBuffer = new Queue<Command>();
        static int frameCount;
        static SocketServer server;
        static object lockobj = new object();
        static void Main(string[] args)
        {
            //创建服务器对象，默认监听本机0.0.0.0，端口12345
            server = new SocketServer(2333);

            //处理从客户端收到的消息
            server.HandleRecMsg = HandleRecMsg;
            

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
                int userID = (int)theCon.Property;
                if (connections.ContainsKey(userID))
                {
                    connections.Remove(userID);
                }
                if (matchPools.ContainsKey(userID))
                {
                    matchPools.Remove(userID);
                }
                Console.WriteLine($@"一个客户端关闭，当前连接数为：{theServer.GetConnectionCount()}");
            });

            //处理异常
            server.HandleException = new Action<Exception>(ex =>
            {
                Console.WriteLine(ex.Message);
            });

            //服务器启动
            server.StartServer();

            IntervalDo(Match);
            //Timer aTimer = new Timer();
            //aTimer.Elapsed += new ElapsedEventHandler(StepLogic);
            //aTimer.Interval = 1000/20;    //配置文件中配置的秒数
            //aTimer.Enabled = true;
            Console.Read();
        }
        public static void HandleRecMsg(byte[] bytes, SocketConnection client, SocketServer theServer)
        {
            Client2ServerData data = Client2ServerData.Parser.ParseFrom(bytes);
            switch (data.CommandID)
            {
                case MessageID.Login:
                    LoginRespond respond = new LoginRespond();
                    respond.ID = UIDHelper.GetUID();
                    Server2ClientData respondData = new Server2ClientData();
                    respondData.CommandID = MessageID.Login;
                    respondData.Data = Any.Pack(respond);
                    client.Property = respond.ID;
                    connections.Add(respond.ID, client);
                    client.Send(respondData.ToByteArray());
                    break;
                case MessageID.Match:
                    MatchRequest request = data.Data.Unpack<MatchRequest>();
                    if (request.Match)//开始匹配
                    {
                        if (!matchPools.ContainsKey(request.Uid))
                        {
                            matchPools.Add(request.Uid, DateTime.Now.Ticks);
                        }
                    }
                    else//取消匹配
                    {
                        if (matchPools.ContainsKey(request.Uid))
                        {
                            matchPools.Remove(request.Uid);
                        }
                    }
                    break;
            }
        }

        public static void IntervalDo(System.Action<object,ElapsedEventArgs> action)
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(action);
            aTimer.Interval = 1000/20;    //配置文件中配置的秒数
            aTimer.Enabled = true;

        }
        //匹配算法
        static void Match(object sender, ElapsedEventArgs args)
        {
            List<int> matching= matchPools.Keys.ToList();
            int count = matching.Count;
            for (int i = 0; i < count-1;)
            {
                int user1 = matching[i];
                int user2 = matching[i + 1];
                i += 2;
                matchPools.Remove(user1);
                matchPools.Remove(user2);
                CreateBattle(user1,user2);
            }
        }
        static void CreateBattle(int user1, int user2)
        {
            Battle battle = new Battle(user1, user2, connections);
            Thread thread = new Thread(battle.Start);
            thread.IsBackground = true;
            thread.Start();
        }

       
    }
}
