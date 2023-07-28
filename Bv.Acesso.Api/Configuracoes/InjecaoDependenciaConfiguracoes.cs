using Bv.Acesso.Dominio.Interfaces.Servicos;
using Bv.Acesso.Dominio.Services;
using Microsoft.Extensions.DependencyInjection;


namespace Bv.Acesso.Api.Configuracoes
{
    public class InjecaoDependenciaConfiguracoes
    {
        public static void RegisterInjecaoDependencia(IServiceCollection services)
        {
            services.AddScoped<IServicoFundTransfer, ServicoFundTransfer>();
        }
    }
}
