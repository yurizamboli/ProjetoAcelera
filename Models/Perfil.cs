// guarda as informações do perfil do usuário, informações que o usuário pode definir.
namespace ProjetoAcelera.Models
{
    public class Perfil
    {
        public string Facebook { get; set; }
        public string Bio { get; set; }
        public string Instagram { get; set; }
        public string FotoPerfil {  get; set; }
        public bool Destaque { get; set; }
    }
}
