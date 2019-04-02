using MySql.Data.MySqlClient;

namespace CloneDB.DAL
{
    public interface IDbFactory
    {
        MySqlConnection GetConnection();
    }
}