using InTheHand.Net;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace RConsole
{
    class BluetoothManager : IHardware
    {
        RConsoleBase console;
        bool Running = false;

        BluetoothListener listener;
        BluetoothClient device;
        NetworkStream stream;

        const string PROX = "music toggle";

        public BluetoothManager(RConsoleBase console)
        {
            this.console = console;   
        }

        ~BluetoothManager()
        {

            if (listener != null) listener.Stop();
            if (stream != null) stream.Close();
            if (device != null) device.Dispose();
        }

        public bool Run()
        {
            // Returns true if bluetooth starts listening.
            // Returns false if bluetooth is already running.
            if (!Running)
            {
                Running = true;
                listener = new BluetoothListener(Guid.Parse("2d26618601fb47c28d9f10b8ec891363")); // Same as in the app
                listener.Start();
                

                listener.BeginAcceptBluetoothClient(new AsyncCallback(AcceptConnection), listener);

                return true;
            }
            return false;
        }

        public void Stop()
        {
            if (Running)
            {
                Running = false;
                
                if (listener != null)
                {
                    
                    stream.Close();
                    device.Dispose();
                }
                
            }
            
            
        }

        private void AcceptConnection(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                Console.WriteLine("Bluetooth Device Connected.");
                device = ((BluetoothListener)result.AsyncState).EndAcceptBluetoothClient(result);

                stream = device.GetStream();

                byte[] buffer = new byte[128];

                while (Running)
                {
                    if (device.Connected)
                    {
                        try
                        {
                            int bytesReceived = stream.Read(buffer, 0, buffer.Length);

                            string message = Encoding.UTF8.GetString(buffer, 0, bytesReceived).Trim();

                            if (message != "")
                            {

                                //Console.WriteLine(message);
                                if (message.StartsWith("%QUITB%"))
                                {
                                    Running = false;
                                    Console.WriteLine("Bluetooth device disconnected.");
                                }
                                else
                                {
                                    if (message.StartsWith("%PROX%"))
                                    {
                                        console.ExecuteCommand(PROX,false);
                                    }
                                    else
                                    {
                                        console.ExecuteCommand(message,false);
                                    }


                                    Array.Clear(buffer, 0, buffer.Length);
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
