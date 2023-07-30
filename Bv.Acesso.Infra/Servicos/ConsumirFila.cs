using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bv.Acesso.Dominio.Interfaces.Servicos;
using Bv.Acesso.Infra.Servicos.RabbitMq;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Bv.Acesso.Dominio.Dto;

namespace Bv.Acesso.Infra.Servicos
{
    public class ConsumirFila : ServicoConsumidorDaBase
    {
        private readonly IServiceProvider _serviceProvider;
        private IServicoTipoErro<Evento> _servicoTipoErro;
        private readonly IServicoInformaEvento _servicoInformaEvento;
        public ConsumirFila(IServicoRabbitMq _rabbitMq,IServiceProvider serviceProvider, IServicoInformaEvento servicoInformaEvento):base(_rabbitMq) 
        {
            _serviceProvider = serviceProvider;
            _servicoInformaEvento = servicoInformaEvento;
            //_rabbitMq.model;
        }

        public override async Task<bool> Processar(byte[] message)
        {
            //var eventoLogs = new EventoLogs(DateTime.Now);
            var evento = default(Evento);

            try
            {
                using (var escopo = _serviceProvider.CreateScope())
                {
                    var servico = escopo.ServiceProvider.GetRequiredService<IServicoEvento>();
                    _servicoTipoErro = escopo.ServiceProvider.GetRequiredService<IServicoTipoErro<Evento>>();
                    evento = JsonConvert.DeserializeObject<Evento>(Encoding.UTF8.GetString(message));
                    await servico.Executar(evento);                      

                }
            }
            catch (Exception ex) 
            {
                _servicoTipoErro.AddErros("","",evento);
            }
            finally
            {
                EnviarEventosLog(evento);
            }
            return await Task.Run(() => true);
        }
        private void EnviarEventosLog(Evento evento)
        {
            if (_servicoTipoErro.Invalido)
            {
                //var remarks = _servicoTipoErro.Notificacoes.Select(n => $"{n.Titulo} - {n.mensagem} - {n.TipoNotificacao}".ToList());
            }
            
        }
    }
}
