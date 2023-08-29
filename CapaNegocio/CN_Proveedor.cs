using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Proveedor
    {
        private CD_Proveedor objcd_proveedor = new CD_Proveedor();

        public List<Proveedor> Listar()
        {
            return objcd_proveedor.Listar();
        }

        public int Registrar(Proveedor objProveedor, out string mensaje)
        {
            mensaje = string.Empty;
            if (objProveedor.razonSocial == "")
            {
                mensaje = mensaje + "Es necesario la razon Social del Proveedor\n";
            }

            if (objProveedor.documento == "")
            {
                mensaje = mensaje + "Es necesario el CUIT del Proveedor\n";
            }


            if (objProveedor.correo == "")
            {
                mensaje = mensaje + "Es necesario el correo del Proveedor\n";
            }

            if (mensaje != string.Empty)
            {
                return 0;
            }
            else
            {

                return objcd_proveedor.Registrar(objProveedor, out mensaje);
            }
        }

        public bool Editar(Proveedor objProveedor, out string mensaje)
        {
            mensaje = string.Empty;
            if (objProveedor.razonSocial == "")
            {
                mensaje = mensaje + "Es necesario la razon Social del Proveedor\n";
            }

            if (objProveedor.documento == "")
            {
                mensaje = mensaje + "Es necesario el CUIT del Proveedor\n";
            }


            if (objProveedor.correo == "")
            {
                mensaje = mensaje + "Es necesario el correo del Proveedor\n";
            }

            if (mensaje != string.Empty)
            {
                return false;
            }
            else
            {

                return objcd_proveedor.Editar(objProveedor, out mensaje);
            }

        }


        public bool Eliminar(Proveedor objProveedor, out string mensaje)
        {
            return objcd_proveedor.Eliminar(objProveedor, out mensaje);
        }



    }
}
