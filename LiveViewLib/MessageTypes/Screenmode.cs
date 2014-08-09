using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveViewLib.MessageTypes
{
    /// <summary>
    /// Helper class containing constants pertaining to the GetScreenmode messages.
    /// </summary>
    public class Screenmode
    {
        public const byte BRIGHTNESS_OFF = 48;
        public const byte BRIGHTNESS_DIM = 49;
        public const byte BRIGHTNESS_MAX = 50;
    }

    /// <summary>
    /// This message is used to request the LiveView screenmode. Only works if menu size is 0.
    /// </summary>
    public class GetScreenmodeMessage : LiveViewMessage
    {
        public GetScreenmodeMessage()
        {
            messageId = LiveViewMessage.MSG_GETSCREENMODE;
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            return AddHeader(payload.ToArray());
        }
    }

    /// <summary>
    /// The response to GetScreenmodeMessage. Contains the LiveViews screenmode.
    /// </summary>
    public class GetScreenmodeResponse : LiveViewMessage
    {
        public byte auto;
        public byte brightness;

        public GetScreenmodeResponse(byte[] payload)
        {
            messageId = LiveViewMessage.MSG_GETSCREENMODE_RESP;
            auto = (byte) (payload[0] & 1);
            brightness = (byte) (payload[0] >> 1);
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.Add((byte) ((brightness << 1) | 1));
            return AddHeader(payload.ToArray());
        }
    }

    /// <summary>
    /// This message is used to set the LiveView screenmode. Only works if menu size is 0.
    /// </summary>
    public class SetScreenmodeMessage : LiveViewMessage
    {
        public byte auto;
        public byte brightness;

        public SetScreenmodeMessage(byte auto, byte brightness)
        {
            messageId = LiveViewMessage.MSG_SETSCREENMODE;
            this.auto = auto;
            this.brightness = brightness;
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.Add((byte)((brightness << 1) | 1));
            return AddHeader(payload.ToArray());
        }
    }

    /// <summary>
    /// Specific acknowledgement message. Response to SetScreenmodeMessage.
    /// </summary>
    public class SetScreenmodeAck : LiveViewMessage
    {
        public byte unknown;

        public SetScreenmodeAck(byte[] payload)
        {
            messageId = LiveViewMessage.MSG_SETSCREENMODE_ACK;
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
