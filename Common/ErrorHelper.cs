namespace IMS.Common
{
    public class ErrorHelper
    {
        public string Error { get; set; } = "";
        public string Success { get; set; } = "";

        public void ClearError()
        {
            Error = "";
        }

        public void ClearSuccess()
        {
            Success = "";
        }
    }
}

