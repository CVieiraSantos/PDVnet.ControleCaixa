using PDVnet.ControleCaixa.Business.Exceptions;
using PDVnet.ControleCaixa.Model.Entities;

namespace PDVnet.ControleCaixa.Business.Validators
{
    public static class MovimentacaoValidator
    {
        public static void Validar(Movimentacao movimentacao)
        {
            ArgumentNullException.ThrowIfNull(movimentacao);

            if (string.IsNullOrWhiteSpace(movimentacao.Descricao))
            {
                throw new ValidacaoException("A descrição é obrigatória.");
            }

            if (movimentacao.Valor <= 0)
            {
                throw new ValidacaoException("O valor deve ser maior que zero.");
            }

            if (!Enum.IsDefined(movimentacao.Tipo))
            {
                throw new ValidacaoException("Tipo de movimentação inválido.");
            }
        }
    }
}
