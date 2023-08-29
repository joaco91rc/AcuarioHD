using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;
namespace CapaDatos
{
    public class CD_Usuario
    {
        public List<Usuario> Listar() {
            List<Usuario> lista = new List<Usuario>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select u.idUsuario,u.documento,u.nombreCompleto,u.correo,u.clave,u.estado,r.idRol, r.descripcion from usuario u");
                    query.AppendLine("inner join rol r on r.idRol = u.idRol");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Usuario()
                            {
                                idUsuario = Convert.ToInt32(dr["idUsuario"]),
                                documento = dr["documento"].ToString(),
                                nombreCompleto = dr["nombreCompleto"].ToString(),
                                correo = dr["correo"].ToString(),
                                clave = dr["clave"].ToString(),
                                estado = Convert.ToBoolean(dr["estado"]),
                                oRol= new Rol() { idRol= Convert.ToInt32(dr["idRol"]), descripcion = dr["descripcion"].ToString() }
                            });
                        }
                    }

                }
                catch (Exception ex) {
                    lista = new List<Usuario>();
                }

            }
            return lista;
        }


        public int Registrar(Usuario objUsuario,  out string mensaje) {
            int idUsuariogenerado = 0;
            mensaje = string.Empty;

            try
            {



                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARUSUARIO", oconexion);
                    cmd.Parameters.AddWithValue("documento", objUsuario.documento);
                    cmd.Parameters.AddWithValue("nombreCompleto", objUsuario.nombreCompleto);
                    cmd.Parameters.AddWithValue("correo", objUsuario.correo);
                    cmd.Parameters.AddWithValue("clave", objUsuario.clave);
                    cmd.Parameters.AddWithValue("idRol", objUsuario.oRol.idRol);
                    cmd.Parameters.AddWithValue("estado", objUsuario.estado);

                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar,500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    idUsuariogenerado = Convert.ToInt32(cmd.Parameters["resultado"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();


                }

            }

            catch (Exception ex) {
                idUsuariogenerado = 0;
                mensaje = ex.Message;

            }


            return idUsuariogenerado;
        
        }

        public bool Editar(Usuario objUsuario, out string mensaje)
        {
            bool respuesta = false;
            mensaje = string.Empty;

            try
            {



                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITARUSUARIO", oconexion);
                    cmd.Parameters.AddWithValue("idUsuario", objUsuario.idUsuario);
                    cmd.Parameters.AddWithValue("documento", objUsuario.documento);
                    cmd.Parameters.AddWithValue("nombreCompleto", objUsuario.nombreCompleto);
                    cmd.Parameters.AddWithValue("correo", objUsuario.correo);
                    cmd.Parameters.AddWithValue("clave", objUsuario.clave);
                    cmd.Parameters.AddWithValue("idRol", objUsuario.oRol.idRol);
                    cmd.Parameters.AddWithValue("estado", objUsuario.estado);

                    cmd.Parameters.Add("respuesta", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar,500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["respuesta"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();

                    if(Environment.GetEnvironmentVariable("usuario") != null)
                    {
                        Environment.SetEnvironmentVariable("usuario", objUsuario.nombreCompleto);
                        
                    }
                }

            }

            catch (Exception ex)
            {
                respuesta = false;
                mensaje = ex.Message;

            }


            return respuesta;

        }


        public bool Eliminar(Usuario objUsuario, out string mensaje)
        {
            bool respuesta = false;
            mensaje = string.Empty;

            try
            {



                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_ELIMINARUSUARIO", oconexion);
                    cmd.Parameters.AddWithValue("idUsuario", objUsuario.idUsuario);
                    cmd.Parameters.Add("respuesta", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar,500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["respuesta"].Value);
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
