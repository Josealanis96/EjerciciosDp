using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MovimientosS2CreditBiblioteca.Procesos;
using MovimientosS2CreditBiblioteca.Modelos;
using System.Net;

namespace MovimientosS2CreditUI
{
    /// <summary>
    /// Interaction logic for MovimientosS2Credit.xaml
    /// </summary>
    ///

    //Folio de vale con ejemplo de vale que no está pagando seguro: WFHUK57JUN.

    public partial class MovimientosS2Credit : Window
    {
        //Se inicializa una instancia de la clase WebServices para ser utilizada a lo largo del uso de la aplicación.
        WebServices consumoWebServices = new WebServices();
        public MovimientosS2Credit()
        {
            InitializeComponent();
        }

        //Metodo EventHandler en el que se vuelven visibles o se esconden las diversas interfaces y los elementes dentro de estas interfaces para cada tipo de movimiento S2Credit de acuerdo a la selección del Combobox de este. 
        private void MovimientosDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FolioValeLabel.Visibility = Visibility.Hidden;
            FolioValeText.Visibility = Visibility.Hidden;
            BuscarValeGrid.Visibility = Visibility.Hidden;
            RespuestaBuscarValeGrid.Visibility = Visibility.Hidden;
            BuscarValesClienteGrid.Visibility = Visibility.Hidden;
            RespuestaBuscarValesClienteGrid.Visibility = Visibility.Hidden;
            GenerarRevaleGrid.Visibility = Visibility.Hidden;
            RespuestaGenerarRevaleGrid.Visibility = Visibility.Hidden;
            BeneficiarioGrid.Visibility = Visibility.Hidden;
            RespuestaInsertarBeneficiarioGrid.Visibility = Visibility.Hidden;
            GenerarDevolucioGrid.Visibility = Visibility.Hidden;
            RespuestaGenerarDevolucionGrid.Visibility = Visibility.Hidden;

            if (MovimientosDropDown.SelectedIndex == 0)
            {
                FolioValeLabel.Visibility = Visibility.Visible;
                FolioValeText.Visibility = Visibility.Visible;
                BuscarValeGrid.Visibility = Visibility.Visible;
                RespuestaBuscarValeGrid.Visibility = Visibility.Visible;
            }

            if (MovimientosDropDown.SelectedIndex == 1)
            {
                BuscarValesClienteGrid.Visibility = Visibility.Visible;
            }

            if (MovimientosDropDown.SelectedIndex == 2)
            {
                FolioValeLabel.Visibility = Visibility.Visible;
                FolioValeText.Visibility = Visibility.Visible;
                GenerarRevaleGrid.Visibility = Visibility.Visible;
                RespuestaGenerarRevaleGrid.Visibility = Visibility.Visible;
                RevaleCompraRadio.IsChecked = true;
            }

            if (MovimientosDropDown.SelectedIndex == 3)
            {
                FolioValeLabel.Visibility = Visibility.Visible;
                FolioValeText.Visibility = Visibility.Visible;
                BeneficiarioGrid.Visibility = Visibility.Visible;
                RespuestaInsertarBeneficiarioGrid.Visibility = Visibility.Visible;
                ParentescoBeneficiarioDropdown.SelectedItem = null;
            }

            if (MovimientosDropDown.SelectedIndex == 4)
            {
                FolioValeLabel.Visibility = Visibility.Visible;
                FolioValeText.Visibility = Visibility.Visible;
                GenerarDevolucioGrid.Visibility = Visibility.Visible;
                RespuestaGenerarDevolucionGrid.Visibility = Visibility.Visible;
            }
        }

        //Metodo EventHandler utilizado para habilitar el textbox del monto del revale por devolución en el caso de seleccionar el RadioButton de devolución.
        private void RevaleDevoluciónRadio_Checked(object sender, RoutedEventArgs e)
        {
            MontoRevaleDevolucionText.IsEnabled = true;
        }

        //Metodo EventHandler utilizado para deshabilitar el textbox del monto del revale por devolución en el caso de seleccionar el RadioButton de revale por compra.
        private void RevaleCompraRadio_Checked(object sender, RoutedEventArgs e)
        {
            MontoRevaleDevolucionText.IsEnabled = false;
        }

