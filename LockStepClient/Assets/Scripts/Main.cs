using Proto.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoSingleton<Main>
{
    SimpleSocket socket;
    MainView mainView;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        mainView  = new MainView();
        Loom loom = Loom.Instance;
        MessageMgr.Instance.AddListener("OnMatchRespond", StartBattle);
       
        MessageMgr.Instance.AddListener<LoginRespond>("OnLoginRespond", OnLoginRespond);
        socket = new SimpleSocket();
        socket.Init();


        mainView.startBtn.onClick.AddListener(WaitBattle);
        mainView.exitBtn.onClick.AddListener(CancleBattle);
        mainView.loginBtn.onClick.AddListener(Login);
    }

    private void OnDestroy()
    {
        socket.Close();

    }
    private void OnLoginRespond(LoginRespond respond)
    {
        StaticDef.UID = respond.ID;
        mainView.OnLoginSuccess(StaticDef.UID);

    }

    private void Login()
    {
        Proto.Data.Client2ServerData sendData = new Client2ServerData();
        sendData.CommandID = MessageID.Login;
        socket.SendData2Server(sendData);
    }
    private void StartBattle()//匹配到对手
    {
        Debug.Log("成功匹配到对手");
        mainView.Hide();
        SceneManager.LoadScene("BTreeDemo");
    }
    public void IsReadyForBattle()//
    {
        Proto.Data.Client2ServerData sendData = new Client2ServerData();
        sendData.CommandID = MessageID.ReadyForBattle;
        socket.SendData2Server(sendData);
    }
    public void Create(SoldierType soldierType,float x,float y)
    {
        Proto.Data.Client2ServerData sendData = new Client2ServerData();
        sendData.CommandID = MessageID.RemoteFuction;
        Command command = new Command();
        command.Mode = StaticDef.playerMode;
        command.SoldierType = (int)soldierType;
        command.X = (int)(x * 1000);
        command.Y = (int)(y * 1000);
        sendData.Data = Google.Protobuf.WellKnownTypes.Any.Pack(command);
        socket.SendData2Server(sendData);

    }
    private void WaitBattle()//匹配对手
    {
        Proto.Data.Client2ServerData sendData = new Client2ServerData();
        sendData.CommandID = MessageID.Match;
        MatchRequest request = new MatchRequest();
        request.Uid = StaticDef.UID;
        request.Match = true;
        sendData.Data = Google.Protobuf.WellKnownTypes.Any.Pack(request);
        socket.SendData2Server(sendData);

    }
    private void CancleBattle()//取消匹配
    {

        Proto.Data.Client2ServerData sendData = new Client2ServerData();
        sendData.CommandID = MessageID.Match;
        MatchRequest request = new MatchRequest();
        request.Uid = StaticDef.UID;
        request.Match = false;
        sendData.Data = Google.Protobuf.WellKnownTypes.Any.Pack(request);
        socket.SendData2Server(sendData);
    }

 }

