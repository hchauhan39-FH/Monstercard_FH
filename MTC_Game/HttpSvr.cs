﻿using MTC_Game;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MTC_Game
{
    /// <summary>This class implements a HTTP server.</summary>
    public sealed class HttpSvr
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private members                                                                                                  //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>TCP listener instance.</summary>
        private TcpListener? _Listener;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public events                                                                                                    //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Is raised when incoming data is available.</summary>
        public event HttpSvrEventHandler? Incoming;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets if the server is available.</summary>
        public bool Active { get; private set; } = false;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public methods                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Runs the server.</summary>
        public void Run()
        {
            if (Active) return;

            Active = true;
            _Listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 12000);
            _Listener.Start();

            byte[] buffer = new byte[256];

            while (Active)
            {
                TcpClient client = _Listener.AcceptTcpClient();
                HandleClient(client, buffer);
            }
        }
        /// <summary>MEthod to handle client</summary>
        private void HandleClient(TcpClient client, byte[] buffer)
        {
            try
            {
                string data = string.Empty;
                using (var stream = client.GetStream())
                {
                    while (stream.DataAvailable || string.IsNullOrWhiteSpace(data))
                    {
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        data += Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    }

                    // Trigger the event for incoming data processing
                    Incoming?.Invoke(this, new HttpSvrEventArgs(client, data));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
            finally
            {
                client.Close(); 
            }
        }

        /// <summary>Stops the server.</summary>
        public void Stop()
        {
            Console.WriteLine("Stopping server...");
            Active = false;
            _Listener?.Stop();
        }
    }
}