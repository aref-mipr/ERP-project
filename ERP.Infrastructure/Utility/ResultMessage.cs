using ERP.Domain.Interface.Utility;

namespace ERP.Infrastructure.Utility
{
    public class ResultMessage: IResultMessage
    {
        public string Message { get; set; }

        #region success

        public string Success()
        {
            Message = "عملیات با موفقیت انجام شد.";
            return Message;
        }

        public string Success(string message)
        {
            Message = message;
            return Message;
        }

        #endregion

        #region not found

        public string NotFound()
        {
            Message = "صفحه موردنظر یافت نشد.";
            return Message;
        }

        public string NotFound(string message)
        {
            Message = message;
            return Message;
        }

        #endregion

        #region error

        public string Error()
        {
            Message = "درخواست شما با خطا مواجه شد.";
            return Message;
        }
        public string Error(string message)
        {
            Message = message;
            return Message;
        }
        #endregion
    }
}
