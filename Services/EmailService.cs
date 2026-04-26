using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjetoAcelera.Services
{
    public class EmailService
    {
       
        public bool EnviarTokenPorEmail(string emailDestino, string token)
        {
            try
            {
                var remetente = "projetoalahc@gmail.com";
                var senha = "SENHA_DE_APP_AQUI"; 
                // SÓ COLOCAR A SENHA PARA TESTAR LOCALMENTE, DEPOIS APAGAR E SUBIR O CÓDIGO SEM A SENHA, PARA EVITAR VAZAMENTO DE CREDENCIAIS. O IDEAL É USAR VARIÁVEIS DE AMBIENTE OU UM ARQUIVO DE CONFIGURAÇÃO QUE NÃO SEJA COMMITADO NO GITHUB.

                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(remetente, senha),
                    EnableSsl = true
                };

                var mensagem = new MailMessage();
                mensagem.From = new MailAddress(remetente, "Acelera Suporte");
                mensagem.To.Add(emailDestino);
                mensagem.Subject = "Redefinição de Senha";
                mensagem.Body = $"Olá,\n\nUse o código abaixo para redefinir sua senha:\n\n{token}\n\nEste código expira em alguns minutos.\n\nSe você não solicitou, ignore este email.";

                smtp.Send(mensagem);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao enviar email: " + ex.Message);
                return false;
            }
        }
    }
}
