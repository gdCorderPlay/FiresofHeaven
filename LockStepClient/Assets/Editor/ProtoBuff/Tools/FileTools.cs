using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileTools 
{

    public static List<string> GetDirectorys(string pathRoot)
    {
        List<string> results = new List<string>();
        string[] subDirectories = Directory.GetDirectories(pathRoot);
        results.AddRange(subDirectories);
        for (int i = 0; i < subDirectories.Length; i++)
        {
            results.AddRange(GetDirectorys(subDirectories[i]));
        }
        return results;
    }

    public static List<string> GetFiles(string path, string searchPattern)
    {
        List<string> results = new List<string>();
        string[] files = Directory.GetFiles(path, searchPattern);
        for (int i = 0; i < files.Length; i++)
        {
            results.Add(Path.GetFileName(files[i]));
        }
        return results;
    }
    public static List<string> GetAllFilesInRoot(string path, string searchPattern)
    {
        List<string> results = new List<string>();
        results.AddRange(GetFiles(path,searchPattern));
        string[] subDirectories = Directory.GetDirectories(path);
        for (int i = 0; i < subDirectories.Length; i++)
        {
            results.AddRange(GetFiles(subDirectories[i], searchPattern));
        }
        return results;
    }
}
