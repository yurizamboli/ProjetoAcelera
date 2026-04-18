/*using ProjetoAcelera.Services;

namespace ProjetoAcelera
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UsuarioService usuarioService = new UsuarioService();
            ObraService obraService = new ObraService(usuarioService);
            PerfilService perfilService = new PerfilService(usuarioService);

            // ===== CADASTRO =====
            while (true)
            {
                Console.WriteLine("Deseja cadastrar um usuário? (s/n)");
                string comeco = Console.ReadLine().Trim().ToLower();

                if (comeco == "n" || comeco == "nao")
                {
                    break;
                }

                Console.Write("Nome: ");
                string nome = Console.ReadLine();

                Console.Write("Email: ");
                string email = Console.ReadLine();

                Console.Write("Senha: ");
                string senha = Console.ReadLine();

                usuarioService.Cadastrar(nome, senha, email);
            }

            // ===== LOGIN =====
            Console.WriteLine("Deseja fazer login agora? (s/n)");
            string respostaLogin = Console.ReadLine().Trim().ToLower();

            if (respostaLogin == "s" || respostaLogin == "sim")
            {
                while (true)
                {
                    Console.Write("Email: ");
                    string email = Console.ReadLine();

                    Console.Write("Senha: ");
                    string senha = Console.ReadLine();

                    bool sucesso = usuarioService.Login(email, senha);

                    if (sucesso)
                    {
                        Console.WriteLine("\nLogin realizado com sucesso!");
                        Console.WriteLine("Usuário logado: " + usuarioService.UsuarioLogado.Nome);

                        // ===== MENU =====
                        while (true)
                        {
                            Console.WriteLine("\n--- MENU ---");
                            Console.WriteLine("1 - Adicionar obra");
                            Console.WriteLine("2 - Listar obras");
                            Console.WriteLine("3 - Remover obra");
                            Console.WriteLine("4 - Favoritar obra");
                            Console.WriteLine("5 - Atualizar perfil");
                            Console.WriteLine("0 - Logout");

                            string opcao = Console.ReadLine();

                            if (opcao == "1")
                            {
                                Console.Write("Título: ");
                                string titulo = Console.ReadLine();

                                Console.Write("Descrição: ");
                                string descricao = Console.ReadLine();

                                Console.Write("Capa: ");
                                string capa = Console.ReadLine();

                                obraService.AdicionarObra(titulo, descricao, capa);
                            }
                            else if (opcao == "2")
                            {
                                obraService.ListarObras();
                            }                            
                            else if (opcao == "5")
                            {
                                Console.Write("Facebook: ");
                                string facebook = Console.ReadLine();

                                Console.Write("Instagram: ");
                                string instagram = Console.ReadLine();

                                Console.Write("Bio: ");
                                string bio = Console.ReadLine();

                                Console.Write("Foto: " );
                                string foto = Console.ReadLine();

                                perfilService.AtualizarPerfil(facebook, instagram, bio,foto);
                            }
                            else if (opcao == "0")
                            {
                                Console.WriteLine("Logout realizado.");
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Opção inválida.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Email ou senha inválidos.");
                    }

                    Console.WriteLine("Deseja tentar login novamente? (s/n)");
                    string tentar = Console.ReadLine().Trim().ToLower();

                    if (tentar == "n" || tentar == "nao")
                    {
                        break;
                    }
                }
            }

            // ===== SALVAR AO FINAL =====
            ArquivoService arquivoFinal = new ArquivoService();
            arquivoFinal.Salvar(usuarioService.ObterTodos());

            Console.WriteLine("Dados salvos. Encerrando sistema...");
        }
      
    }
}
*/