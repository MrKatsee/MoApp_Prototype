using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    private static ClientManager instance = null;
    public static ClientManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    TcpClient client;
    NetworkStream stream;
    StreamReader reader;
    StreamWriter writer;
    bool connected = false;

    void ConnectToServer()
    {
        if (connected)
            return;

        Debug.Log("Trying to Connect to Server...");

        try
        {
            client = new TcpClient("127.0.0.1", 19900);
            stream = client.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            connected = true;

            writer.WriteLine("Connected");
            writer.Flush();
        }
        catch (Exception e)
        {
            Debug.Log("ErrMsg : " + e);
        }

    }

    void Update()
    {
        if (!connected)
        {
            return;
        }

        if (stream.DataAvailable)
        {
            string data = reader.ReadLine();

            if (data != null)
            {
                OnIncommingData(data);
            }
        }
    }

    void OnIncommingData(string data)
    {
        Debug.Log("Msg : " + data);
    }

    void SendMsg(string data)
    {
        if (!connected)
            return;

        writer.WriteLine(data);
        writer.Flush();
    }
}