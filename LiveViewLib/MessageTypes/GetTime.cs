using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveViewLib.MessageTypes
{
    public class GetTimeMessage : LiveViewMessage
    {
        public byte unknown;

        public GetTimeMessage(byte[] payload)
        {
            messageId = LiveViewMessage.MSG_GETTIME;
            unknown = payload[0];
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.Add(unknown);

            return AddHeader(payload.ToArray());
        }
    }

    public class GetTimeResponse : LiveViewMessage
    {
        public long time;
        public bool use24h;

        public GetTimeResponse(long time, bool use24h)
        {
            messageId = LiveViewMessage.MSG_GETTIME_RESP;
            this.time = time;
            this.use24h = use24h;
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.AddRange(LongToBytes(time));
            payload.Add((byte)(use24h ? 0x00 : 0x01));

            return AddHeader(payload.ToArray());
        }
    }
}
