using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveViewLib.MessageTypes
{
    public class SetStatusbarMessage : LiveViewMessage
    {
        public byte menuItemId;
        public ushort unreadCount;
        public byte[] image;

        public SetStatusbarMessage(byte menuItemId, ushort unreadCount, byte[] image)
        {
            messageId = LiveViewMessage.MSG_SETSTATUSBAR;
            this.menuItemId = menuItemId;
            this.unreadCount = unreadCount;
            this.image = image;
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.AddRange(new byte[] { 0, 0, 0 });
            payload.AddRange(ShortToBytes(unreadCount));
            payload.AddRange(new byte[] { 0, 0 });
            payload.Add((byte)(menuItemId + 3));
            payload.AddRange(new byte[] { 0, 0, 0, 0, 0, 0, 0 });
            payload.AddRange(image);

            return AddHeader(payload.ToArray());
        }
    }

    public class SetStatusbarAck : LiveViewMessage
    {
        public byte unknown;

        public SetStatusbarAck(byte[] payload)
        {
            messageId = LiveViewMessage.MSG_SETSTATUSBAR_ACK;
            unknown = payload[0];
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.Add(unknown);
            return AddHeader(payload.ToArray());
        }
    }
}
