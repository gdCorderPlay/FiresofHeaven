using Coldairarrow.Util.Sockets;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using LockStepServer.Common;
using Proto.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LockStepServer
{
    class Battle
    {
        private int id1;
        private int id2;
        private int frameCount;
        private int waitCount = 2;
        private Dictionary<int, SocketConnection> connects;
        static Dictionary<int, FrameData> frames = new Dictionary<int, FrameData>();
        Queue<Command> commandBuffer = new Queue<Command>();
        object lockobj = new object();
        public Battle(int player1, int player2, Dictionary<int, SocketConnection> connects)
        {
            id1 = player1;
            id2 = player2;
            this.connects = connects;
        }
        public void Start()
        {
            Server2ClientData data1 = new Server2ClientData();
            data1.CommandID = MessageID.MatchResond;
            data1.Data = Any.Pack(new MatchRespond(){
                Mode=0
            });
            connects[id1].Send(data1.ToByteArray());
            connects[id1].HandleRecMsg = HandleRecMsg;
            Server2ClientData data2 = new Server2ClientData();
            data2.CommandID = MessageID.MatchResond;
            data2.Data = Any.Pack(new MatchRespond()
            {
                Mode = 1
            });
            connects[id2].Send(data2.ToByteArray());
            connects[id2].HandleRecMsg = HandleRecMsg;

            Program.IntervalDo(StepLogic);
        }
        void HandleRecMsg(byte[] bytes, SocketConnection client, SocketServer theServer)
        {
            Client2ServerData data = Client2ServerData.Parser.ParseFrom(bytes);
            switch (data.CommandID)
            {
                case MessageID.ReadyForBattle:
                    waitCount--;
                    break;
                case MessageID.RemoteFuction:
                    if (waitCount <= 0)
                    {
                        commandBuffer.Enqueue( data.Data.Unpack<Command>());
                    }
                    break;
            }
        }
        void DoStepLogic()
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
       
         void StepLogic(object sender, ElapsedEventArgs e)
        {
            if (waitCount <= 0)
            {
                DoStepLogic();
            }
           
        }
         void Send(int frame)
        {
            FrameData frameData;
            if (frames.ContainsKey(frame))
            {
                frameData = frames[frame];
            }
            else
            {
                frameData = new FrameData();
                frameData.FrameCount = frame;
            }
            Server2ClientData respondData = new Server2ClientData();
            respondData.CommandID = MessageID.RemoteFuction;
            respondData.Data = Any.Pack(frameData);
            connects[id1].Send(respondData.ToByteArray());
            connects[id2].Send(respondData.ToByteArray());
        }
    }
}
