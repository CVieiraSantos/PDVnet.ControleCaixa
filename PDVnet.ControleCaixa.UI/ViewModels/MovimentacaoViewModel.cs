using CommunityToolkit.Mvvm.ComponentModel;
using PDVnet.ControleCaixa.Model.Enums;

namespace PDVnet.ControleCaixa.UI.ViewModels
{
    public partial class MovimentacaoViewModel : ViewModelBase
    {
        [ObservableProperty]
        private int id;

        [ObservableProperty]
        private string descricao = string.Empty;

        [ObservableProperty]
        private TipoMovimentacao tipo;

        [ObservableProperty]
        private string? categoria;

        [ObservableProperty]
        private decimal valor;

        [ObservableProperty]
        private DateTime dataMovimento;

        [ObservableProperty]
        private bool status;
    }
}
