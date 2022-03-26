using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

public static class ProtoEditor 
{
    [MenuItem("GD/Proto/Proto2CSharp")]
    public static void Proto2CSharp()
    {
        string protoc = @Application.dataPath + ProtoDefine.protoc;
        string protoPath= @Application.dataPath + ProtoDefine.protoPath;
        string csharpOut = @Application.dataPath + ProtoDefine.csharpOut;
        string includePath = @Application.dataPath + ProtoDefine.include;
        CommandRunner command = new CommandRunner(protoc,protoPath);
        List<string> paths = FileTools.GetDirectorys(protoPath);
        List<string> protos = FileTools.GetAllFilesInRoot(protoPath,"*.proto");

        StringBuilder sb = new StringBuilder();
        //���������csharp·��
        sb.Append(ProtoDefine.space);
        sb.Append(ProtoDefine.command_csharp_out);
        sb.Append(csharpOut);

        //����include ·��
        sb.Append(ProtoDefine.space);
        sb.Append(ProtoDefine.command_proto_path);
        sb.Append(includePath);

        //����proto�ļ�·��
        sb.Append(ProtoDefine.space);
        sb.Append(ProtoDefine.command_proto_path);
        sb.Append(protoPath);

        for (int i = 0; i < paths.Count; i++)
        {
            sb.Append(ProtoDefine.space);
            sb.Append(ProtoDefine.command_proto_path);
            sb.Append(paths[i]);
        }

        //������Ҫת����proto�ļ�
        for (int i = 0; i < protos.Count; i++)
        {
            sb.Append(ProtoDefine.space);
            sb.Append(protos[i]);
        }
        string args = sb.ToString();
        Debug.Log(args);
        //��ʼת��
        string result=  command.Run(args);
        AssetDatabase.Refresh();
    }
}
public class ProtoDefine
{
    public const string protoc = "/Editor/ProtoBuff/bin/protoc.exe";
    public const string include = "/Editor/ProtoBuff/include";
    public const string protoPath = "/Data/Proto";
    public const string csharpOut = "/Scripts/ProtoBuff/CSharp";

    public const string command_proto_path = "--proto_path=";
    public const string command_csharp_out = "--csharp_out=";
    public const string space = " ";

}
