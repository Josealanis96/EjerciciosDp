using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace Practica_2_Biblioteca
{
    public class SqlConexion
    {
        public void InsertarDistribuidor(DistribuidorModelo distribuidor)
        {
            using (IDbConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["Practica#2"].ToString()))
            {
                var p = new DynamicParameters();
                p.Add("@IdDistribuidor", distribuidor.IdDistribuidor);
                p.Add("@FechaRegistro", distribuidor.FechaRegistro);
                p.Add("@Nombres", distribuidor.Nombres);
                p.Add("@ApellidoPaterno", distribuidor.ApellidoPaterno);
                p.Add("@ApellidoMaterno", distribuidor.ApellidoMaterno);
                p.Add("@Calle", distribuidor.Calle);
                p.Add("@NumeroCasa", distribuidor.NumeroCasa);
                p.Add("@Colonia", distribuidor.Colonia);

                conexion.Execute("dbo.spDistribuidor_Insertar", p, commandType: CommandType.StoredProcedure);
            }

        }
    }
}
