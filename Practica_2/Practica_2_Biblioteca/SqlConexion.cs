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
                p.Add("@DistribuidorId", distribuidor.DistribuidorId);
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

            public IList<DistribuidorModelo> ConsultarDistribuidor(DistribuidorModelo distribuidor)
            {
                IList<DistribuidorModelo> list;
                using (IDbConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["Practica#2"].ToString()))
                {
                    var p = new DynamicParameters();
                    p.Add("@DistribuidorId", distribuidor.DistribuidorId);
                    list = conexion.Query<DistribuidorModelo>("dbo.spDistribuidor_Consultar", p, commandType: CommandType.StoredProcedure).ToList();
                }
                return list;
            }
    }
}
