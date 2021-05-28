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

        /*Al hacer click en el boton "Guardar" se ejecuta el metodo para conseguir los datos tecleados del distribuidor,
        pasarlos a su constructor y ejecutar el procedimiento almacenado para insertarlo en la base de datos.*/
        private void GuardarBtn_Click(object sender, RoutedEventArgs e)
        {
            SqlConexion sql = new SqlConexion();
            try
            {
                DistribuidorModelo distribuidor = new DistribuidorModelo(IdDistribuidorText.Text, DateTime.Now, NombresText.Text, ApellidoPaternoText.Text,
                                                                         ApellidoMaternoText.Text, CalleText.Text, Convert.ToInt32(NumeroCasaText.Text), ColoniaText.Text);
                sql.InsertarDistribuidor(distribuidor);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //Al hacer click en el boton "Consultar" se muestra la ventana para consultar el ID del distribuidor.
        private void ConsultarBtn_Click(object sender, RoutedEventArgs e)
        {
            ConsultarDistribuidor consultar = new ConsultarDistribuidor();
            consultar.Show();
        }
    }
}
