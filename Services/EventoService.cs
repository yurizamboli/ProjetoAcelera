using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoAcelera.Models;


namespace ProjetoAcelera.Services
{
    public class EventoService
    {
        private List<Evento> eventos;
        private ArquivoService arquivoService;

        public EventoService(ArquivoService arquivoService  )
        {
            this.arquivoService = arquivoService;
            eventos = arquivoService.CarregarEventos();
            if (eventos == null)
            {
                eventos = new List<Evento>();
            }
        }
        public List<Evento> ObterEvento()
        {
            return eventos.OrderBy(e => e.DataInicio).ToList();
        }
        public List<Evento> ObterEventosDestaque()
        {
            return eventos.Where(e => e.Destaque).OrderBy(e => e.DataInicio).ToList();
        }
        public List<Evento> ObterEventosPorData(DateTime data)
        {
            return eventos
                .Where(e =>e.DataInicio.Date <= data.Date &&(e.DataFim == null || e.DataFim.Value.Date >= data.Date))
                .OrderBy(e => e.DataInicio)
                .ToList();
        }
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
        public void RemoverEvento(Guid id)
        {
            var evento = eventos.FirstOrDefault(e => e.Id == id);

            if (evento == null)
            {
                return;
            }

            eventos.Remove(evento);
        }

        public void SalvarEventos()
        {
            arquivoService.SalvarEventos(eventos);
        }
    }
}

