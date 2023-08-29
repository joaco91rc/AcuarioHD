using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_CajaRegistradora
    {
        private CD_CajaRegistradora objcd_CajaRegistradora = new CD_CajaRegistradora();

        public List<CajaRegistradora> Listar()
        {
            return objcd_CajaRegistradora.Listar();
        }

        public int AperturaCaja(CajaRegistradora objCajaRegistradora, out string mensaje)
        {
            mensaje = string.Empty;

            if (mensaje != string.Empty)
            {
                return 0;
            }
            else

                return objcd_CajaRegistradora.AperturaCaja(objCajaRegistradora, out mensaje);
            
        }

        public bool CerrarCaja(CajaRegistradora objCajaRegistradora, out string mensaje)
        {
            mensaje = string.Empty;

            return objcd_CajaRegistradora.CerrarCaja(objCajaRegistradora, out mensaje);
        }

    }
}
