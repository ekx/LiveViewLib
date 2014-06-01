using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveViewLib.MessageTypes
{
    public class SetLEDMessage : LiveViewMessage
    {
        public byte red;
        public byte green;
        public byte blue;
        public ushort delay;
        public ushort duration;

        public SetLEDMessage(byte red, byte green, byte blue, ushort delay, ushort duration)
        {
            messageId = LiveViewMessage.MSG_SETLED;
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.delay = delay;
            this.duration = duration;
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.AddRange(ShortToBytes(ToRGB565(red, green, blue)));
            payload.AddRange(ShortToBytes(delay));
            payload.AddRange(ShortToBytes(duration));

            return AddHeader(payload.ToArray());
        }
    }

    public class SetLEDAck : LiveViewMessage
    {
        public byte unknown;

        public SetLEDAck(byte[] payload)
        {
            messageId = LiveViewMessage.MSG_SETLED_ACK;
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
