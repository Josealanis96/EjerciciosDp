using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace Practica_2_Biblioteca
{
    public class SqlConexion
    {

        //Metodo para insertar un distribuidor nuevo tomando los valores en las propiedades del constructor y ejecutandolos en el procedimiento almacenado en bd.
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

        //Metodo para consultar un distribuidor existente tomando el valor id en las propiedades del constructor y ejecutandolos en el procedimiento almacenado en bd.
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
