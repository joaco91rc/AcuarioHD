using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Negocio
    {
        private CD_Negocio objcd_negocio = new CD_Negocio();

        public Negocio ObtenerDatos()
        {
            return objcd_negocio.ObtenerDatos();
        }

        public bool Guardardatos(Negocio objNegocio, out string mensaje)
        {
            mensaje = string.Empty;
            if (objNegocio.nombre == "")
            {
                mensaje = mensaje + "Es necesario el nombre del Negocio\n";
            }

            if (objNegocio.CUIT == "")
            {
                mensaje = mensaje + "Es necesario el  numero de CUIT del Negocio\n";
            }


            if (objNegocio.direccion == "")
            {
                mensaje = mensaje + "Es necesario la direccion del Negocio\n";
            }

            if (mensaje != string.Empty)
            {
                return false;
            }
            else
            {

                return objcd_negocio.Guardardatos(objNegocio, out mensaje);
            }
        }

        public byte[] ObtenerLogo( out bool obtenido)
        {
            return objcd_negocio.ObtenerLogo(out obtenido);
        }

        public bool ActualizarLogo(byte[] imagen,out string mensaje)
        {
            return objcd_negocio.ActualizarLogo(imagen, out mensaje);
        }
    }
}
