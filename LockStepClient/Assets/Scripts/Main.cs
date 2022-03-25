using Proto.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    SimpleSocket socket;
    // Start is called before the first frame update
    void Start()
    {
        Loom loom = Loom.Instance;
        MessageMgr.Instance.AddListener<FrameData>("LockStepLogic",StepLogic);
        socket = new SimpleSocket();
        socket.Init();
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
