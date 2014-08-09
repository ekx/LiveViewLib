using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveViewLib.MessageTypes
{
    /// <summary>
    /// This message is used by the LiveView to request all the MenuItems it should display.
    /// </summary>
    public class GetMenuItemsMessage : LiveViewMessage
    {
        public byte unknown;

        public GetMenuItemsMessage(byte[] payload)
        {
            messageId = LiveViewMessage.MSG_GETMENUITEMS;
            unknown = payload[0];
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.Add(unknown);

            return AddHeader(payload.ToArray());
        }
    }

    /// <summary>
    /// This message is used by the LiveView to request a specific MenuItem it should display.
    /// </summary>
    public class GetMenuItemMessage : LiveViewMessage
    {
        public byte index;

        public GetMenuItemMessage(byte[] payload)
        {
            messageId = LiveViewMessage.MSG_GETMENUITEM;
            index = payload[0];
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.Add(index);

            return AddHeader(payload.ToArray());
        }
    }

    /// <summary>
    /// The response to GetMenuItemsMessage and GetMenuItemMessage. Contains the data the LiveView needs to display a single MenuItem.
    /// </summary>
    public class GetMenuItemResponse : LiveViewMessage
    {
        public byte index;
        public bool isAlert;
        public ushort unreadCount;
        public string text;
        public byte[] bitmap;

        public GetMenuItemResponse(byte index, bool isAlert, ushort unreadCount, string text, byte[] bitmap)
        {
            messageId = LiveViewMessage.MSG_GETMENUITEM_RESP;
            this.index = index;
            this.isAlert = isAlert;
            this.unreadCount = unreadCount;
            this.text = text;
            this.bitmap = bitmap;
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.Add((byte) (isAlert ? 0 : 1));
            payload.AddRange(new byte[] { 0, 0 });
            payload.AddRange(ShortToBytes(unreadCount));
            payload.AddRange(new byte[] { 0, 0 });
            payload.Add((byte) (index + 3));
            payload.AddRange(new byte[] { 0, 0, 0, 0, 0 });
            payload.AddRange(ShortToBytes((ushort) text.Length));
            payload.AddRange(System.Text.Encoding.ASCII.GetBytes(text));
            payload.AddRange(bitmap);

            return AddHeader(payload.ToArray());
        }
    }
}
