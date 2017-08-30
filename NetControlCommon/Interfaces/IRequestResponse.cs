namespace NetControlCommon.Interfaces
{
    public interface IRequestResponse
    {
        string ContentType { get; }
        byte[] GetBytes();
    }
}