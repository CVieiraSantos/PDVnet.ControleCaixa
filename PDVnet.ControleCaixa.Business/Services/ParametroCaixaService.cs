using PDVnet.ControleCaixa.Business.Exceptions;
using PDVnet.ControleCaixa.Business.Interfaces;
using PDVnet.ControleCaixa.Data.Repositories.Interfaces;

namespace PDVnet.ControleCaixa.Business.Services
{
    public class ParametroCaixaService : IParametroCaixaService
    {
        private readonly IParametroCaixaRepository _parametroCaixaRepository;

        public ParametroCaixaService(IParametroCaixaRepository parametroCaixaRepository)
        {
            _parametroCaixaRepository = parametroCaixaRepository;
        }

        public async Task AtualizarSaldoMinimoAsync(decimal saldoMinimo)
        {
            if(saldoMinimo <= 0)
            {
                throw new ValidacaoException("O saldo mínimo deve ser maior que zero.");
            }
            
            await _parametroCaixaRepository.AtualizarSaldoMinimoAsync(saldoMinimo);
        }

        public async Task<decimal> ObterSaldoMinimoAsync()
        {
            return await _parametroCaixaRepository.ObterSaldoMinimoAsync();
        }
    }
}
