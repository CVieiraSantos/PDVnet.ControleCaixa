using PDVnet.ControleCaixa.Model.Entities;
using PDVnet.ControleCaixa.UI.ViewModels;

namespace PDVnet.ControleCaixa.UI.Mappings
{
    public static class MovimentacaoMapper
    {
        public static Movimentacao ToEntity(MovimentacaoViewModel viewModel)
        {
            return new Movimentacao
            {
                Id = viewModel.Id,
                Descricao = viewModel.Descricao,
                Tipo = viewModel.Tipo,
                Categoria = viewModel.Categoria,
                Valor = viewModel.Valor,
                DataMovimento = viewModel.DataMovimento,
                Status = viewModel.Status
            };
        }

        public static MovimentacaoViewModel ToViewModel(Movimentacao entity)
        {
            return new MovimentacaoViewModel
            {
                Id = entity.Id,
                Descricao = entity.Descricao,
                Tipo = entity.Tipo,
                Categoria = entity.Categoria,
                Valor = entity.Valor,
                DataMovimento = entity.DataMovimento,
                Status = entity.Status
            };
        }

        public static MovimentacaoViewModel Clone(MovimentacaoViewModel origem)
        {
            ArgumentNullException.ThrowIfNull(origem);

            return new MovimentacaoViewModel
            {
                Id = origem.Id,
                Descricao = origem.Descricao,
                Tipo = origem.Tipo,
                Categoria = origem.Categoria,
                Valor = origem.Valor,
                DataMovimento= origem.DataMovimento,
                Status = origem.Status
            };
        }
    }
}
