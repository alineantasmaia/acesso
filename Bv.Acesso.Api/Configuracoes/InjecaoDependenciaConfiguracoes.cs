using Bv.Acesso.Dominio.Interfaces.Servicos;
using Bv.Acesso.Dominio.Services;
using Microsoft.Extensions.DependencyInjection;


namespace Bv.Acesso.Api.Configuracoes
{
    public static class InjecaoDependenciaConfiguracoes
    {
        public static void AddInjecaoDependenciaConfig(this IServiceCollection services)
        {
            services.AddScoped<IServicoFundTransfer, ServicoFundTransfer>();            
            services.AddSingleton<IServicoFundTransfer, ServicoFundTransfer>();
        }
    }
}
