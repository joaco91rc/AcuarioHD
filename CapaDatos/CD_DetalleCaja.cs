using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_DetalleCaja
    {

        public List<DetalleCaja> DetalleCaja(string fechaConsulta)
        {
            List<DetalleCaja> lista = new List<DetalleCaja>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {

                    StringBuilder query = new StringBuilder();

                    SqlCommand cmd = new SqlCommand("SP_DETALLECAJA", oconexion);
                    cmd.Parameters.AddWithValue("fechaConsulta", fechaConsulta);
                   
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new DetalleCaja()
                            {
                                fechaApertura = dr["fechaApertura"].ToString(),
                                hora = dr["hora"].ToString(),
                                tipoTransaccion = dr["tipoTransaccion"].ToString(),
                                monto = Convert.ToDecimal(dr["monto"].ToString()),
                                docAsociado = dr["docAsociado"].ToString(),
                                usuarioTransaccion = dr["usuarioTransaccion"].ToString()
                               

                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    lista = new List<DetalleCaja>();
                }

          }
            return lista;


        }

        public List<DetalleCaja> Listar(string fecha)
        {
            List<DetalleCaja> lista = new List<DetalleCaja>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select cr.fechaApertura,tc.hora,tc.tipoTransaccion,tc.monto,tc.docAsociado,tc.usuarioTransaccion from CAJA_REGISTRADORA cr");
                    query.AppendLine("inner join TRANSACCION_CAJA tc  on tc.idCajaRegistradora = cr.idCajaRegistradora");
                    query.AppendLine("where CONVERT(DATE, cr.fechaApertura) = @fecha");
                    
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@fecha", fecha);
                    cmd.CommandType = CommandType.Text;

                    
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new DetalleCaja()
                            {
                                fechaApertura = dr["fechaApertura"].ToString(),
                                hora = dr["hora"].ToString(),
                                tipoTransaccion = dr["tipoTransaccion"].ToString(),
                                monto = Convert.ToDecimal(dr["monto"].ToString()),
                                docAsociado = dr["docAsociado"].ToString(),
                                usuarioTransaccion = dr["usuarioTransaccion"].ToString()


                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    lista = new List<DetalleCaja>();
                }

            }
            return lista;
        }


    }  
    
}
