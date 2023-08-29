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
    public class CD_Venta
    {

        public int ObtenerCorrelativo()
        {
            int idCorrelativo = 0;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select count (*) +1 from VENTA");

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

        public  bool RestarStock(int idProducto, int cantidad)
        {
            bool respuesta = true;
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update producto set stock = stock - @cantidad where idProducto = @idproducto");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@idproducto", idProducto);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    respuesta = cmd.ExecuteNonQuery()>0?true:false;

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }

            return respuesta;
        }

        public bool SumarStock(int idProducto, int cantidad)
        {
            bool respuesta = true;
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update producto set stock = stock + @cantidad where idProducto = @idproducto");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@idproducto", idProducto);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }

            return respuesta;
        }


        public bool Registrar(Venta objventa, DataTable detalleVenta, out string mensaje)
        {
            bool respuesta = false;
            mensaje = string.Empty;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {



                    SqlCommand cmd = new SqlCommand("SP_REGISTRARVENTA", oconexion);
                    cmd.Parameters.AddWithValue("idUsuario", objventa.oUsuario.idUsuario);
                    
                    cmd.Parameters.AddWithValue("tipoDocumento", objventa.tipoDocumento);
                    cmd.Parameters.AddWithValue("nroDocumento", objventa.nroDocumento);
                    cmd.Parameters.AddWithValue("documentoCliente", objventa.documentoCliente);
                    cmd.Parameters.AddWithValue("nombreCliente", objventa.nombreCliente);
                    cmd.Parameters.AddWithValue("montoPago", objventa.montoPago);
                    cmd.Parameters.AddWithValue("montoCambio", objventa.montoCambio);
                    cmd.Parameters.AddWithValue("montoTotal", objventa.montoTotal);
                    
                    cmd.Parameters.AddWithValue("detalleVenta", detalleVenta);
                    cmd.Parameters.AddWithValue("formaPago", objventa.formaPago);
                    cmd.Parameters.AddWithValue("descuento", objventa.descuento);
                    cmd.Parameters.AddWithValue("montoDescuento", objventa.montoDescuento);
                    cmd.Parameters.AddWithValue("fecha", Convert.ToDateTime(objventa.fechaRegistro));

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


        public Venta ObtenerVenta(string numero)
        {
            Venta objVenta = new Venta();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {


                    oconexion.Open();
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT v.idVenta, u.nombreCompleto,v.documentoCliente,v.nombreCliente,v.tipoDocumento,v.nroDocumento,v.montoPago,v.montoCambio,v.montoTotal, convert(char(10), v.fechaRegistro, 103)[FechaRegistro],v.formaPago,v.descuento,v.montoDescuento");
                    query.AppendLine("FROM VENTA v");
                    query.AppendLine("inner join USUARIO U ON U.idUsuario = V.idUsuario");
                    query.AppendLine("WHERE v.nroDocumento = @numero");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@numero", numero);
                    cmd.CommandType = CommandType.Text;
                    

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            objVenta = new Venta()
                            {
                                idVenta = Convert.ToInt32(dr["idVenta"]),
                                oUsuario = new Usuario() { nombreCompleto = dr["nombreCompleto"].ToString() },
                                documentoCliente = dr["documentoCliente"].ToString(),
                                tipoDocumento = dr["tipoDocumento"].ToString(),
                                nombreCliente = dr["nombreCliente"].ToString(),
                                nroDocumento = dr["nroDocumento"].ToString(),
                                montoPago = Convert.ToDecimal(dr["montoPago"].ToString()),
                                montoCambio = Convert.ToDecimal(dr["montoCambio"].ToString()),
                                montoTotal = Convert.ToDecimal(dr["montoTotal"].ToString()),
                                formaPago = dr["formaPago"].ToString(),
                                descuento = Convert.ToInt32(dr["descuento"]),
                                montoDescuento = Convert.ToDecimal(dr["montoDescuento"].ToString()),
                                fechaRegistro = dr["fechaRegistro"].ToString()
                            };


                        }
                    }

                }
                catch (Exception ex)
                {
                    objVenta = new Venta();
                }

            }



            return objVenta;
        }


        public List<DetalleVenta> ObtenerDetalleVenta(int idVenta)
        {
            List<DetalleVenta> oLista = new List<DetalleVenta>();
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("SELECT P.nombre,DV.precioVenta,DV.cantidad,DV.subTotal FROM DETALLE_VENTA DV");
                    query.AppendLine("INNER JOIN  PRODUCTO P ON P.idProducto= DV.idProducto");
                    query.AppendLine("WHERE DV.idVenta=@idVenta");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@idVenta", idVenta);
                    cmd.CommandType = CommandType.Text;


                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            oLista.Add(new DetalleVenta()
                            {

                                oProducto = new Producto() { nombre = dr["nombre"].ToString() },
                                precioVenta = Convert.ToDecimal(dr["precioVenta"].ToString()),
                                cantidad = Convert.ToInt32(dr["cantidad"].ToString()),
                                subTotal = Convert.ToDecimal(dr["subTotal"].ToString())
                            });


                        }
                    }

                }
            }
            catch (Exception)
            {

                oLista = new List<DetalleVenta>();
            }
            return oLista;
        }

    }
}
