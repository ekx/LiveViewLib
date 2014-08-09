using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveViewLib.MessageTypes
{
    /// <summary>
    /// This message is used to display images on the LiveView. Only works if menu size is 0.
    /// </summary>
    public class DisplayBitmapMessage : LiveViewMessage
    {
        public byte width;
        public byte height;
        public byte[] image;

        public DisplayBitmapMessage(byte width, byte height, byte[] image)
        {
            messageId = LiveViewMessage.MSG_DISPLAYBITMAP;
            this.width = width;
            this.height = height;
            this.image = image;
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.Add(width);
            payload.Add(height);
            payload.Add(1);
            payload.AddRange(image);

            return AddHeader(payload.ToArray());
        }
    }

    /// <summary>
    /// Specific acknowledgement message. Response to DisplayBitmapMessage.
    /// </summary>
    public class DisplayBitmapAck : LiveViewMessage
    {
        public byte unknown;

        public DisplayBitmapAck(byte[] payload)
        {
            messageId = LiveViewMessage.MSG_DISPLAYBITMAP_ACK;
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
