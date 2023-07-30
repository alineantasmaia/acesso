using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bv.Acesso.Dominio.Dto;

namespace Bv.Acesso.Dominio.Interfaces.Servicos
{
    public interface IServicoTipoErro<T> where T : Evento
    {
        bool Valido { get; }
        IList<string> Notificacoes { get; set; }
        bool Invalido { get; }
        void AddErros(IReadOnlyCollection<string> notificacoes, T evento = null);
        void AddErros(string contante, string descricao, T evento = null);
        void AddErros(List<string> constantes, string descricao = null, T evento = null);
        void AddErroValidacao(string constante, string descricao, T evento = null, bool erroConstanteGenerica = false);
        void AddErroValidacao(List<string> constante, string descricao = null, T evento = null);
        void AddErroInconsistencia(string constante, string valorEsperado, string valorEncontrado);

    }
}
