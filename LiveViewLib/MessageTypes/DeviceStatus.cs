using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveViewLib.MessageTypes
{
    public class DeviceStatus
    {
        public const byte OFF = 0;
        public const byte ON = 1;
        public const byte MENU = 2;
    }

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
