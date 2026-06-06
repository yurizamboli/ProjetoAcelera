// guarda as informações de um comentário feito em uma publicação.
namespace ProjetoAcelera.Models
{
    public class Comentario
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string NomeAutor { get; set; } = "";
        public string EmailAutor { get; set; } = "";
        public string Conteudo { get; set; } = "";
        public DateTime DataComentario { get; set; } = DateTime.Now;
        
        // situação do comentário: pendente, aprovado ou reprovado.
        public string Status { get; set; } = "Aguardando aprovação";
    }
}
