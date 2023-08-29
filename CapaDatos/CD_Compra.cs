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
    public class CD_Compra
    {

        public int ObtenerCorrelativo()
        {
            int idCorrelativo = 0;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select count (*) +1 from COMPRA");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    idCorrelativo = Convert.ToInt32(cmd.ExecuteScalar());

                }
                catch (Exception ex)
                {
                    idCorrelativo = 0;
                }
            }

            return idCorrelativo;
        }

        public bool Registrar(Compra objCompra,DataTable detalleCompra, out string mensaje)
        {
            bool respuesta = false;
            mensaje = string.Empty;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {

                    

                    SqlCommand cmd = new SqlCommand("SP_REGISTRARCOMPRA", oconexion);
                    cmd.Parameters.AddWithValue("idUsuario",objCompra.oUsuario.idUsuario);
                    cmd.Parameters.AddWithValue("idProveedor",objCompra.oProveedor.idProveedor);
                    cmd.Parameters.AddWithValue("tipoDocumento",objCompra.tipoDocumento);
                    cmd.Parameters.AddWithValue("nroDocumento",objCompra.nroDocumento);
                    cmd.Parameters.AddWithValue("montoTotal",objCompra.montoTotal);
                    cmd.Parameters.AddWithValue("detalleCompra",detalleCompra);

                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    

                    respuesta = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();

                    

                }
                catch (Exception ex)
                {
                    respuesta = false;
                    mensaje = ex.Message;
                }
            }

            return respuesta;
        }



        public Compra ObtenerCompra(string numero)
        {
            Compra objCompra = new Compra();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {

     

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT C.idCompra,");
                    query.AppendLine("U.nombreCompleto,");
                    query.AppendLine("PR.documento, PR.razonSocial,");
                    query.AppendLine("C.tipoDocumento,C.nroDocumento,C.montoTotal,CONVERT(CHAR(10), C.fechaRegistro, 103)[fechaRegistro]");
                    query.AppendLine("FROM COMPRA C");
                    query.AppendLine("INNER JOIN USUARIO U ON U.idUsuario = C.idUsuario");
                    query.AppendLine("INNER JOIN PROVEEDOR PR ON PR.idProveedor = C.idProveedor");
                    query.AppendLine("WHERE C.nroDocumento = @numero");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@numero", numero);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            objCompra = new Compra()
                            {
                                idCompra = Convert.ToInt32(dr["idCompra"]),
                                oUsuario = new Usuario() { nombreCompleto=dr["nombreCompleto"].ToString()},
                                oProveedor = new Proveedor() { documento = dr["documento"].ToString(),razonSocial= dr["razonSocial"].ToString()},
                                tipoDocumento = dr["tipoDocumento"].ToString(),
                                nroDocumento = dr["nroDocumento"].ToString(),
                                montoTotal = Convert.ToDecimal(dr["montoTotal"].ToString()),
                                fechaRegistro = dr["fechaRegistro"].ToString()
                            };

                          
                        }
                    }

                }
                catch (Exception ex)
                {
                    objCompra = new Compra();
                }

            }



            return objCompra;
        }


        public List<DetalleCompra> ObtenerDetalleCompra(int idCompra)
        {
            List<DetalleCompra> oLista = new List<DetalleCompra>();
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("select P.nombre, DC.precioCompra,DC.cantidad,DC.montoTotal from DETALLE_COMPRA DC");
                    query.AppendLine("inner join PRODUCTO P ON P.idProducto=DC.idProducto");
                    query.AppendLine("WHERE DC.idCompra=@idCompra");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@idCompra", idCompra);
                    cmd.CommandType = CommandType.Text;


                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            oLista.Add(new DetalleCompra()
                            {
                                
                                oProducto = new Producto() { nombre = dr["nombre"].ToString() },
                                precioCompra = Convert.ToDecimal(dr["precioCompra"].ToString()),
                                cantidad = Convert.ToInt32(dr["cantidad"].ToString()),
                                montoTotal = Convert.ToDecimal(dr["montoTotal"].ToString())
                            });


                        }
                    }

                }
            }
            catch (Exception)
            {

                oLista = new List<DetalleCompra>();
            }
            return oLista;
        }



    }
}

    

