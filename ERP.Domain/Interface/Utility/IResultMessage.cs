namespace ERP.Domain.Interface.Utility
{
    public interface IResultMessage
    {
        string Success();
        string Success(string message);
        string NotFound();
        string NotFound(string message);
        string Error();
        string Error(string message);

    }
}
