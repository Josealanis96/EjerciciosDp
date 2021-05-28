using System;

namespace Practica_2_Biblioteca
{
    //Propiedades pertenecientes al modelo del distribuidor.
    public class DistribuidorModelo
    {
        public string DistribuidorId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Calle { get; set; }
        public int NumeroCasa { get; set; }
        public string Colonia { get; set; }

        //Metodo constructor utilizado para insertar un distribuidor nuevo.
        public DistribuidorModelo(string distribuidorId, DateTime fechaRegistro, string nombres, string apellidoPaterno, string apellidoMaterno, string calle, int numeroCasa, string colonia)
        {
            DistribuidorId = distribuidorId;
            FechaRegistro = fechaRegistro;
            Nombres = nombres;
            ApellidoPaterno = apellidoPaterno;
            ApellidoMaterno = apellidoMaterno;
            Calle = calle;
            NumeroCasa = numeroCasa;
            Colonia = colonia;
        }


        //Metodo constructor utilizado para consultar un distribuidor por su id.
        public DistribuidorModelo(string distribuidorId)
        {
            DistribuidorId = distribuidorId;
        }

        //Metodo constructor utilizado para capturar los datos devueltos por el procedimiento almacenado de la consulta.
        public DistribuidorModelo(string nombre_Completo, string calle, int numero_de_casa, string colonia)
        {
            Nombres = nombre_Completo;
            Calle = calle;
            NumeroCasa = numero_de_casa;
            Colonia = colonia;
        }
    }
}
