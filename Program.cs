using ProjetoAcelera.Models;
using ProjetoAcelera.Services;
namespace ProjetoAcelera
{
    internal class Program
    {
        static void Main(string[] args)
        {



            var usuarioService = new UsuarioService();
            Console.WriteLine("Digite o nome:");
            string nome = Console.ReadLine();

            Console.WriteLine("Digite o email:");
            string email = Console.ReadLine();

            Console.WriteLine("Digite a senha:");
            string senha = Console.ReadLine();


            usuarioService.Cadastrar(nome, senha, email);
        }
    }
}
