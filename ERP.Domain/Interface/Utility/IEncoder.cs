namespace ERP.Domain.Interface.Utility
{
    public interface IEncoder
    {
        string EncodeToMd5(string text);
        bool CompareMd5Text(string md5Text, string secondParam);
    }
}
