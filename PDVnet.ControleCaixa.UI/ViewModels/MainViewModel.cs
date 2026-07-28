using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PDVnet.ControleCaixa.Business.Exceptions;
using PDVnet.ControleCaixa.Business.Interfaces;
using PDVnet.ControleCaixa.Model.Entities;
using PDVnet.ControleCaixa.Model.Enums;
using PDVnet.ControleCaixa.UI.Enums;
using PDVnet.ControleCaixa.UI.Mappings;
using PDVnet.ControleCaixa.UI.Services;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace PDVnet.ControleCaixa.UI.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly IMovimentacaoService _movimentacaoService;
        private readonly INotificationService _notificationService;

        public MainViewModel(IMovimentacaoService movimentacaoService, INotificationService notificationService)
        {
            _movimentacaoService = movimentacaoService;
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

        [ObservableProperty]
        private int totalMovimentacoes;

        public string StatusCaixa
        {
            get
            {
                return SaldoAtual < 100
                    ? "Saldo abaixo de R$ 100,00"
                    : "Caixa saudável";
            }
        }

        public Brush CorStatusCaixa
        {
            get
            {
                return SaldoAtual < 100
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
    }
}