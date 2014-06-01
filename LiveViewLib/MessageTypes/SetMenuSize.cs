using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveViewLib.MessageTypes
{
    public class SetMenuSizeMessage : LiveViewMessage
    {
        public byte menuSize;

        public SetMenuSizeMessage(byte menuSize)
        {
            messageId = LiveViewMessage.MSG_SETMENUSIZE;
            this.menuSize = menuSize;
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.Add(menuSize);

            return AddHeader(payload.ToArray());
        }
    }

    public class SetMenuSizeAck : LiveViewMessage
    {
        public byte unknown;

        public SetMenuSizeAck(byte[] payload)
        {
            messageId = LiveViewMessage.MSG_SETMENUSIZE_ACK;
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
