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
    public class CD_Transaccion
    {

        public List<TransaccionCaja> Listar(int idCajaRegistradora)
        {
            List<TransaccionCaja> lista = new List<TransaccionCaja>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT * FROM TRANSACCION_CAJA WHERE idCajaRegistradora = @idCajaRegistradora");
                    
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("idCajaRegistradora", idCajaRegistradora);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new TransaccionCaja()
                            {
                                idTransaccion = Convert.ToInt32(dr["idTransaccion"]),
                                idCajaRegistradora = Convert.ToInt32(dr["idCajaRegistradora"]),
                                hora = dr["hora"].ToString(),
                                tipoTransaccion = dr["tipoTransaccion"].ToString(),
                                monto = Convert.ToDecimal(dr["monto"]),
                                docAsociado = dr["docAsociado"].ToString(),
                                usuarioTransaccion = dr["usuarioTransaccion"].ToString(),


                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    lista = new List<TransaccionCaja>();
                }

            }
            return lista;
        }

        public int RegistrarMovimiento(TransaccionCaja objTransaccion, out string mensaje)
        {
            int idTransaccionGenerado = 0;
            mensaje = string.Empty;

            try
            {



                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARMOVIMIENTO", oconexion);
                    cmd.Parameters.AddWithValue("idCajaRegistradora", objTransaccion.idCajaRegistradora);
                    cmd.Parameters.AddWithValue("tipoTransaccion", objTransaccion.tipoTransaccion);
                    cmd.Parameters.AddWithValue("monto", objTransaccion.monto);
                    cmd.Parameters.AddWithValue("docAsociado", objTransaccion.docAsociado);
                    

                    cmd.Parameters.AddWithValue("usuarioTransaccion", objTransaccion.usuarioTransaccion);
                    cmd.Parameters.AddWithValue("fecha", Convert.ToDateTime(objTransaccion.hora));

                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    idTransaccionGenerado = Convert.ToInt32(cmd.Parameters["resultado"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();


                }

            }

            catch (Exception ex)
            {
                idTransaccionGenerado = 0;
                mensaje = ex.Message;

            }


            return idTransaccionGenerado;

        }

    }
}
