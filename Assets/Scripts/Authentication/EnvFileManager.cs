using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Web;
using Newtonsoft.Json.Linq;

public class EnvFileManager : MonoBehaviour
{
    public static EnvFileManager Instance { get; private set; }

    public Dictionary<string, string> envVariables;

    private string envFilePath = Path.Combine(Application.streamingAssetsPath, "file.env");

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        envVariables = ReadEnvFile(envFilePath);
    }


    private Dictionary<string, string> ReadEnvFile(string path)
    {
        Dictionary<string, string> env = new Dictionary<string, string>();
        string[] lines = File.ReadAllLines(path);

        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
            {
                string[] keyValue = line.Split(new[] { '=' }, 2);
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim();
                    string value = keyValue[1].Trim();

                    // Replace double underscores with a single underscore
                    key = key.Replace("__", "_");

                    env[key] = value;
                }
            }
        }
        return env;
    }
}
