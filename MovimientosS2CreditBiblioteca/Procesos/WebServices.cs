using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RestSharp;
using MovimientosS2CreditBiblioteca.Modelos;

namespace MovimientosS2CreditBiblioteca.Procesos
{

    //TODO - Nuevo modulo para buscar limite y saldo disponible de cliente.

    //TODO - En el mismo modulo de devolución que pregunte si se desea generar un revale a partir de la devolución.

    //TODO - Que se muestren las devoluciones que tiene el vale en el modulo de generar revale y que muestre si ya tiene un revale generado de esa devolución.
    public class WebServices
    {

        /*Metodo para enviar un request al cliente el cual toma el folio de un vale, lo inserta en la estructura de un cuerpo para el request
          y posteriormente se ejecuta, devolviendo una respuesta Json del web service "Buscar vale".*/
        public dynamic BuscarVale(string folioVale)
        {
            var clienteRest = new RestClient("http://10.200.3.103:7082/pos/s2credit");
            var requestBuscarVale = new RestRequest(Method.POST);

            string cuerpoRequestBuscarVale = "{\"coupon-search\":{\"coupon\":\"" + folioVale + "\"}}";
            requestBuscarVale.AddHeader("content-type", "application/json; charset=utf-8");
            requestBuscarVale.AddParameter("application/json", cuerpoRequestBuscarVale, ParameterType.RequestBody);
            requestBuscarVale.AddHeader("Authorization", null);

            IRestResponse respuestaBuscarVale = clienteRest.Execute(requestBuscarVale);
            dynamic respuestaBuscarValeJson = JObject.Parse(respuestaBuscarVale.Content);

            return respuestaBuscarValeJson;

        }

        /*Metodo para enviar un request al cliente el cual toma el numero de un cliente, lo inserta en la estructura de un cuerpo para el request
          y posteriormente se ejecuta, devolviendo una respuesta Json del web service "Buscar vales de cliente".*/
        public List<ValesClienteModelo> BuscarValesCliente(string numeroCliente)
        {
            var clienteRest = new RestClient("http://10.200.3.103:7081/pos/s2credit-portal/customer/search-coupons");
            var requestBuscarValesCliente = new RestRequest(Method.POST);

            string cuerpoRequestBuscarValesCliente = "{\"number\":\"" + numeroCliente + "\",\"name\":\"\",\"coupon\":\"\",\"limit\":\"\",\"page\":\"\"}";
            requestBuscarValesCliente.AddHeader("content-type", "application/json; charset=utf-8");
            requestBuscarValesCliente.AddParameter("application/json", cuerpoRequestBuscarValesCliente, ParameterType.RequestBody);
            requestBuscarValesCliente.AddHeader("Authorization", null);

            IRestResponse respuestaBuscarValesCliente = clienteRest.Execute(requestBuscarValesCliente);
            IList<JToken> respuesta = JObject.Parse(respuestaBuscarValesCliente.Content);

            //Condicional if para validar si el cliente no existe o si no se encuentra pagando vales.
            if(((JProperty)respuesta[1]).Value.ToString() == "0")
            {
                return null;
            }

            var info = ((JProperty)respuesta[2]).Children();
            List<ValesClienteModelo> valesCliente = new List<ValesClienteModelo>();

            //Tres foreach anidados para sacar los valores anidados de la respuesta json y e ingresarse en un metodo constructor. 
            foreach (JToken obj in info)
            {
                foreach (JToken obj2 in obj)
                {
                    foreach(JToken obj3 in obj2)
                    {  
                        valesCliente.Add(new ValesClienteModelo()
                        {
                            FolioVale = obj3["id_coupon"].ToString(),
                            TipoVale = obj3["charge_type"].ToString(),
                            FechaCanje = obj3["date"].ToString(),
                            Estatus = obj3["type"].ToString(),
                            Quincenas = obj3["number"].ToString(),
                            NombreCliente = obj3["customer"]["customer"].ToString()
                        });
                    }
                }
            }

            return valesCliente;

        }

