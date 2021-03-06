﻿using System;
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
    /// <summary>
    /// Represents a single LiveView and manages its state.
    /// </summary>
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

        /// <summary>
        /// Called right after the LiveView connects to the computer.
        /// </summary>
        public void Start()
        {
            this.listenThread = new Thread(new ThreadStart(ConnectionLoop));
            this.listenThread.IsBackground = true;
            this.listenThread.Start();
        }

        /// <summary>
        /// Called when a LiveView connects that was already connect at one point.
        /// </summary>
        /// <param name="client">BluetoothClient of the new connection.</param>
        public void Reconnect(BluetoothClient client)
        {
            if (!client.RemoteEndPoint.Address.Equals(GetAddress()))
                return;

            if (listenThread.IsAlive)
                listenThread.Abort();

            this.client = client;

            this.listenThread = new Thread(new ThreadStart(ConnectionLoop));
            this.listenThread.IsBackground = true;
            this.listenThread.Start();
        }

        /// <summary>
        /// Loop that gets executed in a separate thread and continually checks for new data.
        /// </summary>
        protected void ConnectionLoop()
        {
            this.Send(new GetCapsMessage());
            while (this.client.Connected)
            {
                this.Listen();
            }
        }

        /// <summary>
        /// Method used to send messages to the LiveView.
        /// </summary>
        /// <param name="message">Message to send to the LiveView.</param>
        public void Send(LiveViewMessage message)
        {
            if (client.Connected)
            {
                Console.WriteLine("--> Sending message:\n" + message.ToString() + "\n");

                try
                {
                    this.client.GetStream().Write(message.ToByteArray(), 0, message.ToByteArray().Length);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }

        }

        /// <summary>
        /// Returns the BluetoothAddress associated with the LiveView.
        /// </summary>
        /// <returns>The requested BluetoothAddress.</returns>
        public BluetoothAddress GetAddress()
        {
            return address;
        }

        /// <summary>
        /// Sets the MenuItems that are displayed on the LiveView.
        /// </summary>
        /// <param name="items">MenuItems to display.</param>
        public void SetMenuItems(MenuItem[] items)
        {
            this.MenuItems = items;
        }

        /// <summary>
        /// Checks for new messages from the LiveView and responds accordingly.
        /// </summary>
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
                        Send(new GetTimeResponse(GetTimeResponse.GetLocalTime(), Use24HourClock));
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
                        //EVENT
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
