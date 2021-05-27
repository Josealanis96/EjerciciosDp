using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica_2_Biblioteca
{
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

        public DistribuidorModelo(string nombre_Completo, string calle, int numero_de_casa, string colonia)
        {
            Nombres = nombre_Completo;
            Calle = calle;
            NumeroCasa = numero_de_casa;
            Colonia = colonia;
        }

        public DistribuidorModelo(string distribuidorId)
        {
            DistribuidorId = distribuidorId;
        }
    }
}
