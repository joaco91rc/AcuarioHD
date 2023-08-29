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
    public class CD_Reporte
    {

        public List<ReporteCompra> Compra(string fechaInicio, string fechaFin, int idProveedor)
        {
            List<ReporteCompra> lista = new List<ReporteCompra>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {

                    StringBuilder query = new StringBuilder();

                    SqlCommand cmd = new SqlCommand("SP_REPORTECOMPRAS", oconexion);
                    cmd.Parameters.AddWithValue("fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("fechaFin", fechaFin);
                    cmd.Parameters.AddWithValue("idProveedor", idProveedor);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ReporteCompra()
                            {
                                fechaRegistro = dr["fechaRegistro"].ToString(),
                                tipoDocumento = dr["tipoDocumento"].ToString(),
                                nroDocumento = dr["nroDocumento"].ToString(),
                                montoTotal = dr["montoTotal"].ToString(),
                                usuarioRegistro = dr["usuarioRegistro"].ToString(),
                                documentoProveedor = dr["documentoProveedor"].ToString(),
                                razonSocial = dr["razonSocial"].ToString(),
                                codigoProducto = dr["codigoProducto"].ToString(),
                                nombreProducto = dr["nombreProducto"].ToString(),
                                categoria = dr["categoria"].ToString(),
                                precioCompra = dr["precioCompra"].ToString(),
                                precioVenta = dr["precioVenta"].ToString(),
                                cantidad = dr["cantidad"].ToString(),
                                subTotal = dr["subTotal"].ToString(),

                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    lista = new List<ReporteCompra>();
                }

            }
            return lista;


        }


        public List<ReporteVenta> Venta(string fechaInicio, string fechaFin)
        {
            List<ReporteVenta> lista = new List<ReporteVenta>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {

                    StringBuilder query = new StringBuilder();

                    SqlCommand cmd = new SqlCommand("SP_REPORTEVENTAS", oconexion);
                    cmd.Parameters.AddWithValue("fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("fechaFin", fechaFin);
                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ReporteVenta()
                            {
                                fechaRegistro = dr["fechaRegistro"].ToString(),
                                tipoDocumento = dr["tipoDocumento"].ToString(),
                                nroDocumento = dr["nroDocumento"].ToString(),
                                montoTotal = dr["montoTotal"].ToString(),
                                usuarioRegistro = dr["usuarioRegistro"].ToString(),
                                documentoCliente = dr["documentoCliente"].ToString(),
                                nombreCliente = dr["nombreCliente"].ToString(),
                                codigoProducto = dr["codigoProducto"].ToString(),
                                nombreProducto = dr["nombreProducto"].ToString(),
                                categoria = dr["categoria"].ToString(),
                                precioVenta = dr["precioVenta"].ToString(),
                                cantidad = dr["cantidad"].ToString(),
                                subTotal = dr["subTotal"].ToString(),
                                formaPago = dr["formaPago"].ToString(),
                                descuento = dr["descuento"].ToString(),
                                montoDescuento = dr["montoDescuento"].ToString(),

                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    lista = new List<ReporteVenta>();
                }

            }
            return lista;


        }

    }
}
