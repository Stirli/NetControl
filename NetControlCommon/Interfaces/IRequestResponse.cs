namespace NetControlCommon.Interfaces
{
    public interface IRequestResponse
    {
        byte[] GetBytes();
        string ContentType { get; }
    }
}