        /*Metodo EventHandler utilizado para consumir el web service "Buscar vale" a partir del folio del vale capturado en el textbox, el resultado de este consumo
          del web service es almacenado en una variable dinamica para ser utilizados sus valores requeridos en los campos correspondientes.*/
        private void BuscarValeBoton_Click(object sender, RoutedEventArgs e)
        {
            bool camposValidos = ValidarCamposBuscarVale();
            bool estatusConexion = ProbarConexionCliente();

            if (camposValidos && estatusConexion == true)
            {
                dynamic respuestaBuscarValeJson;
                respuestaBuscarValeJson = consumoWebServices.BuscarVale(FolioValeText.Text);
                try
                {
                    DisponibleCalzadoText.Text = "$" + respuestaBuscarValeJson.credit.availables["1"].ToString();
                    DisponibleFinancieroText.Text = "$" + respuestaBuscarValeJson.credit.availables["2"].ToString();
                }
                catch
                {
                    MessageBox.Show(respuestaBuscarValeJson.ToString());
                }
            }
        }

        private void BuscarValesClienteBoton_Click(object sender, RoutedEventArgs e)
        {
            ValesClienteDataGrid.ItemsSource = null;
            bool camposValidos = ValidarCamposBuscarValesCliente();
            bool estatusConexion = ProbarConexionCliente();

            List<ValesClienteModelo> valesCliente = new List<ValesClienteModelo>();

            if (camposValidos && estatusConexion == true)
            {
                valesCliente = consumoWebServices.BuscarValesCliente(NumeroClienteText.Text);
                if(valesCliente == null)
                {
                    NombreClienteLabel.Content = "El numero de cliente no existe o el cliente no se encuentra pagando vales";
                    RespuestaBuscarValesClienteGrid.Visibility = Visibility.Visible;
                    return;
                }
                ValesClienteDataGrid.ItemsSource = valesCliente;
                NombreClienteLabel.Content = "Nombre del cliente: " + valesCliente[0].NombreCliente.ToString();
                RespuestaBuscarValesClienteGrid.Visibility = Visibility.Visible;
            }
        }

        /*Metodo EventHandler utilizado para consumir el web service "Generar revale", se toma el folio del vale de su campo de texto y el monto del revale en el caso
          de ser un revale de una devolución, y se muestra el resultado del metodo en un visualizador de HTML incluido en el UI.*/
        private void GenerarRevaleBoton_Click(object sender, RoutedEventArgs e)
        {
            bool camposValidos = ValidarCamposGenerarRevale();
            bool estatusConexion = ProbarConexionCliente();

            if (camposValidos && estatusConexion == true)
            {
                //Se insertó la declaración del monto devolución text porque no deja correr el metodo si no tiene un valor.
                if (MontoRevaleDevolucionText.Text == "")
                {
                    MontoRevaleDevolucionText.Text = "0";
                }

                    string respuestaGenerarRevaleJson = consumoWebServices.GenerarRevale(FolioValeText.Text.ToString(), RevaleDevoluciónRadio.IsChecked.Value, Convert.ToDecimal(MontoRevaleDevolucionText.Text)).ToString();
                    RespuestaRevaleLabel.Content = respuestaGenerarRevaleJson;
            }
        }

        /*Metodo EventHandler utilizado para consumir el web service "Insertar beneficiario", se toman los datos requeridos del beneficiario(Folio del vale, nombres, apellidos y parentesco)
          y se muestra la confirmación del proceso por medio de un label.*/
        private void InsertarBeneficiarioBoton_Click(object sender, RoutedEventArgs e)
        {
            bool camposValidos = ValidarCamposInsertarBeneficiario();
            bool estatusConexion = ProbarConexionCliente();

            if (camposValidos && estatusConexion == true)
            {
                string respuestaInsertarBeneficiarioJson = consumoWebServices.InsertarBeneficiario(FolioValeText.Text, NombresBeneficiarioText.Text, PrimerApellidoBeneficiarioText.Text, SegundoApellidoBeneficiarioText.Text, ParentescoBeneficiarioDropdown.Text);
                RespuestaBeneficiarioLabel.Content = respuestaInsertarBeneficiarioJson;
            }
        }

