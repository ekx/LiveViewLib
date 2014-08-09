using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveViewLib.MessageTypes
{
    /// <summary>
    /// Helper class containing constants pertaining to the Navigation messages.
    /// </summary>
    public class Navigation
    {
        public const byte ACTION_PRESS = 0;
        public const byte ACTION_LONGPRESS = 1;
        public const byte ACTION_DOUBLEPRESS = 2;

        public const byte TYPE_UP = 0;
        public const byte TYPE_DOWN = 1;
        public const byte TYPE_LEFT = 2;
        public const byte TYPE_RIGHT = 3;
        public const byte TYPE_SELECT = 4;
        public const byte TYPE_MENUSELECT = 5;
    }

    /// <summary>
    /// This message is used by the LiveView to inform the server of user inputs.
    /// </summary>
    public class NavigationMessage : LiveViewMessage
    {
        public byte navAction;
        public byte navType;
        public byte menuItemId;
        public byte menuId;

        public NavigationMessage(byte[] payload)
        {
            messageId = LiveViewMessage.MSG_NAVIGATION;
            
            navAction = (byte) ((payload[2] - 1) % 3);
            navType = (byte) ((payload[2] - 1) / 3);
            menuItemId = payload[3];
            menuId = payload[4];
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.Add(0);
            payload.Add(3);
            payload.Add((byte) ((navType * 3) + navAction + 1));
            payload.Add(menuItemId);
            payload.Add(menuId);

            return AddHeader(payload.ToArray());
        }
    }

    /// <summary>
    /// This is the response to NavigationMessage. Contains instructions on how to handle the input.
    /// </summary>
    public class NavigationResponse : LiveViewMessage
    {
        public byte code;

        public NavigationResponse(byte code)
        {
            messageId = LiveViewMessage.MSG_NAVIGATION_RESP;
            this.code = code;
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.Add(code);

            return AddHeader(payload.ToArray());
        }
    }
}
