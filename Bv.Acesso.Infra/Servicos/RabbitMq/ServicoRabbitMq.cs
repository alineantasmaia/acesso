using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bv.Acesso.Dominio.Interfaces.Servicos;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Bv.Acesso.Infra.Servicos.RabbitMq
{
    public class ServicoRabbitMq: IServicoRabbitMq
    {
        private IConnection _connection;
        private IModel _canal;

        public ServicoRabbitMq(IConfiguration configuration)
        {
            var fabrica = new ConnectionFactory()
            {
                HostName = "localhost",
                VirtualHost = "localhost",
                UserName = "root",
                Password = "password",
                Port = 80
            };
            _connection = fabrica.CreateConnection();
            CriarCanal();
        }

        private void CriarCanal()
        {
            _canal = _connection.CreateModel();
            _canal.BasicQos(0, 5, false);
        }

        public IModel model => _canal;

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            _canal?.Dispose();
            _connection?.Dispose();
        }
        public void ConfigurarFila(string routeKey,string fila,string exchange)
        {
            _canal.ExchangeDeclare(exchange:exchange,type:"direct",durable:true);
            _canal.QueueDeclare(queue: fila, durable: true, exclusive: false, autoDelete: false);
            _canal.QueueBind(queue: fila, exchange: exchange, routingKey: routeKey);
        }
        public void ConfigurarFila(string nomeFile)
        {
            _canal.QueueDeclare(queue:nomeFile,durable:true,exclusive:false,autoDelete:false);
        }
        public void CloseConection()
        {
            _connection?.Close();
            _canal?.Close();
        }

    }
}
