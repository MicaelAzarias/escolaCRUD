using MySql.Data.MySqlClient;
using EscolaCRUD.Models;

namespace EscolaCRUD.Data;


public class AlunoRepository
{
    //  INSERT 
    public void Inserir(Aluno aluno)
    {
        using var conn = DbConnection.GetConnection();
        const string sql = "INSERT INTO Alunos (Nome, Idade) VALUES (@nome, @idade)";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@nome",  aluno.Nome);
        cmd.Parameters.AddWithValue("@idade", aluno.Idade);
        cmd.ExecuteNonQuery();

        aluno.Id = (int)cmd.LastInsertedId;
    }

    //  SELECT ALL 
    public List<Aluno> ListarTodos()
    {
        var lista = new List<Aluno>();
        using var conn = DbConnection.GetConnection();
        const string sql = "SELECT Id, Nome, Idade FROM Alunos ORDER BY Id";

        using var cmd    = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Aluno
            {
                Id    = reader.GetInt32("Id"),
                Nome  = reader.GetString("Nome"),
                Idade = reader.GetInt32("Idade"),
            });
        }
        return lista;
    }

    //  SELECT BY ID 
    public Aluno? BuscarPorId(int id)
    {
        using var conn = DbConnection.GetConnection();
        const string sql = "SELECT Id, Nome, Idade FROM Alunos WHERE Id = @id";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        return new Aluno
        {
            Id    = reader.GetInt32("Id"),
            Nome  = reader.GetString("Nome"),
            Idade = reader.GetInt32("Idade"),
        };
    }

    // UPDATE
    public bool Atualizar(Aluno aluno)
    {
        using var conn = DbConnection.GetConnection();
        const string sql =
            "UPDATE Alunos SET Nome = @nome, Idade = @idade WHERE Id = @id";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@nome",  aluno.Nome);
        cmd.Parameters.AddWithValue("@idade", aluno.Idade);
        cmd.Parameters.AddWithValue("@id",    aluno.Id);

        return cmd.ExecuteNonQuery() > 0;
    }

    //  DELETE 
    public bool Remover(int id)
    {
        using var conn = DbConnection.GetConnection();
        const string sql = "DELETE FROM Alunos WHERE Id = @id";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);

        return cmd.ExecuteNonQuery() > 0;
    }
}
