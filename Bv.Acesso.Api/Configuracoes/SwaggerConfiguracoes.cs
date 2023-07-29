using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using System.Linq;

namespace Bv.Acesso.Api.Configuracoes
{
    public static class SwaggerConfiguracoes
    {
        public static void AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddApiVersioning(
                opcoes =>
                {
                    opcoes.AssumeDefaultVersionWhenUnspecified = true;
                    opcoes.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                    opcoes.ReportApiVersions = true;
                });
            services.AddVersionedApiExplorer(opcoes =>
            {
                opcoes.GroupNameFormat = "'v'VVV";
                opcoes.SubstituteApiVersionInUrl = true;
            });
            services.AddSwaggerGen(c => 
            {
                c.OperationFilter<SwaggerDefaultValues>();
            });
        }       
     
        public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder application,IApiVersionDescriptionProvider provider)
        {
            application.UseSwagger();
            application.UseSwaggerUI(options => 
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",description.GroupName.ToUpperInvariant());
                }                
            });
            return application;
        }
        
    }
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach(var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "API - Acesso",
                Version = description.ApiVersion.ToString(),
                Description = "Descricao da API",
                Contact = new OpenApiContact() { Name = "BV", Email="aline.maia@act.com.br" }

            };

            return info;
        }
    }
    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;
            operation.Deprecated |= apiDescription.IsDeprecated();

            if (operation.Parameters == null)
                return;

            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
                
                if(parameter.Description == null)               
                    parameter.Description = description.ModelMetadata?.Description;                
                
                if(parameter.Schema.Default == null && description.DefaultValue != null)                
                    parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());

                parameter.Required |= description.IsRequired;
            }

        }
    }
    public class FiltroIgnoraPropriedadeSwagger:IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var todosTipos = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetExportedTypes());

            foreach (var definicao in swaggerDoc.Components.Schemas)
            {
                var tipo = todosTipos.FirstOrDefault(x => x.Name == definicao.Key);
                if (tipo != null)
                {
                    var propriedades = tipo.GetProperties();
                    foreach (var prop in propriedades.ToList())
                    {
                        if (Attribute.GetCustomAttribute(prop, typeof(Newtonsoft.Json.JsonIgnoreAttribute)) is Newtonsoft.Json.JsonIgnoreAttribute ignore)
                            definicao.Value.Properties.Remove(prop.Name);
                    }
                }
            }
        }

        
    }   

}
