using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProjetoAcelera.Services;


namespace ProjetoAcelera.Views.Cadastro
{
    public partial class TelaCadastro : Window
    {
        private UsuarioService usuarioService;

        public TelaCadastro(UsuarioService service)
        {
            InitializeComponent();
            this.usuarioService = service;
        }


        private void BtnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            string nome = txtNome.Text;
            string email = txtEmail.Text;
            string senha = txtSenha.Password;

            usuarioService.Cadastrar(nome, senha, email);

            MessageBox.Show("Cadastro feito!");
        }

        private void BtnVoltar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}