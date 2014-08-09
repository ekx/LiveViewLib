using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveViewLib.MessageTypes
{
    /// <summary>
    /// Helper class containing constants pertaining to the GetAlert messages.
    /// </summary>
    public class Alert
    {
        public const byte ACTION_CURRENT = 0;
        public const byte ACTION_FIRST = 1;
        public const byte ACTION_LAST = 2;
        public const byte ACTION_NEXT = 3;
        public const byte ACTION_PREV = 4;
    }

    /// <summary>
    /// This message is sent by the LiveView if the user selected a MenuItem.
    /// </summary>
    public class GetAlertMessage : LiveViewMessage
    {
        public byte menuItemId;
        public byte action;
        public ushort maxBodySize;
        public byte unknownA;
        public byte unknownB;
        public byte unknownC;

        public GetAlertMessage(byte[] payload)
        {
            messageId = LiveViewMessage.MSG_GETALERT;
            menuItemId = payload[0];
            action = payload[1];
            maxBodySize = BytesToShort(payload[2], payload[3]);
            unknownA = payload[4];
            unknownB = payload[5];
            unknownC = payload[6];
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.Add(menuItemId);
            payload.Add(action);
            payload.AddRange(ShortToBytes(maxBodySize));
            payload.Add(unknownA);
            payload.Add(unknownB);
            payload.Add(unknownC);

            return AddHeader(payload.ToArray());
        }
    }

    /// <summary>
    /// The response to GetAlertMessage. Tells the LiveView what information to display in the alert.
    /// </summary>
    public class GetAlertResponse : LiveViewMessage
    {
        public ushort totalCount;
        public ushort unreadCount;
        public ushort alertIndex;
        public string timestampText;
        public string headerText;
        public string bodyText;
        public byte[] image;

        public GetAlertResponse(ushort totalCount, ushort unreadCount, ushort alertIndex, string timestampText, string headerText, string bodyText, byte[] image)
        {
            messageId = LiveViewMessage.MSG_GETALERT_RESP;
            this.totalCount = totalCount;
            this.unreadCount = unreadCount;
            this.alertIndex = alertIndex;
            this.timestampText = timestampText;
            this.headerText = headerText;
            this.bodyText = bodyText;
            this.image = image;
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.Add(0);
            payload.AddRange(ShortToBytes(totalCount));
            payload.AddRange(ShortToBytes(unreadCount));
            payload.AddRange(ShortToBytes(alertIndex));
            payload.Add(0);
            payload.Add(0);
            payload.AddRange(ShortToBytes((ushort) timestampText.Length));
            payload.AddRange(System.Text.Encoding.ASCII.GetBytes(timestampText));
            payload.AddRange(ShortToBytes((ushort) headerText.Length));
            payload.AddRange(System.Text.Encoding.ASCII.GetBytes(headerText));
            payload.AddRange(ShortToBytes((ushort) bodyText.Length));
            payload.AddRange(System.Text.Encoding.ASCII.GetBytes(bodyText));
            payload.Add(0);
            payload.AddRange(LongToBytes(image.Length));
            payload.AddRange(image);

            return AddHeader(payload.ToArray());
        }
    }
}
