namespace DAL.Common
{
    public class CommonMethod
    {
        public static string GetErrorMessage(Exception ex)
        {
            return (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
        }
    }
}
