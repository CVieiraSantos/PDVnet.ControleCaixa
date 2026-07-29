using PDVnet.ControleCaixa.Model.Enums;

namespace PDVnet.ControleCaixa.UI.Filtro
{
    public sealed class ItemFiltroTipo
    {
        public string Descricao { get; set; } = string.Empty;

        public TipoMovimentacao? Tipo { get; set; }
    }
}
