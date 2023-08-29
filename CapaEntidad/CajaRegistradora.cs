using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class CajaRegistradora
    {

        public int idCajaRegistradora { get; set; }
        public string fechaApertura { get; set; }
        public string fechaCierre { get; set; }
        public string usuarioAperturaCaja { get; set; }
        public decimal saldo { get; set; }
        public bool estado { get; set; }

    }
}
