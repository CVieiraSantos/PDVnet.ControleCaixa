using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PDVnet.ControleCaixa.Business.Exceptions;
using PDVnet.ControleCaixa.Business.Interfaces;
using PDVnet.ControleCaixa.Model.Entities;
using PDVnet.ControleCaixa.Model.Enums;
using PDVnet.ControleCaixa.Model.Filters;
using PDVnet.ControleCaixa.UI.Enums;
using PDVnet.ControleCaixa.UI.Filtro;
using PDVnet.ControleCaixa.UI.Mappings;
using PDVnet.ControleCaixa.UI.Services;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace PDVnet.ControleCaixa.UI.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly IMovimentacaoService _movimentacaoService;
        private readonly IParametroCaixaService _parametroCaixaService;
        private readonly INotificationService _notificationService;

        public MainViewModel(IMovimentacaoService movimentacaoService,IParametroCaixaService parametroCaixaService,INotificationService notificationService)
        {
            _movimentacaoService = movimentacaoService;
            _parametroCaixaService = parametroCaixaService;
            _notificationService = notificationService;

            Movimentacao = CriarNovaMovimentacao();
        }

        #region Properties

        [ObservableProperty]
        private MovimentacaoViewModel movimentacao = new();

        [ObservableProperty]
        private ObservableCollection<MovimentacaoViewModel> movimentacoes = new();

        [NotifyPropertyChangedFor(nameof(StatusCaixa))]
        [NotifyPropertyChangedFor(nameof(CorStatusCaixa))]
        [ObservableProperty]
        private decimal saldoAtual;

        [NotifyPropertyChangedFor(nameof(StatusCaixa))]
        [NotifyPropertyChangedFor(nameof(CorStatusCaixa))]
        [ObservableProperty]
        private decimal saldoMinimo = 100m;

        [ObservableProperty]
        private int totalMovimentacoes;

        [ObservableProperty]
        private DateTime? dataInicial;

        [ObservableProperty]
        private DateTime? dataFinal;

        [ObservableProperty]
        private ItemFiltroTipo? tipoFiltro;
        public IReadOnlyList<ItemFiltroTipo> TiposFiltro { get; } =
        [
            new()
            {
                Descricao = "Todos",
                Tipo = null
            },
            new()
            {
                Descricao = "Entrada",
                Tipo = TipoMovimentacao.Entrada
            },
            new()
            {
                Descricao = "Saída",
                Tipo = TipoMovimentacao.Saida
            }
        ];

        [ObservableProperty]
        private string? categoriaFiltro;

        public string StatusCaixa
        {
            get
            {
                return SaldoAtual < SaldoMinimo
                    ? $"Saldo abaixo de {SaldoMinimo:C2}"
                    : "Caixa saudável";
            }
        }

        public Brush CorStatusCaixa
        {
            get
            {
                return SaldoAtual < SaldoMinimo
                    ? Brushes.Red
                    : Brushes.Green;
            }
        }

        [ObservableProperty]
        private MovimentacaoViewModel? movimentacaoSelecionada;

        [NotifyPropertyChangedFor(nameof(PodeEditar))]
        [NotifyPropertyChangedFor(nameof(MostrarSalvar))]
        [NotifyPropertyChangedFor(nameof(MostrarAtualizar))]
        [NotifyPropertyChangedFor(nameof(MostrarExcluir))]
        [NotifyPropertyChangedFor(nameof(PodeCriarNovaMovimentacao))]
        [ObservableProperty]
        private EstadoTela estadoTela = EstadoTela.Consulta;

        #endregion

        #region Computed Properties

        public bool PodeEditar { get { return EstadoTela != EstadoTela.Consulta; } }

        public bool MostrarSalvar { get { return EstadoTela == EstadoTela.Cadastro; } }

        public bool MostrarAtualizar { get { return EstadoTela == EstadoTela.Edicao; } }

        public bool MostrarExcluir { get { return EstadoTela == EstadoTela.Edicao; } }

        public bool PodeCriarNovaMovimentacao { get { return EstadoTela == EstadoTela.Consulta; } }

        public IEnumerable<TipoMovimentacao> TiposMovimentacao { get; }
            = Enum.GetValues<TipoMovimentacao>();

        #endregion

        #region Inicialização

        public async Task InicializarAsync()
        {
            SaldoMinimo = await _parametroCaixaService.ObterSaldoMinimoAsync();

            await AtualizarTelaAsync();
        }

        #endregion

        #region Commands

        [RelayCommand]
        private void NovaMovimentacao()
        {
            LimparFormulario();

            EstadoTela = EstadoTela.Cadastro;
        }

        [RelayCommand]
        private async Task AtualizarSaldoMinimoAsync()
        {
            try
            {
                await _parametroCaixaService.AtualizarSaldoMinimoAsync(SaldoMinimo);

                _notificationService.Information("Saldo mínimo de alerta atualizado com sucesso.");
            }
            catch (ValidacaoException ex)
            {
                _notificationService.Warning(ex.Message);
            }
            catch (Exception)
            {
                _notificationService.Error("Ocorreu um erro ao atualizar o saldo mínimo.");
            }
        }

        [RelayCommand]
        private async Task PesquisarAsync()
        {
            MovimentacaoFiltro filtro = CriarFiltro();

            IReadOnlyList<Movimentacao> lista =
                await _movimentacaoService.PesquisarAsync(filtro);

            Movimentacoes.Clear();

            foreach (Movimentacao item in lista)
            {
                Movimentacoes.Add(MovimentacaoMapper.ToViewModel(item));
            }

            await AtualizarSaldoAsync();
        }

        [RelayCommand]
        private async Task LimparFiltrosAsync()
        {
            DataInicial = null;
            DataFinal = null;
            TipoFiltro = null;
            CategoriaFiltro = null;

            await AtualizarTelaAsync(false);
        }

        [RelayCommand]
        private async Task SalvarAsync()
        {
            try
            {
                Movimentacao movimentacao = CriarMovimentacao();

                await _movimentacaoService.InserirAsync(movimentacao);

                EstadoTela = EstadoTela.Consulta;

                await AtualizarTelaAsync();

                _notificationService.Information("Movimentação cadastrada com sucesso.");
            }
            catch (ValidacaoException ex)
            {
                _notificationService.Warning(ex.Message);
            }
            catch (Exception)
            {
                _notificationService.Error(
                    "Ocorreu um erro ao salvar a movimentação.");
            }
        }

        [RelayCommand]
        private async Task AtualizarAsync()
        {
            if (MovimentacaoSelecionada is null)
            {
                return;
            }

            try
            {
                Movimentacao movimentacao = CriarMovimentacao();

                await _movimentacaoService.AtualizarAsync(movimentacao);

                EstadoTela = EstadoTela.Consulta;

                await AtualizarTelaAsync();

                _notificationService.Information("Movimentação atualizada com sucesso.");
            }
            catch (ValidacaoException ex)
            {
                _notificationService.Warning(ex.Message);
            }
            catch (Exception)
            {
                _notificationService.Error(
                    "Ocorreu um erro ao atualizar a movimentação.");
            }
        }

        [RelayCommand]
        private async Task ExcluirAsync()
        {
            if (MovimentacaoSelecionada is null)
            {
                return;
            }

            try
            {
                await _movimentacaoService.ExcluirAsync(MovimentacaoSelecionada.Id);

                EstadoTela = EstadoTela.Consulta;

                await AtualizarTelaAsync();

                _notificationService.Information("Movimentação excluída com sucesso.");
            }
            catch (Exception)
            {
                _notificationService.Error(
                    "Ocorreu um erro ao excluir a movimentação.");
            }
        }

        #endregion

        #region Métodos Privados

        private async Task AtualizarTelaAsync(bool limparFormulario = true)
        {
            await CarregarMovimentacoesAsync();

            await AtualizarSaldoAsync();

            if (limparFormulario)
            {
                LimparFormulario();
            }
        }

        private async Task CarregarMovimentacoesAsync()
        {
            IReadOnlyList<Movimentacao> lista =
                await _movimentacaoService.ObterTodasAsync();

            Movimentacoes.Clear();

            foreach (Movimentacao item in lista)
            {
                Movimentacoes.Add(MovimentacaoMapper.ToViewModel(item));
            }

            TotalMovimentacoes = Movimentacoes.Count;
        }

        private async Task AtualizarSaldoAsync()
        {
            SaldoAtual = await _movimentacaoService.ObterSaldoAsync();
        }

        private Movimentacao CriarMovimentacao()
        {
            Movimentacao movimentacao =
                MovimentacaoMapper.ToEntity(Movimentacao);

            movimentacao.Status = true;

            return movimentacao;
        }

        private MovimentacaoViewModel CriarNovaMovimentacao()
        {
            return new MovimentacaoViewModel
            {
                Tipo = TipoMovimentacao.Entrada
            };
        }

        private void LimparFormulario()
        {
            Movimentacao = CriarNovaMovimentacao();

            MovimentacaoSelecionada = null;
        }

        #endregion

        partial void OnMovimentacaoSelecionadaChanged(MovimentacaoViewModel? value)
        {
            if (value is null)
            {
                LimparFormulario();

                EstadoTela = EstadoTela.Consulta;

                return;
            }

            Movimentacao = MovimentacaoMapper.Clone(value);

            EstadoTela = EstadoTela.Edicao;
        }

        private MovimentacaoFiltro CriarFiltro()
        {
            return new MovimentacaoFiltro
            {
                DataInicial = DataInicial,
                DataFinal = DataFinal,
                Tipo = TipoFiltro?.Tipo,
                Categoria = CategoriaFiltro
            };
        }
    }
}