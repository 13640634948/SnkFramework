namespace SnkFramework.Connector
{
    public interface ISnkMessage
    {
        uint Command { get; }
        eSnkMessageType MsgType { get; }
        ushort Sequence { get; }
    }
}