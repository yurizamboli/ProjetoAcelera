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
        private List<Evento> eventos = new List<Evento>();
       
        public EventoService()
        {
            AdicionarEventos("Evento de Tecnologia", "2024-07-15", "Um evento para discutir as últimas tendências em tecnologia.", "Participe de palestras, workshops e networking com profissionais da área.", "https://example.com/imagem-tecnologia.jpg");
            AdicionarEventos("Feira de Ciências", "2024-08-20", "Uma feira para apresentar projetos científicos inovadores.", "Explore experimentos, apresentações e interaja com cientistas de todas as idades.", "https://example.com/imagem-feira-ciencias.jpg");
            AdicionarEventos("Festival de Música", "2024-09-10", "Um festival para celebrar a diversidade musical.", "Desfrute de performances ao vivo, food trucks e atividades para toda a família.", "https://example.com/imagem-festival-musica.jpg");
        }
        public List<Evento> ObterEvento()
        {
            return eventos;
        }

        //Tava private, deixei public para poder adicionar eventos depois
        public void AdicionarEventos(string titulo, string data, string descricao, string detalhes, string imagem)
        {       
                Evento novoEvento = new Evento
                {
                    Titulo = titulo,
                    Data = data,
                    Descricao = descricao,
                    Detalhes = detalhes,
                    Imagem = imagem
                };
                eventos.Add(novoEvento);
            
            
        }
    }
}
