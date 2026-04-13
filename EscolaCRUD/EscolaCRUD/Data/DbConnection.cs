using MySql.Data.MySqlClient;

namespace EscolaCRUD.Data;

public static class DbConnection
{
    private const string Server   = "localhost";
    private const string Port     = "3306";
    private const string Database = "Escola";
    private const string User     = "root";
    private const string Password = "@F1lh0t2s";        


    private static readonly string _connectionString =
        $"Server={Server};Port={Port};Database={Database};" +
        $"Uid={User};Pwd={Password};CharSet=utf8mb4;";


    public static MySqlConnection GetConnection()
    {
        var conn = new MySqlConnection(_connectionString);
        conn.Open();
        return conn;
    }
}
