using DAL.Models;
using System.Data.SqlClient;

namespace DAL.Database
{
    public class DbContext
    {
        public SqlConnection mConnection { get; set; }
        public DbContext()
        {
            mConnection = AppConfig.mConnection;
        }

        #region Common methods
        public void Open()
        {
            mConnection.Open();
        }
        public void Close()
        {
            mConnection.Close();
        }
        #endregion
    }
}