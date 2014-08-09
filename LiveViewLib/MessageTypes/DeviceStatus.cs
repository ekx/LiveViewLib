using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveViewLib.MessageTypes
{
    /// <summary>
    /// Helper class containing constants pertaining to the DeviceStatus messages.
    /// </summary>
    public class DeviceStatus
    {
        public const byte OFF = 0;
        public const byte ON = 1;
        public const byte MENU = 2;
    }

    /// <summary>
    /// This message is used by the LiveView to tell the server what state it is in.
    /// </summary>
    public class DeviceStatusMessage : LiveViewMessage
    {
        public byte deviceStatus;

        public DeviceStatusMessage(byte[] payload)
        {
            messageId = LiveViewMessage.MSG_DEVICESTATUS;
            deviceStatus = payload[0];
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.Add(deviceStatus);

            return AddHeader(payload.ToArray());
        }
    }

    /// <summary>
    /// Specific acknowledgement message. Response to DeviceStatusMessage.
    /// </summary>
    public class DeviceStatusAck : LiveViewMessage
    {
        public DeviceStatusAck()
        {
            messageId = LiveViewMessage.MSG_DEVICESTATUS_ACK;
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.Add(LiveViewMessage.RESULT_OK);

            return AddHeader(payload.ToArray());
        }
    }
}
