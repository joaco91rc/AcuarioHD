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
    public class CD_Negocio
    {

        public Negocio ObtenerDatos()
        {
            Negocio objNegocio = new Negocio();

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();
                    string query = "select idNegocio,nombre,CUIT,direccion from NEGOCIO where idNegocio =1";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {

                        while (dr.Read())
                        {
                            objNegocio = new Negocio()
                            {
                                idNegocio = int.Parse(dr["idNegocio"].ToString()),
                                nombre = dr["nombre"].ToString(),
                                CUIT = dr["CUIT"].ToString(),
                                direccion = dr["direccion"].ToString()
                            };
                        }
                    }


                }


            }

            catch
            {
                objNegocio = new Negocio();

            }

            return objNegocio;
        }

        public bool Guardardatos(Negocio objeto, out string mensaje)
        {
            mensaje = string.Empty;
            bool respuesta = true;

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();


                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update NEGOCIO SET nombre = @nombre,");
                    query.AppendLine("cuit = @cuit,");
                    query.AppendLine("direccion = @direccion");
                    query.AppendLine("where idNegocio = 1;");
                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@nombre", objeto.nombre);
                    cmd.Parameters.AddWithValue("@cuit", objeto.CUIT);
                    cmd.Parameters.AddWithValue("@direccion", objeto.direccion);
                    cmd.CommandType = CommandType.Text;

                    if (cmd.ExecuteNonQuery() < 1)
                    {

                        mensaje = "No se pudo guardar los datos";
                        respuesta = false;
                    }



                }


            }

            catch (Exception ex)
            {
                mensaje = ex.Message;
                respuesta = false;

            }

            return respuesta;

        }

        public byte[] ObtenerLogo(out bool obtenido)
        {

            obtenido = true;
            byte[] logoBytes = new byte[0];

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();
                    string query = "select logo from NEGOCIO where idNegocio =1";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {

                        while (dr.Read())
                        {
                            logoBytes = (byte[])dr["logo"];
                            
                        }
                    }
                   
                }


            }

            catch (Exception ex)
            {
                obtenido = false;
                logoBytes = new byte[0];

            }

            return logoBytes;
        }

        public bool ActualizarLogo(byte[] image, out string mensaje) 
        {

            mensaje = string.Empty;
            bool respuesta = true;

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();


                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update NEGOCIO SET logo = @imagen");
                    
                    query.AppendLine("where idNegocio = 1;");
                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@imagen", image);
                    
                    cmd.CommandType = CommandType.Text;

                    if (cmd.ExecuteNonQuery() < 1)
                    {

                        mensaje = "No se pudo actualizar el logos";
                        respuesta = false;
                    }



                }


            }

            catch (Exception ex)
            {
                mensaje = ex.Message;
                respuesta = false;

            }

            return respuesta;


        }
    }
}

