namespace SnkFramework.Connector
{
    public interface ISnkMessage
    {
        uint CommandID { get; set; }
        eSnkMessageType MsgType { get; set; }
        ushort Sequence { get; set; }
    }

    public class SnkMessage : ISnkMessage
    {
        public uint CommandID { get; set; }
        public eSnkMessageType MsgType { get; set; }
        public ushort Sequence { get; set; }
        public byte[] Content { get; set; }
        public short ContentType { get; set; }
    }


    public class SnkRequest : SnkMessage, ISnkRequest
    {
    }

    public class SnkResponse : SnkMessage, ISnkResponse
    {
        public int Status { get; set; }
    }

    public class SnkNotification : SnkMessage, ISnkNotification
    {
    }
}