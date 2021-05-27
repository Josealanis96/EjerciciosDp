using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using Practica_2_Biblioteca;

namespace Practica_2_UI
{
    /// <summary>
    /// Interaction logic for ConsultarDistribuidor.xaml
    /// </summary>
    public partial class ConsultarDistribuidor : Window
    {
        public ConsultarDistribuidor()
        {
            InitializeComponent();
        }

        private void BuscarBtn_Click(object sender, RoutedEventArgs e)
        {
            SqlConexion sql = new SqlConexion();
            try
            {
                DistribuidorModelo distribuidor = new DistribuidorModelo(DistribuidorIdText.Text);
                DistribuidorDatagrid.ItemsSource = sql.ConsultarDistribuidor(distribuidor);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
