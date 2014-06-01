using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveViewLib.MessageTypes
{
    public class ClearDisplayMessage : LiveViewMessage
    {
        public ClearDisplayMessage()
        {
            messageId = LiveViewMessage.MSG_CLEARDISPLAY;
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            return AddHeader(payload.ToArray());
        }
    }

    public class ClearDisplayAck : LiveViewMessage
    {
        public byte unknown;

        public ClearDisplayAck(byte[] payload)
        {
            messageId = LiveViewMessage.MSG_CLEARDISPLAY_ACK;
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
