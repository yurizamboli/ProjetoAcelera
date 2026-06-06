using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoAcelera.Models;

// cuida de toda a lógica relacionada aos eventos
namespace ProjetoAcelera.Services
{
    public class EventoService
    {
        private List<Evento> eventos;
        private ArquivoService arquivoService;
        
        public EventoService(ArquivoService arquivoService)
        {
            this.arquivoService = arquivoService;
            eventos = arquivoService.CarregarEventos();
            if (eventos == null)
            {
                eventos = new List<Evento>();
            }
        }

        // retorna a lista de eventos ordenada pela data de início, do mais recente para o mais antigo.
        public List<Evento> ObterEvento()
        {
            return eventos.OrderByDescending(e => e.DataInicio).ToList();
        }

        // retorna os eventos que estão marcados como destaque, ordenados pela data de início, do mais recente para o mais antigo.
        public List<Evento> ObterEventosDestaque()
        {
            return eventos.Where(e => e.Destaque).OrderBy(e => e.DataInicio).ToList();
        }

        // retorna os eventos que estão ocorrendo em uma data específica, ou seja, eventos cuja data de início é anterior ou igual à data fornecida e cuja data de fim é posterior ou igual à data fornecida (ou seja, eventos que estão ativos nessa data).
        public List<Evento> ObterEventosPorData(DateTime data)
        {
            return eventos.Where(e =>e.DataInicio.Date <= data.Date &&(e.DataFim == null || e.DataFim.Value.Date >= data.Date)).OrderBy(e => e.DataInicio).ToList();
        }

        // adiciona um novo evento à lista de eventos, criando um novo objeto Evento com os dados fornecidos e atribuindo um novo ID único (Guid.NewGuid()).
        public void AdicionarEvento(string titulo, DateTime datainicio, DateTime? datafim, string descricao, string programacao,string local, string imagem, bool destaque)
        {
            Evento novoEvento = new Evento
            {
                Id = Guid.NewGuid(),
                Titulo = titulo,
                DataInicio = datainicio,
                DataFim = datafim,
                Descricao = descricao,
                Programacao = programacao,
                Local = local,
                Imagem = imagem,
                Destaque = destaque
            };
            eventos.Add(novoEvento);
        }

        // remove um evento da lista de eventos com base no ID fornecido. Ele procura o evento correspondente na lista e, se encontrado, remove-o.
        public void RemoverEvento(Guid id)
        {
            var evento = eventos.FirstOrDefault(e => e.Id == id);
            if (evento == null)
            {
                return;
            }
            eventos.Remove(evento);
        }

        // salva a lista de eventos usando o serviço de arquivo, garantindo que as alterações feitas na lista de eventos sejam persistidas.
        public void SalvarEventos()
        {
            arquivoService.SalvarEventos(eventos);
        }
    }
}

