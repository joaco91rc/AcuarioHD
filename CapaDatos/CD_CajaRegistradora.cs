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
    public class CD_CajaRegistradora
    {


        public int AperturaCaja(CajaRegistradora objCajaRegistradora, out string mensaje)
        {
            int idCajaGenerado = 0;
            mensaje = string.Empty;

            try
            {



                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_APERTURACAJA", oconexion);
                    cmd.Parameters.AddWithValue("usuarioAperturaCaja", objCajaRegistradora.usuarioAperturaCaja);
                    cmd.Parameters.AddWithValue("estado", 1);
                    cmd.Parameters.AddWithValue("saldo", objCajaRegistradora.saldo);


                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    idCajaGenerado = Convert.ToInt32(cmd.Parameters["resultado"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();


                }

            }

            catch (Exception ex)
            {
                idCajaGenerado = 0;
                mensaje = ex.Message;

            }


            return idCajaGenerado;

        }


        public List<CajaRegistradora> Listar()
        {
            List<CajaRegistradora> lista = new List<CajaRegistradora>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select * FROM CAJA_REGISTRADORA");
                   
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new CajaRegistradora()
                            {
                                idCajaRegistradora = Convert.ToInt32(dr["idCajaRegistradora"]),
                                fechaApertura = dr["fechaApertura"].ToString(),
                                fechaCierre = dr["fechaCierre"].ToString(),
                                usuarioAperturaCaja = dr["usuarioAperturaCaja"].ToString(),
                                saldo = Convert.ToDecimal(dr["saldo"]),
                                estado = Convert.ToBoolean(dr["estado"])

                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    lista = new List<CajaRegistradora>();
                }

            }
            return lista;
        }



        public bool CerrarCaja(CajaRegistradora objCajaRegistradora, out string mensaje)
        {
            bool respuesta = false;
            mensaje = string.Empty;

            try
            {



                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_CERRARCAJA", oconexion);
                    cmd.Parameters.AddWithValue("idCajaRegistradora", objCajaRegistradora.idCajaRegistradora);
                    cmd.Parameters.AddWithValue("fechaCierre", Convert.ToDateTime(objCajaRegistradora.fechaCierre));
                    cmd.Parameters.AddWithValue("saldo", objCajaRegistradora.saldo);
                    

                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();


                }

            }

            catch (Exception ex)
            {
                respuesta = false;
                mensaje = ex.Message;

            }


            return respuesta;

        }



    }
}
