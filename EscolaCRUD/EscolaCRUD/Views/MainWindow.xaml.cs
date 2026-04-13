using System.Windows;
using System.Windows.Media;
using EscolaCRUD.Data;
using EscolaCRUD.Models;

namespace EscolaCRUD.Views;

public partial class MainWindow : Window
{
    private readonly AlunoRepository _repo = new();

    public MainWindow()
    {
        InitializeComponent();
        CarregarAlunos();
    }


    private void CarregarAlunos()
    {
        try
        {
            var lista = _repo.ListarTodos();
            GridAlunos.ItemsSource = lista;
            TxtTotal.Text = lista.Count.ToString();
            SetStatus($"✅ {lista.Count} aluno(s) carregado(s).", Colors.SeaGreen);
        }
        catch (Exception ex)
        {
            SetStatus($"❌ Erro ao listar: {ex.Message}", Colors.Crimson);
        }
    }

    private void PreencherFormulario(Aluno a)
    {
        TxtId.Text    = a.Id.ToString();
        TxtNome.Text  = a.Nome;
        TxtIdade.Text = a.Idade.ToString();
    }

    private void LimparFormulario()
    {
        TxtId.Text    = string.Empty;
        TxtNome.Text  = string.Empty;
        TxtIdade.Text = string.Empty;
        TxtBuscaId.Text = string.Empty;
        GridAlunos.SelectedItem = null;
        TxtNome.Focus();
    }

    private bool ValidarEntrada(out string nome, out int idade)
    {
        nome  = TxtNome.Text.Trim();
        idade = 0;

        if (string.IsNullOrWhiteSpace(nome))
        {
            SetStatus("⚠️  O campo Nome não pode estar vazio.", Colors.DarkOrange);
            TxtNome.Focus();
            return false;
        }
        if (!int.TryParse(TxtIdade.Text.Trim(), out idade) || idade <= 0 || idade > 120)
        {
            SetStatus("⚠️  Informe uma idade válida (1–120).", Colors.DarkOrange);
            TxtIdade.Focus();
            return false;
        }
        return true;
    }

    private void SetStatus(string msg, Color color)
    {
        TxtStatus.Text       = msg;
        TxtStatus.Foreground = new SolidColorBrush(color);
    }



    private void BtnInserir_Click(object sender, RoutedEventArgs e)
    {
        if (!ValidarEntrada(out var nome, out var idade)) return;

        try
        {
            var aluno = new Aluno { Nome = nome, Idade = idade };
            _repo.Inserir(aluno);
            SetStatus($"✅ Aluno '{aluno.Nome}' inserido com ID {aluno.Id}.", Colors.SeaGreen);
            LimparFormulario();
            CarregarAlunos();
        }
        catch (Exception ex)
        {
            SetStatus($"❌ Erro ao inserir: {ex.Message}", Colors.Crimson);
        }
    }

    private void BtnListar_Click(object sender, RoutedEventArgs e) => CarregarAlunos();

    private void BtnAtualizar_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(TxtId.Text.Trim(), out var id) || id <= 0)
        {
            SetStatus("⚠️  Selecione um aluno na lista ou busque pelo ID antes de atualizar.",
                      Colors.DarkOrange);
            return;
        }
        if (!ValidarEntrada(out var nome, out var idade)) return;

        try
        {
            var aluno = new Aluno { Id = id, Nome = nome, Idade = idade };
            bool ok = _repo.Atualizar(aluno);

            if (ok)
            {
                SetStatus($"✅ Aluno ID {id} atualizado com sucesso.", Colors.SeaGreen);
                LimparFormulario();
                CarregarAlunos();
            }
            else
            {
                SetStatus($"📨 Nenhum aluno encontrado com ID {id}.", Colors.DarkOrange);
            }
        }
        catch (Exception ex)
        {
            SetStatus($"❌ Erro ao atualizar: {ex.Message}", Colors.Crimson);
        }
    }

    /// <summary>4 – Remover aluno</summary>
    private void BtnRemover_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(TxtId.Text.Trim(), out var id) || id <= 0)
        {
            SetStatus("⚠️  Selecione um aluno na lista ou busque pelo ID antes de remover.",
                      Colors.DarkOrange);
            return;
        }

        var confirm = MessageBox.Show(
            $"Deseja realmente remover o aluno de ID {id}?",
            "Confirmar Exclusão",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (confirm != MessageBoxResult.Yes) return;

        try
        {
            bool ok = _repo.Remover(id);
            if (ok)
            {
                SetStatus($"✅ Aluno ID {id} removido com sucesso.", Colors.SeaGreen);
                LimparFormulario();
                CarregarAlunos();
            }
            else
            {
                SetStatus($"📨 Nenhum aluno encontrado com ID {id}.", Colors.DarkOrange);
            }
        }
        catch (Exception ex)
        {
            SetStatus($"❌ Erro ao remover: {ex.Message}", Colors.Crimson);
        }
    }

   
    private void BtnBuscar_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(TxtBuscaId.Text.Trim(), out var id) || id <= 0)
        {
            SetStatus("⚠️  Informe um ID válido para buscar.", Colors.DarkOrange);
            TxtBuscaId.Focus();
            return;
        }

        try
        {
            var aluno = _repo.BuscarPorId(id);
            if (aluno is not null)
            {
                PreencherFormulario(aluno);
                SetStatus($"🔍 Aluno encontrado: {aluno.Nome}, {aluno.Idade} anos.", Colors.SteelBlue);
            }
            else
            {
                SetStatus($"📨 Nenhum aluno encontrado com ID {id}.", Colors.DarkOrange);
                LimparFormulario();
            }
        }
        catch (Exception ex)
        {
            SetStatus($"❌ Erro na busca: {ex.Message}", Colors.Crimson);
        }
    }

    private void BtnLimpar_Click(object sender, RoutedEventArgs e)
    {
        LimparFormulario();
        SetStatus("Campos limpos. Pronto para nova entrada.", Colors.Gray);
    }

    private void GridAlunos_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (GridAlunos.SelectedItem is Aluno aluno)
        {
            PreencherFormulario(aluno);
            SetStatus($"📌 Aluno selecionado: {aluno.Nome} (ID {aluno.Id})", Colors.SteelBlue);
        }
    }
}
