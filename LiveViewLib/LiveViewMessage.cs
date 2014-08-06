using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiveViewLib.MessageTypes;

namespace LiveViewLib
{
    /// <summary>
    /// Base class containing attributes common to all LiveViewMessages.
    /// </summary>
    abstract public class LiveViewMessage
    {
        public const byte RESULT_OK = 0;
        public const byte RESULT_ERROR = 1;
        public const byte RESULT_OOM = 2;
        public const byte RESULT_EXIT = 3;
        public const byte RESULT_CANCEL = 4;

        public const byte MSG_GETCAPS = 1;
        public const byte MSG_GETCAPS_RESP = 2;

        public const byte MSG_DISPLAYTEXT = 3;
        public const byte MSG_DISPLAYTEXT_ACK = 4;

        public const byte MSG_DISPLAYPANEL = 5;
        public const byte MSG_DISPLAYPANEL_ACK = 6;

        public const byte MSG_DEVICESTATUS = 7;
        public const byte MSG_DEVICESTATUS_ACK = 8;

        public const byte MSG_DISPLAYBITMAP = 19;
        public const byte MSG_DISPLAYBITMAP_ACK = 20;

        public const byte MSG_CLEARDISPLAY = 21;
        public const byte MSG_CLEARDISPLAY_ACK = 22;

        public const byte MSG_SETMENUSIZE = 23;
        public const byte MSG_SETMENUSIZE_ACK = 24;

        public const byte MSG_GETMENUITEM = 25;
        public const byte MSG_GETMENUITEM_RESP = 26;

        public const byte MSG_GETALERT = 27;
        public const byte MSG_GETALERT_RESP = 28;

        public const byte MSG_NAVIGATION = 29;
        public const byte MSG_NAVIGATION_RESP = 30;

        public const byte MSG_SETSTATUSBAR = 33;
        public const byte MSG_SETSTATUSBAR_ACK = 34;

        public const byte MSG_GETMENUITEMS = 35;

        public const byte MSG_SETMENUSETTINGS = 36;
        public const byte MSG_SETMENUSETTINGS_ACK = 37;

        public const byte MSG_GETTIME = 38;
        public const byte MSG_GETTIME_RESP = 39;

        public const byte MSG_SETLED = 40;
        public const byte MSG_SETLED_ACK = 41;

        public const byte MSG_SETVIBRATE = 42;
        public const byte MSG_SETVIBRATE_ACK = 43;

        public const byte MSG_ACK = 44;

        public const byte MSG_SETSCREENMODE = 64;
        public const byte MSG_SETSCREENMODE_ACK = 65;

        public const byte MSG_GETSCREENMODE = 66;
        public const byte MSG_GETSCREENMODE_RESP = 67;

        public byte messageId;

        abstract public byte[] ToByteArray();
        
        /// <summary>
        /// Helper function that adds the common message header to a byte array.
        /// </summary>
        /// <param name="data">Data which to prepend with header.</param>
        /// <returns>Modified data with header.</returns>
        protected byte[] AddHeader(byte[] data)
        {
            List<byte> output = new List<byte>();
            output.Add((byte) messageId);
            output.Add((byte) 4);
            output.AddRange(LongToBytes((long) data.Length));
            output.AddRange(data);
            return output.ToArray();
        }

        /// <summary>
        /// Helper function that converts a ushort to 2 bytes.
        /// </summary>
        /// <param name="number">ushort to convert.</param>
        /// <returns>Resulting bytes.</returns>
        protected static byte[] ShortToBytes(ushort number)
        {
            byte[] result = BitConverter.GetBytes(number);
            Array.Reverse(result);
            return result;
        }

        /// <summary>
        /// Helper function that converts 2 bytes to ushort.
        /// </summary>
        /// <param name="a">High byte.</param>
        /// <param name="b">Low byte.</param>
        /// <returns>Resulting ushort.</returns>
        protected static ushort BytesToShort(byte a, byte b)
        {
            return BitConverter.ToUInt16(new byte[] {b, a}, 0);
        }

        /// <summary>
        /// Helper function that converts a long to 4 bytes.
        /// </summary>
        /// <param name="number">long to convert.</param>
        /// <returns>Resulting bytes.</returns>
        protected static byte[] LongToBytes(long number)
        {
            byte[] result = BitConverter.GetBytes(UInt32.Parse(number.ToString()));
            Array.Reverse(result);
            return result;
        }

