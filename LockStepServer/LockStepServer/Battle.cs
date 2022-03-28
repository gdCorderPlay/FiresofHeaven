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

namespace LockStepServer
{
    class Battle
    {
        private int id1;
        private int id2;
        private int frame;
        private Dictionary<int, SocketConnection> connects;
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

            Server2ClientData data2 = new Server2ClientData();
            data2.CommandID = MessageID.MatchResond;
            data2.Data = Any.Pack(new MatchRespond()
            {
                Mode = 1
            });
            connects[id2].Send(data2.ToByteArray());

        }
        void DoStepLogic(int frame)
        {

        }
        public void SetCommand(Command command)
        {

        }
    }
}
