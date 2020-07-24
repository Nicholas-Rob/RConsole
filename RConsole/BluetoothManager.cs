using InTheHand.Net;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace RConsole
{
    class BluetoothManager
    {
        RConsoleBase console;
        bool Running = false;
        BluetoothListener listener;

        const string PROX = "music toggle";

        public BluetoothManager(RConsoleBase console)
        {
            this.console = console;   
        }

        public bool Run()
        {
            // Returns true if bluetooth starts listening.
            // Returns false if bluetooth is already running.
            if (!Running)
            {
                Running = true;
                listener = new BluetoothListener(Guid.Parse("2d26618601fb47c28d9f10b8ec891363"));
                listener.Start();
                

                listener.BeginAcceptBluetoothClient(new AsyncCallback(AcceptConnection), listener);

                return true;
            }
            return false;
        }

        private void AcceptConnection(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                Console.WriteLine("Bluetooth Device Connected.");
                BluetoothClient device = ((BluetoothListener)result.AsyncState).EndAcceptBluetoothClient(result);

                NetworkStream stream = device.GetStream();

                byte[] buffer = new byte[128];

                while (Running)
                {
                    if (device.Connected)
                    {
                        try
                        {
                            int bytesReceived = stream.Read(buffer, 0, buffer.Length);

                            string message = Encoding.UTF8.GetString(buffer, 0, bytesReceived);

                            if (message != "")
                            {


                                if (message == "%QUITB%")
                                {
                                    Running = false;
                                    Console.WriteLine("Bluetooth device disconnected.");
                                }
                                else
                                {
                                    if (message == "%PROX%")
                                    {
                                        console.ExecuteCommand(PROX);
                                    }
                                    else
                                    {
                                        console.ExecuteCommand(message);
                                    }


                                    buffer = new byte[128];
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Running = false;
                        }
                    }
                    else
                    {
                        Running = false;

                    }
                }

                stream.Close();
                device.Dispose();
                listener.Stop();

            }
        }

        
    }
}
