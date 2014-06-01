using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveViewLib.MessageTypes
{
    public class GetCapsMessage : LiveViewMessage
    {
        public GetCapsMessage()
        {
            messageId = LiveViewMessage.MSG_GETCAPS;
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.Add((byte) LiveViewServer.CLIENT_SOFTWARE_VERSION.Length);
            payload.AddRange(System.Text.Encoding.ASCII.GetBytes(LiveViewServer.CLIENT_SOFTWARE_VERSION));

            return AddHeader(payload.ToArray());
        }
    }

    public class GetCapsResponse : LiveViewMessage
    {
        public byte width;
        public byte height;
        public byte statusBarWidth;
        public byte statusBarHeight;
        public byte viewWidth;
        public byte viewHeight;
        public byte announceWidth;
        public byte announceHeight;
        public byte textChunkSize;
        public byte idleTimer;
        public string softwareVersion;

        public GetCapsResponse(byte[] data)
        {
            messageId = LiveViewMessage.MSG_GETCAPS_RESP;
            this.width = data[0];
            this.height = data[1];
            this.statusBarWidth = data[2];
            this.statusBarHeight = data[3];
            this.viewWidth = data[4];
            this.viewHeight = data[5];
            this.announceWidth = data[6];
            this.announceHeight = data[7];
            this.textChunkSize = data[8];
            this.idleTimer = data[9];
            byte[] version = new byte[data.Length - 10];
            Array.Copy(data, 10, version, 0, version.Length);
            this.softwareVersion = System.Text.Encoding.ASCII.GetString(version);
        }

        override public byte[] ToByteArray()
        {
            List<byte> payload = new List<byte>();
            payload.Add(width);
            payload.Add(height);
            payload.Add(statusBarWidth);
            payload.Add(statusBarHeight);
            payload.Add(viewWidth);
            payload.Add(viewHeight);
            payload.Add(announceWidth);
            payload.Add(announceHeight);
            payload.Add(textChunkSize);
            payload.Add(idleTimer);
            payload.AddRange(System.Text.Encoding.ASCII.GetBytes(this.softwareVersion));

            return AddHeader(payload.ToArray());
        }
    }
}
