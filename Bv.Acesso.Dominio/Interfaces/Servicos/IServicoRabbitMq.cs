using RabbitMQ.Client;

namespace Bv.Acesso.Dominio.Interfaces.Servicos
{
    public interface IServicoRabbitMq : IDisposable
    {        
        IModel model { get; }
        void ConfigurarFila(string nomeFila);
        void ConfigurarFila(string routeKey, string nomeFila,string exchange);
        void CloseConection();
    }
}