        /// <summary>
        /// Helper function that converts RGB888 to RGB565.
        /// </summary>
        /// <param name="red">Red value.</param>
        /// <param name="green">Green value.</param>
        /// <param name="blue">Blue value.</param>
        /// <returns>RGB565 result.</returns>
        protected static ushort ToRGB565(byte red, byte green, byte blue)
        {
            int rgb32 = (blue << 0) | (green << 8) | (red << 16);
            return (ushort)((rgb32 >> 8 & 0xf800) | (rgb32 >> 5 & 0x07e0) | (rgb32 >> 3 & 0x001f));
        }

        /// <summary>
        /// Method that decodes data recieved from a LiveView and constructs the appropriate LiveViewMessage.
        /// </summary>
        /// <param name="data">Recieved data.</param>
        /// <returns>Constructed LiveViewMessage.</returns>
        public static LiveViewMessage[] Decode(byte[] data)
        {
            List<LiveViewMessage> messages = new List<LiveViewMessage>();

            int index = 0;
            while (index < data.Length)
            {
                byte messageId;
                byte[] payload;

                index = SplitMessages(data, index, out messageId, out payload);

                switch (messageId)
                {
                    case LiveViewMessage.MSG_GETCAPS_RESP:
                        messages.Add(new GetCapsResponse(payload));
                        break;
                    case LiveViewMessage.MSG_GETMENUITEMS:
                        messages.Add(new GetMenuItemsMessage(payload));
                        break;
                    case LiveViewMessage.MSG_GETMENUITEM:
                        messages.Add(new GetMenuItemMessage(payload));
                        break;
                    case LiveViewMessage.MSG_GETTIME:
                        messages.Add(new GetTimeMessage(payload));
                        break;
                    case LiveViewMessage.MSG_DEVICESTATUS:
                        messages.Add(new DeviceStatusMessage(payload));
                        break;
                    case LiveViewMessage.MSG_NAVIGATION:
                        messages.Add(new NavigationMessage(payload));
                        break;
                    case LiveViewMessage.MSG_GETALERT:
                        messages.Add(new GetAlertMessage(payload));
                        break;
                    case LiveViewMessage.MSG_SETVIBRATE_ACK:
                        messages.Add(new SetVibrateAck(payload));
                        break;
                    case LiveViewMessage.MSG_SETLED_ACK:
                        messages.Add(new SetLEDAck(payload));
                        break;
                    case LiveViewMessage.MSG_DISPLAYPANEL_ACK:
                        messages.Add(new DisplayPanelAck(payload));
                        break;
                    case LiveViewMessage.MSG_SETSTATUSBAR_ACK:
                        messages.Add(new SetStatusbarAck(payload));
                        break;

                    case LiveViewMessage.MSG_GETSCREENMODE_RESP:
                        messages.Add(new GetScreenmodeResponse(payload));
                        break;
                    case LiveViewMessage.MSG_SETSCREENMODE_ACK:
                        messages.Add(new SetScreenmodeAck(payload));
                        break;
                    case LiveViewMessage.MSG_DISPLAYBITMAP_ACK:
                        messages.Add(new DisplayBitmapAck(payload));
                        break;
                    case LiveViewMessage.MSG_CLEARDISPLAY_ACK:
                        messages.Add(new ClearDisplayAck(payload));
                        break;

                    case LiveViewMessage.MSG_DISPLAYTEXT_ACK:
                        messages.Add(new DisplayTextAck(payload));
                        break;
                    default:
                        throw new Exception("Unknown message: " + messageId);
                }
            }

            return messages.ToArray();
        }

        /// <summary>
        /// Method that splits recieved data into separate messages.
        /// </summary>
        /// <param name="data">Recieved data.</param>
        /// <param name="index">Current position in recieved data.</param>
        /// <param name="messageId">Message id of current message.</param>
        /// <param name="payload">Payload of current message.</param>
        /// <returns>Return new position in recieved data.</returns>
        private static int SplitMessages(byte[] data, int index, out byte messageId, out byte[] payload)
        {
            messageId = data[index + 0];
            int headerLength = data[index + 1];

            byte[] temp = new byte[4];
            Array.Copy(data, index + 2, temp, 0, 4);
            Array.Reverse(temp);
            int payloadLength = BitConverter.ToInt16(temp, 0);

            int messageLength = 2 + headerLength + payloadLength;

            payload = new byte[payloadLength];
            Array.Copy(data, (index + 2 + headerLength), payload, 0, payloadLength);

            if (headerLength != 4)
                throw new Exception(string.Format("Unexpected header length {0}", headerLength));

            return index + messageLength;
        }

        /// <summary>
        /// Shared toString method that all LiveViewMessages use.
        /// </summary>
        /// <returns>String describing the LiveViewMessage.</returns>
        override public string ToString()
        {
            return "// " + base.ToString() + "\n// " + BitConverter.ToString(ToByteArray());
        }
    }
}
