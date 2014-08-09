using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveViewLib.MessageTypes
{
    /// <summary>
    /// This message is used by the LiveView to request the current time.
    /// </summary>
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

    /// <summary>
    /// The response to GetTimeMessage. Contains the current time.
    /// </summary>
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

        public static long GetLocalTime()
        {
            TimeSpan span = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (long)Math.Round(span.TotalSeconds);
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
