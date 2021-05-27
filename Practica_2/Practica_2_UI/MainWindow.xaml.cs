using System;
using System.Windows;
using Practica_2_Biblioteca;

namespace Practica_2_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GuardarBtn_Click(object sender, RoutedEventArgs e)
        {
            SqlConexion sql = new SqlConexion();
            try
            {
                DistribuidorModelo distribuidor = new DistribuidorModelo(IdDistribuidorText.Text, DateTime.Now, NombresText.Text, ApellidoPaternoText.Text, ApellidoMaternoText.Text, CalleText.Text, Convert.ToInt32(NumeroCasaText.Text), ColoniaText.Text);
                sql.InsertarDistribuidor(distribuidor);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
