using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using System.Threading;
using Bv.Acesso.Dominio.Interfaces.Servicos; 

namespace Bv.Acesso.Infra.Servicos.RabbitMq
{
    public abstract class ServicoConsumidorDaBase : IHostedService,IDisposable
    {
        private readonly IServicoRabbitMq _servicoRabbitMq;
        public ServicoConsumidorDaBase(IServicoRabbitMq servicoRabbitMq)
        {
                _servicoRabbitMq = servicoRabbitMq;
        }
        public void Dispose() 
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing) 
        {
            _servicoRabbitMq.Dispose();
        }        
        public void RegistrarConsumidor(string fila)
        {
            _servicoRabbitMq.ConfigurarFila(fila);

            var consumidor = new EventingBasicConsumer(_servicoRabbitMq.model);
            consumidor.Received += (model, ea) =>
            {
                Processar(ea.Body.ToArray());
                _servicoRabbitMq.model.BasicAck(ea.DeliveryTag, false);
            };
            _servicoRabbitMq.model.BasicConsume(queue: fila, consumer: consumidor, autoAck: false);
        }
        public abstract Task<bool> Processar(byte[] message);

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _servicoRabbitMq.CloseConection();
            return Task.CompletedTask;
        }
    }
}
