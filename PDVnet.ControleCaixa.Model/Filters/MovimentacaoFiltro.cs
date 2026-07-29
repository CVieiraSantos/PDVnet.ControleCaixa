using PDVnet.ControleCaixa.Model.Enums;

namespace PDVnet.ControleCaixa.Model.Filters
{
    public class MovimentacaoFiltro
    {
        public DateTime? DataInicial { get; set; }

        public DateTime? DataFinal { get; set; }

        public TipoMovimentacao? Tipo { get; set; }

        public string? Categoria { get; set; }
    }
}
