using Proto.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    SimpleSocket socket;
    MainView mainView;
    // Start is called before the first frame update
    void Start()
    {
        mainView  = new MainView();
        Loom loom = Loom.Instance;
        MessageMgr.Instance.AddListener("StartBattle", StartBattle);
        MessageMgr.Instance.AddListener<FrameData>("LockStepLogic", StepLogic);
        MessageMgr.Instance.AddListener<LoginRespond>("OnLoginRespond", OnLoginRespond);
        socket = new SimpleSocket();
        socket.Init();


        mainView.startBtn.onClick.AddListener(WaitBattle);
        mainView.exitBtn.onClick.AddListener(CancleBattle);
        mainView.loginBtn.onClick.AddListener(Login);
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


    }

    private void WaitBattle()//匹配对手
    {


    }
    private void CancleBattle()//取消匹配
    {


    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < roles.Count; i++)
        {
            roles[i].model.position = roles[i].nextStepPos;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            socket.sendBattleRecordToServer(10000);
        }
        if (Input.GetKey(KeyCode.W))
        {
            socket.sendBattleRecordToServer(10001);

        }
        else
        if (Input.GetKey(KeyCode.S))
        {
            socket.sendBattleRecordToServer(10002);


        }
    }

    void StepLogic(FrameData frame)
    {
        for (int i = 0; i < frame.Commands.Count; i++)
        {
            DoCommand(frame.Commands[i]);
        }
        for (int i = 0; i < roles.Count; i++)
        {
            roles[i].Move();
        }
    }

    void DoCommand(Command command)
    {
        Debug.Log(command.CommandID);
        if (command.CommandID == 10000)
        {
            CreateRole(command.PlayerID);
        }
        else if (command.CommandID == 10001)
        {
            roles.Find(a => { return a.roleID == command.PlayerID; }).velicty = Vector3.forward;

        }
        else if (command.CommandID == 10002)
        {

            roles.Find(a => { return a.roleID == command.PlayerID; }).velicty = Vector3.back;

        }
    }

    void CreateRole(int roleID)
    {

        Role role = new Role();
        role.model = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        role.speed = 1;
        role.velicty = Vector3.zero;
        role.lastStepPos = role.nextStepPos = Vector3.zero;

        role.roleID = roleID;
        roles.Add(role);
    }
    List<Role> roles = new List<Role>();
 }

public class Role
{
    public int roleID;
    public Transform model;
    public Vector3 velicty;

    public float speed;
    public Vector3 lastStepPos;
    public Vector3 nextStepPos;

    public void Move()
    {
        Vector3 temp = lastStepPos;
        lastStepPos = nextStepPos;
        nextStepPos = temp + velicty * 0.05f * speed;
    }
}
