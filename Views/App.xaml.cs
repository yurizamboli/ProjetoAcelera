using System.Configuration;
using System.Data;
using System.Windows;

namespace ProjetoAcelera
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        [System.STAThread]
        public static void Main()
        {
            var app = new App();
            app.Run();
        }
    }
}