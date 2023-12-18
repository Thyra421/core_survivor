using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

public static class MasterServerHelper
{
    private const string PROGRAM_PATH =
        @"C:/Users/test/RiderProjects/ConsoleApp2/ConsoleApp2/bin/Debug/net7.0/ConsoleApp2.exe";

    private static int _lastInstancePort = 7778;

    private static bool IsPortAvailable(int port)
    {
        try
        {
            using UdpClient udpClient = new();
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));
            return true;
        }
        catch (SocketException)
        {
            return false;
        }
    }

    private static int FindFirstPortAvailable()
    {
        int port = _lastInstancePort;
        int cpt = 500;

        while (!IsPortAvailable(port) && cpt > 0)
        {
            port++;
            cpt--;
        }

        return port;
    }

    public static void StartDedicatedServerInstance(string name)
    {
        try
        {
            int port = FindFirstPortAvailable();
            GameInstance gameInstance = new() { port = port, name = name };
        
            Process newProcess = new()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = PROGRAM_PATH,
                    Arguments = port.ToString(),
                    UseShellExecute = true
                },
                EnableRaisingEvents = true
            };
            newProcess.Exited += (sender, e) => GameInstanceManager.Current.Remove(gameInstance);
        
            bool status = newProcess.Start();
        
            if (!status) return;
        
            GameInstanceManager.Current.Add(gameInstance);
            _lastInstancePort = port;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting the process: {ex.Message}");
        }
    }
}