using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveViewLib.MessageTypes
{
    /// <summary>
    /// This message is used to display panels on the LiveView. A panel contains a 36 by 36 image, a header text and a footer text.
    /// </summary>
    public class DisplayPanelMessage : LiveViewMessage
    {
        public string headerText;
        public string footerText;
        public byte[] image;
        public bool isAlert;

        public DisplayPanelMessage(string headerText, string footerText, byte[] image, bool isAlert)
        {
            messageId = LiveViewMessage.MSG_DISPLAYPANEL;
            this.headerText = headerText;
            this.footerText = footerText;
            this.image = image;
            this.isAlert = isAlert;
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.AddRange(new byte[] { 0, 0, 0, 0, 0, 0, 0 });
            payload.Add(isAlert ? (byte) 80 : (byte) 81);
            payload.Add(0);
            payload.AddRange(ShortToBytes((ushort) headerText.Length));
            payload.AddRange(System.Text.Encoding.ASCII.GetBytes(headerText));
            payload.AddRange(new byte[] { 0, 0 });
            payload.AddRange(ShortToBytes((ushort) footerText.Length));
            payload.AddRange(System.Text.Encoding.ASCII.GetBytes(footerText));
            payload.AddRange(image);

            return AddHeader(payload.ToArray());
        }
    }

    /// <summary>
    /// Specific acknowledgement message. Response to DisplayPanelMessage.
    /// </summary>
    public class DisplayPanelAck : LiveViewMessage
    {
        public byte unknown;

        public DisplayPanelAck(byte[] payload)
        {
            messageId = LiveViewMessage.MSG_DISPLAYPANEL_ACK;
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
