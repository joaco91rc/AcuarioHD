using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Venta
    {
        public int idVenta { get; set; }
        public Usuario oUsuario { get; set; }
        public string tipoDocumento { get; set; }
        public string nroDocumento { get; set; }
        public string documentoCliente { get; set; }
        public string nombreCliente { get; set; }
        public decimal montoPago { get; set; }
        public decimal montoCambio { get; set; }
        public decimal montoTotal { get; set; }
        public List<DetalleVenta> oDetalleVenta { get; set; }
        public string fechaRegistro { get; set; }
        public string formaPago { get; set; }
        public decimal descuento { get; set; }
        public decimal montoDescuento { get; set; }
    }
}
