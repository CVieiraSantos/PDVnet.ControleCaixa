using System.ComponentModel;

namespace PDVnet.ControleCaixa.Model.Enums
{
    public enum TipoMovimentacao
    {
        /// <summary>
        /// Indica que a movimentação é uma entrada de caixa.
        /// </summary>
        [Description("Entrada")]
        Entrada = 1,
        /// <summary>
        /// Indica que a movimentação é uma saída de caixa.
        /// </summary>
        [Description("Saída")]
        Saida = 2,
    }
}