        /*Metodo EventHandler utilizado para consumir el web service "Generar devolucion", se toma el folio del vale y el monto de la devolución
          y se muestra la confirmación del proceso por medio de un label.*/
        private void GenerarDevolucionBoton_Click(object sender, RoutedEventArgs e)
        {
            bool camposValidos = ValidarCamposGenerarDevolucion();
            bool estatusConexion = ProbarConexionCliente();

            if (camposValidos && estatusConexion == true)
            {
                string respuestaGenerarDevolucionJson = consumoWebServices.GenerarDevolucion(FolioValeText.Text, Convert.ToDecimal(MontoDevolucionText.Text));

                //Tras consumir el web service se deja en blanco el campo del monto de la devolución para no generar devoluciones erroneas al usar el programa.
                MontoRevaleDevolucionText.Text = "";

                RespuestaDevolucionLabel.Content = respuestaGenerarDevolucionJson;
            }
        }

        private bool ValidarCamposBuscarVale()
        {
            bool camposValidos = true;

            if(FolioValeText.Text == "")
            {
                camposValidos = false;
                MessageBox.Show("Ingresar un folio de vale");
            }

            return camposValidos;
        }

        private bool ValidarCamposBuscarValesCliente()
        {
            bool camposValidos = true;

            if (NumeroClienteText.Text == "")
            {
                camposValidos = false;
                MessageBox.Show("Ingresar un numero de cliente");
            }

            return camposValidos;
        }


        private bool ValidarCamposGenerarRevale()
        {
            bool camposValidos = true;

            if(FolioValeText.Text == "")
            {
                camposValidos = false;
                MessageBox.Show("Ingresar un folio de vale");
            }

            if(RevaleDevoluciónRadio.IsChecked == true)
            {
                if(MontoRevaleDevolucionText.Text == "")
                {
                    camposValidos = false;
                    MessageBox.Show("Ingresar un monto para el revale por devolución");
                }
            }

            return camposValidos;
        }

        private bool ValidarCamposInsertarBeneficiario()
        {
            bool camposValidos = true;

            if(FolioValeText.Text == "")
            {
                camposValidos = false;
                MessageBox.Show("Ingresar un folio de vale");
            }

            if(NombresBeneficiarioText.Text == "")
            {
                camposValidos = false;
                MessageBox.Show("Ingresar los nombres del beneficiario");
            }

            if(PrimerApellidoBeneficiarioText.Text == "")
            {
                camposValidos = false;
                MessageBox.Show("Ingresar el primer apellido del beneficiario");
            }

            if(SegundoApellidoBeneficiarioText.Text == "")
            {
                camposValidos = false;
                MessageBox.Show("Ingresar el segundo apellido del beneficiario");
            }

            if(ParentescoBeneficiarioDropdown.SelectedIndex == -1)
            {
                camposValidos = false;
                MessageBox.Show("Ingresar el parentesco del beneficiario");
            }

            return camposValidos;
        }

        private bool ValidarCamposGenerarDevolucion()
        {
            bool camposValidos = true;

            if (FolioValeText.Text == "")
            {
                camposValidos = false;
                MessageBox.Show("Ingresar un folio de vale");
            }

            if(MontoDevolucionText.Text == "")
            {
                camposValidos = false;
                MessageBox.Show("Ingresar un monto de la devolución");
            }

            return camposValidos;
        }

        private bool ProbarConexionCliente()
        {
            bool estatusConexion = true;
            var request = (HttpWebRequest)WebRequest.Create("http://10.200.3.103:7082/pos/s2credit");
            request.Timeout = 1000;
            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch
            {
                if (response == null)
                {
                    estatusConexion = false;
                    MessageBox.Show("No se pudo realizar la conexión a S2Credit");
                }
            }
            return estatusConexion;
        }
    }
}

