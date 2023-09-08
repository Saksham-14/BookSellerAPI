using System.Data.SqlClient;

namespace DAL.Models
{
    public static class AppConfig
    {
        public static SqlConnection? mConnection { get; set; }
    }
}
