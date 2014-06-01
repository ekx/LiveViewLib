using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LiveViewLib;
using LiveViewLib.MessageTypes;
using System.IO;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            LiveViewServer server = new LiveViewServer();
            server.connectListener += OnConnect;
            server.Start();

            while (true)
            {
                string command = Console.ReadLine();

                switch (command)
                {
                    case "vibrate":
                        server.SendAll(new SetVibrateMessage(0, 500));
                        break;
                    case "led":
                        server.SendAll(new SetLEDMessage(0, 0, 255, 0, 500));
                        break;
                    case "panel":
                        server.SendAll(new DisplayPanelMessage("Test", "Footer", GetPreview(), false));
                        break;
                    case "statusbar":
                        server.SendAll(new SetStatusbarMessage(1, 100, GetPreview()));
                        break;

                    case "getscreenmode":
                        server.SendAll(new GetScreenmodeMessage());
                        break;
                    case "setscreenmode":
                        server.SendAll(new SetScreenmodeMessage(0, Screenmode.BRIGHTNESS_MAX));
                        break;
                    case "bitmap":
                        server.SendAll(new DisplayBitmapMessage(128, 128, GetImage()));
                        break;
                    case "cleardisplay":
                        server.SendAll(new ClearDisplayMessage());
                        break;

                    case "text":
                        server.SendAll(new DisplayTextMessage("Text test"));
                        break;
                }
            }
        }

        public static void OnConnect(LiveView liveView)
        {
            liveView.SetMenuItems(new LiveViewLib.MenuItem[] { 
                new LiveViewLib.MenuItem(true, 30, "Menu0", GetPreview()),
                new LiveViewLib.MenuItem(true, 52, "Menu1", GetPreview())
            });

            liveView.deviceStatusListener += OnDeviceStatus;
            liveView.navigationListener += OnNavigation;
            liveView.alertListener += OnAlert;
        }

        public static void OnDeviceStatus(LiveView liveView, DeviceStatusMessage message)
        {
        }

        public static void OnNavigation(LiveView liveView, NavigationMessage message)
        {
            liveView.Send(new NavigationResponse(LiveViewMessage.RESULT_EXIT));
        }

        public static void OnAlert(LiveView liveView, GetAlertMessage message)
        {
            liveView.Send(new GetAlertResponse(20, 10, 1, "tst", "ht", "bt", File.ReadAllBytes("test36.png")));
        }

        public static byte[] GetPreview()
        {
            return File.ReadAllBytes("test36.png");
        }

        public static byte[] GetImage()
        {
            return File.ReadAllBytes("test128.png");
        }
    }
}
