using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica_2_Biblioteca
{
    public class DistribuidorModelo
    {
        public string IdDistribuidor { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Calle { get; set; }
        public int NumeroCasa { get; set; }
        public string Colonia { get; set; }

        public DistribuidorModelo(string idDistribuidor, DateTime fechaRegistro, string nombres, string apellidoPaterno, string apellidoMaterno, string calle, int numeroCasa, string colonia)
        {
            IdDistribuidor = idDistribuidor;
            FechaRegistro = fechaRegistro;
            Nombres = nombres;
            ApellidoPaterno = apellidoPaterno;
            ApellidoMaterno = apellidoMaterno;
            Calle = calle;
            NumeroCasa = numeroCasa;
            Colonia = colonia;
        }
    }
}
