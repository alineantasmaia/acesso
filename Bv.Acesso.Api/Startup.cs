using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Bv.Acesso.Api.Configuracoes;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Bv.Acesso.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuracao;
        public IContainer ApplicationContainer { get; private set; }
        private readonly ContainerBuilder _containerBuilder;

        public Startup(IConfiguration config)
        {
            _configuracao = config;
            _containerBuilder = new ContainerBuilder();
        }
        public void ConfigureServices(IServiceCollection services) 
        {
            services.AddSwaggerConfig();
            services.AddVersionedApiExplorer();
            services.AddInjecaoDependenciaConfig();
            services.AddControllers();            
        }
        public IServiceProvider ConfigureContainer(IServiceCollection services)
        {
            ApplicationContainer =_containerBuilder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
        }
        public void Configure(IApplicationBuilder application,IWebHostEnvironment environment,IApiVersionDescriptionProvider provider)
        {
            if(environment.IsDevelopment())application.UseDeveloperExceptionPage();
            
            application.UseSwagger();
            application.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "BvAcessoApi V1"); c.RoutePrefix = "swagger"; });
            
            application.UseRouting();
            application.UseSwaggerConfig(provider);
            application.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
