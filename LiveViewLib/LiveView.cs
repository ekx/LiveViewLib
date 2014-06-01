using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.IO.Ports;

using LiveViewLib.MessageTypes;
using InTheHand.Net.Sockets;
using InTheHand.Net;

namespace LiveViewLib
{
    public class LiveView
    {
        public delegate void DeviceStatusListener(LiveView liveView, DeviceStatusMessage message);
        public delegate void NavigationListener(LiveView liveView, NavigationMessage message);
        public delegate void AlertListener(LiveView liveView, GetAlertMessage message);

        protected BluetoothClient client;
        protected BluetoothAddress address;
        protected Thread listenThread;

        public byte MenuVibrationTime = 5;
        public bool Use24HourClock = true;
        public GetCapsResponse Capabilities;
        protected MenuItem[] MenuItems = new MenuItem[0];

        public event DeviceStatusListener deviceStatusListener;
        public event NavigationListener navigationListener;
        public event AlertListener alertListener;

        public LiveView(BluetoothClient client)
        {
            this.client = client;
            this.address = client.RemoteEndPoint.Address;
        }

        public void Start()
        {
            this.listenThread = new Thread(new ThreadStart(OnConnected));
            this.listenThread.IsBackground = true;
            this.listenThread.Start();
        }

        public void Reconnect(BluetoothClient client)
        {
            if (!client.RemoteEndPoint.Address.Equals(GetAddress()))
                return;

            if (listenThread.IsAlive)
                listenThread.Abort();

            this.client = client;

            this.listenThread = new Thread(new ThreadStart(OnConnected));
            this.listenThread.IsBackground = true;
            this.listenThread.Start();
        }

        protected void OnConnected()
        {
            this.Send(new GetCapsMessage());
            while (this.client.Connected)
            {
                this.Listen();
            }
        }

        public void Send(LiveViewMessage message)
        {
            Console.WriteLine("--> Sending message:\n" + message.ToString() + "\n");
            this.client.GetStream().Write(message.ToByteArray(), 0, message.ToByteArray().Length);
        }

        public BluetoothAddress GetAddress()
        {
            return address;
        }

        public void SetMenuItems(MenuItem[] items)
        {
            this.MenuItems = items;
        }

        private long GetLocalTime()
        {
            TimeSpan span = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (long) Math.Round(span.TotalSeconds);
        }

        protected void Listen()
        {
            if (client.Available == 0)
                return;

            byte[] data = new byte[client.Available];
            this.client.GetStream().Read(data, 0, data.Length);

            LiveViewMessage[] messages = LiveViewMessage.Decode(data);

            foreach (LiveViewMessage message in messages)
            {
                Console.WriteLine("<-- Recieved message:\n" + message.ToString() + "\n");
                Send(new AckMessage(message.messageId));

                switch (message.messageId)
                {
                    case LiveViewMessage.MSG_GETCAPS_RESP:
                        Capabilities = message as GetCapsResponse;
                        Send(new SetMenuSizeMessage((byte) MenuItems.Length));
                        Send(new SetMenuSettingsMessage(MenuVibrationTime, 0));
                        break;
                    case LiveViewMessage.MSG_GETMENUITEMS:
                        byte i = 0;
                        foreach (MenuItem menuItem in this.MenuItems)
                        {
                            Send(new GetMenuItemResponse(i, menuItem.isAlert, menuItem.unreadCount, menuItem.text, menuItem.bitmap));
                            i++;
                        }
                        break;
                    case LiveViewMessage.MSG_GETMENUITEM:
                        byte index = ((GetMenuItemMessage)(message)).index;
                        MenuItem item = MenuItems.ElementAt(index);
                        this.Send(new GetMenuItemResponse(index, item.isAlert, item.unreadCount, item.text, item.bitmap));
                        break;
                    case LiveViewMessage.MSG_GETTIME:
                        Send(new GetTimeResponse(GetLocalTime(), Use24HourClock));
                        break;
                    case LiveViewMessage.MSG_DEVICESTATUS:
                        Send(new DeviceStatusAck());
                        if(deviceStatusListener != null) deviceStatusListener(this, message as DeviceStatusMessage);
                        break;
                    case LiveViewMessage.MSG_NAVIGATION:
                        if(navigationListener != null) navigationListener(this, message as NavigationMessage);
                        break;
                    case LiveViewMessage.MSG_GETALERT:
                        if(alertListener != null) alertListener(this, message as GetAlertMessage);
                        break;
                    case LiveViewMessage.MSG_SETVIBRATE_ACK:
                        break; 
                    case LiveViewMessage.MSG_SETLED_ACK:
                        break;
                    case LiveViewMessage.MSG_DISPLAYPANEL_ACK:
                        break;
                    case LiveViewMessage.MSG_SETSTATUSBAR_ACK:
                        break;

                    case LiveViewMessage.MSG_GETSCREENMODE_RESP:
                        break;
                    case LiveViewMessage.MSG_SETSCREENMODE_ACK:
                        break;
                    case LiveViewMessage.MSG_DISPLAYBITMAP_ACK:
                        break;
                    case LiveViewMessage.MSG_CLEARDISPLAY_ACK:
                        break;

                    case LiveViewMessage.MSG_DISPLAYTEXT_ACK:
                        break;
                    default:
                        throw new Exception("Unknown message: " + message.messageId);
                }
            }
        }
    }
}