        /*Metodo para enviar un request al cliente el cual toma el folio de un vale, consigue la información necesaria para generar el revale,
          posteriormente se ejecuta, generando el revale en S2Credit y devolviendo una respuesta Json del web service "Generar revale".*/
        public string GenerarRevale(string folioVale, bool esDevolucion, decimal montoRevaleDevolucion)
        {

            dynamic respuestaBuscarValeJson = BuscarVale(folioVale);
            int numeroCanjeante;
            //En base a la respuesta Json del request buscar vale, se consigue el numero del canjeante para ser utilizado en el cuerpo del request generar revale.
                try
                {
                    numeroCanjeante = Convert.ToInt32(respuestaBuscarValeJson.coupon.idCustomer);
                }
                catch
                {
                    return respuestaBuscarValeJson.ToString();
                }

    //Dependiendo del tipo de revale que se haya seleccionado, se genera un cuerpo
            string cuerpoRequestGenerarRevale = "";
                if (esDevolucion == false)
                {
                    //Se resta el monto total del vale menos el monto utilizado en la compra para conseguir el restante del vale, el cual se utilizará para generar el revale de la compra
                    decimal montoRevaleCompra = Convert.ToDecimal(respuestaBuscarValeJson.coupon.amount) - Convert.ToDecimal(respuestaBuscarValeJson.coupon.purchasedPmount);

                    cuerpoRequestGenerarRevale = "{\"new-coupon\":{\"idCoupon\":\"" + folioVale + "\",\"idCustomer\":\"" + numeroCanjeante + "\",\"amount\":\"" + montoRevaleCompra + "\",\"type\":\"1\"}}";
                }
                else if (esDevolucion == true)
                {
                    cuerpoRequestGenerarRevale = "{\"new-coupon\":{\"idCoupon\":\"" + folioVale + "\",\"idCustomer\":\"" + numeroCanjeante + "\",\"amount\":\"" + montoRevaleDevolucion + "\",\"type\":\"2\"}}";
                }

            var clienteRest = new RestClient("http://10.200.3.103:7082/pos/s2credit");
            var requestGenerarRevale = new RestRequest(Method.POST);
            requestGenerarRevale.AddHeader("content-type", "application/json; charset=utf-8");
            requestGenerarRevale.AddParameter("application/json", cuerpoRequestGenerarRevale, ParameterType.RequestBody);
            requestGenerarRevale.AddHeader("Authorization", null);

            IRestResponse respuestaGenerarRevale = clienteRest.Execute(requestGenerarRevale);
            dynamic respuestaGenerarRevaleJson = JObject.Parse(respuestaGenerarRevale.Content);

            //Se estructura el folio del revale completo generado capturandolo desde la respuesta Json

            string folioRevale;
            try
            {
                folioRevale = "00" + respuestaGenerarRevaleJson.coupon.id.ToString();
            }
            catch
            {
                return respuestaGenerarRevaleJson.ToString();
            }

            return respuestaGenerarRevaleJson.ToString();
        }

        /*Metodo para enviar un request al cliente el cual toma el folio de un vale, toma la información necesaria para insertar un beneficiario,
          luego lo inserta en un cuerpo para el request y se ejecuta.*/
        public string InsertarBeneficiario(string folioVale, string nombres, string primerApellido, string segundoApellido, string parentesco)
        {
            dynamic respuestaBuscarValeJson = BuscarVale(folioVale);
            if(respuestaBuscarValeJson.insurance == null)
            {
                return "No se está pagando seguro en el vale";
            }
            string idPurchase;

            //A partir de la respuesta Json de la busqueda del vale se consigue el idpurchase requerido en el cuerpo request para insertar el beneficiario.
            try
            {
                idPurchase = respuestaBuscarValeJson.insurance[0]["id_purchase"].ToString();
            }
            catch
            {
                return respuestaBuscarValeJson.ToString();
            }
            var clienteRest = new RestClient("http://10.200.3.103:7082/pos/s2credit");
            var requestInsertarBeneficiario = new RestRequest(Method.POST);

            //parentesco = parentesco.Remove(parentesco.IndexOf("."));

            string cuerpoRequestInsertarBeneficiario = "{\"beneficiary-change\":{\"id_relationship\":\"" + parentesco + "\",\"id_purchase\":\"" + idPurchase + "\",\"name\":\"" + nombres + "\",\"last_name\":\"" + primerApellido + "\",\"second_last_name\":\"" + segundoApellido + "\"}}";
            requestInsertarBeneficiario.AddHeader("content-type", "application/json; charset=utf-8");
            requestInsertarBeneficiario.AddParameter("application/json", cuerpoRequestInsertarBeneficiario, ParameterType.RequestBody);
            requestInsertarBeneficiario.AddHeader("Authorization", null);

            IRestResponse respuestaInsertarBeneficiario = clienteRest.Execute(requestInsertarBeneficiario);
            dynamic respuestaInsertarBeneficiarioJson = JObject.Parse(respuestaInsertarBeneficiario.Content);

            return respuestaInsertarBeneficiarioJson.ToString();
        }

        /*Metodo para enviar un request al cliente el cual toma el folio de un vale, toma la información necesaria para generar una devolución,
          luego lo inserta en un cuerpo para el request y se ejecuta.*/
        public string GenerarDevolucion(string folioVale, decimal montoDevolucion)
        {
            dynamic respuestaBuscarValeJson = BuscarVale(folioVale);

            //A partir de la respuesta Json de la busqueda del vale se consigue el idpurchase requerido en el cuerpo request para insertar el beneficiario.
            string distribuidor;
            try
            {
                distribuidor = respuestaBuscarValeJson.distributor.number.ToString();
            }
            catch
            {
                return respuestaBuscarValeJson.ToString();
            }
            var clienteRest = new RestClient("http://10.200.3.103:7082/pos/s2credit");
            var requestGenerarDevolucion = new RestRequest(Method.POST);

            string cuerpoRequestGenerarDevolucion = "{\"devolution\":{\"distributor_number\":\"" + distribuidor + "\",\"id_coupon\":\"" + folioVale + "\",\"amount\":\"" + montoDevolucion + "\"}}";
            requestGenerarDevolucion.AddHeader("content-type", "application/json; charset=utf-8");
            requestGenerarDevolucion.AddParameter("application/json", cuerpoRequestGenerarDevolucion, ParameterType.RequestBody);
            requestGenerarDevolucion.AddHeader("Authorization", null);

            IRestResponse responseGenerarDevolucion = clienteRest.Execute(requestGenerarDevolucion);
            dynamic respuestaGenerarDevolucionJson = JObject.Parse(responseGenerarDevolucion.Content);

            return respuestaGenerarDevolucionJson.ToString();
        }
    }
}
