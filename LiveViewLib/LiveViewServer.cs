using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InTheHand.Net.Sockets;
using System.Threading;
using InTheHand.Net.Bluetooth;

namespace LiveViewLib
{
    /// <summary>
    /// Outward facing class that represents the server the LiveViews connect to. 
    /// Manages connections and LiveViews.
    /// </summary>
    public class LiveViewServer
    {
        public const string CLIENT_SOFTWARE_VERSION = "0.0.3";

        public delegate void ConnectListener(LiveView liveView);

        protected BluetoothListener listener;
        protected Thread connectThread;

        public event ConnectListener connectListener;
        protected List<LiveView> liveViews;

        public LiveViewServer()
        {
            liveViews = new List<LiveView>();
        }

        /// <summary>
        /// Called to start the server.
        /// </summary>
        public void Start()
        {
            this.listener = new BluetoothListener(BluetoothService.SerialPort);
            this.listener.ServiceClass = ServiceClass.Information;
            this.listener.ServiceName = "LiveView";

            this.connectThread = new Thread(new ThreadStart(AcceptClientLoop));
            this.connectThread.IsBackground = true;
            this.connectThread.Start();
        }

        /// <summary>
        /// Loop that gets executed in separate thread and listens for LiveViews wanting to connect.
        /// </summary>
        protected void AcceptClientLoop()
        {
            this.listener.Start();
            while (true)
            {
                Console.WriteLine("Listening  for client...");
                BluetoothClient client = this.listener.AcceptBluetoothClient();

                bool isAdded = false;
                foreach (LiveView liveView in liveViews)
                {
                    if (client.RemoteEndPoint.Address.Equals(liveView.GetAddress()))
                    {
                        Console.WriteLine(string.Format("Client '{0}' reconnected", client.RemoteEndPoint.Address.ToString()));
                        liveView.Reconnect(client);
                        isAdded = true;
                    }
                }

                if (!isAdded)
                {
                    Console.WriteLine(string.Format("Client '{0}' connected", client.RemoteEndPoint.Address.ToString()));

                    LiveView liveView = new LiveView(client);
                    liveViews.Add(liveView);

                    if(connectListener != null) connectListener(liveView);

                    liveView.Start();
                }
            }
        }

        /// <summary>
        /// Returns all LiveViews that were connected to this server.
        /// </summary>
        /// <returns>All LiveViews.</returns>
        public List<LiveView> GetLiveViews()
        {
            return liveViews;
        }

        /// <summary>
        /// Helper function that sends a message to all connect LiveViews.
        /// </summary>
        /// <param name="message">Message to send.</param>
        public void SendAll(LiveViewMessage message)
        {
            foreach (LiveView liveView in liveViews)
            {
                liveView.Send(message);
            }
        }
    }
}
