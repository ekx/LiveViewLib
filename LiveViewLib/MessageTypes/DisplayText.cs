﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveViewLib.MessageTypes
{
    public class DisplayTextMessage : LiveViewMessage
    {
        public string text;

        public DisplayTextMessage(string text)
        {
            messageId = LiveViewMessage.MSG_DISPLAYTEXT;
            this.text = text;
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.Add(0);
            payload.AddRange(System.Text.Encoding.ASCII.GetBytes(text));

            return AddHeader(payload.ToArray());
        }
    }

    public class DisplayTextAck : LiveViewMessage
    {
        public byte unknown;

        public DisplayTextAck(byte[] payload)
        {
            messageId = LiveViewMessage.MSG_DISPLAYTEXT_ACK;
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
