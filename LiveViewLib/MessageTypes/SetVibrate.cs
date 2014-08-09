using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveViewLib.MessageTypes
{
    /// <summary>
    /// This message is used to activate the vibrate motor on the LiveView.
    /// </summary>
    public class SetVibrateMessage : LiveViewMessage
    {
        public ushort delay;
        public ushort duration;

        public SetVibrateMessage(ushort delay, ushort duration)
        {
            messageId = LiveViewMessage.MSG_SETVIBRATE;
            this.delay = delay;
            this.duration = duration;
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.AddRange(ShortToBytes(delay));
            payload.AddRange(ShortToBytes(duration));

            return AddHeader(payload.ToArray());
        }
    }

    /// <summary>
    /// Specific acknowledgement message. Response to SetVibrateMessage.
    /// </summary>
    public class SetVibrateAck : LiveViewMessage
    {
        public byte unknown;

        public SetVibrateAck(byte[] payload)
        {
            messageId = LiveViewMessage.MSG_SETVIBRATE_ACK;
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
