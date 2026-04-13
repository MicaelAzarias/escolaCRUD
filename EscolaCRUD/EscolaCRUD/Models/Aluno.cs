namespace EscolaCRUD.Models;

public class Aluno
{
    public int    Id    { get; set; }
    public string Nome  { get; set; } = string.Empty;
    public int    Idade { get; set; }

    public override string ToString() => $"[{Id}] {Nome} — {Idade} anos";
}
