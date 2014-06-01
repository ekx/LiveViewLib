using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveViewLib.MessageTypes
{
    public class AckMessage : LiveViewMessage
    {
        byte ackId;

        public AckMessage(byte ackId)
        {
            messageId = LiveViewMessage.MSG_ACK;
            this.ackId = ackId;
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.Add(ackId);

            return AddHeader(payload.ToArray());
        }
    }
}
