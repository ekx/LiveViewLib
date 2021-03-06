﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveViewLib.MessageTypes
{
    /// <summary>
    /// This message is used to transmit menu setting to the LiveView.
    /// </summary>
    public class SetMenuSettingsMessage : LiveViewMessage
    {
        public byte vibrationTime;
        public byte initialMenuItemId;

        public SetMenuSettingsMessage(byte vibrationTime, byte initialMenuItemId)
        {
            messageId = LiveViewMessage.MSG_SETMENUSETTINGS;
            this.vibrationTime = vibrationTime;
            this.initialMenuItemId = initialMenuItemId;
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.Add(vibrationTime);
            payload.Add(12);
            payload.Add(initialMenuItemId);

            return AddHeader(payload.ToArray());
        }
    }

    /// <summary>
    /// Specific acknowledgement message. Response to SetMenuSettingsMessage.
    /// </summary>
    public class SetMenuSettingsAck : LiveViewMessage
    {
        public byte unknown;

        public SetMenuSettingsAck(byte[] payload)
        {
            messageId = LiveViewMessage.MSG_SETMENUSETTINGS_ACK;
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
