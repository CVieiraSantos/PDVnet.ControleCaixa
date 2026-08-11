using Microsoft.Extensions.DependencyInjection;
using PDVnet.ControleCaixa.Business.Interfaces;
using PDVnet.ControleCaixa.Business.Services;
using PDVnet.ControleCaixa.Data.Connection;
using PDVnet.ControleCaixa.Data.Repositories;
using PDVnet.ControleCaixa.Data.Repositories.Interfaces;

namespace PDVnet.ControleCaixa.Infra.Ioc.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegistrarDependencias(this IServiceCollection services)
        {
            {
                services.AddSingleton<IConnectionFactory, SqlConnectionFactory>();
                services.AddScoped<IMovimentacaoRepository, MovimentacaoRepository>();
                services.AddScoped<IMovimentacaoService, MovimentacaoService>();
                services.AddScoped<IParametroCaixaRepository, ParametroCaixaRepository>();
                services.AddScoped<IParametroCaixaService, ParametroCaixaService>();
                return services;
            }
        }
    }
}
