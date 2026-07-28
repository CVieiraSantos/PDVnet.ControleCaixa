using PDVnet.ControleCaixa.Model.Enums;

namespace PDVnet.ControleCaixa.Model.Entities
{
    public class Movimentacao
    {
        public int Id { get; set; }

        public string Descricao { get; set; } = string.Empty;

        public TipoMovimentacao Tipo { get; set; }

        public string? Categoria { get; set; }

        public decimal Valor { get; set; }

        public DateTime DataMovimento { get; set; }

        public bool Status { get; set; }
    }
}
