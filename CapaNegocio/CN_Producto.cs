using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Producto
    {

        private CD_Producto objcd_Producto = new CD_Producto();

        public List<Producto> Listar()
        {
            return objcd_Producto.Listar();
        }

        public int Registrar(Producto objProducto, out string mensaje)
        {
            mensaje = string.Empty;
            if (objProducto.nombre == "")
            {
                mensaje = mensaje + "Es necesario el nombre del Producto\n";
            }

            if (objProducto.codigo == "")
            {
                mensaje = mensaje + "Es necesario el codigo del Producto\n";
            }


            if (objProducto.descripcion == "")
            {
                mensaje = mensaje + "Es necesario la descripcion del Producto\n";
            }

            if (mensaje != string.Empty)
            {
                return 0;
            }
            else
            {

                return objcd_Producto.Registrar(objProducto, out mensaje);
            }
        }

        public bool Editar(Producto objProducto, out string mensaje)
        {
            mensaje = string.Empty;
            if (objProducto.nombre == "")
            {
                mensaje = mensaje + "Es necesario el nombre del Producto\n";
            }

            if (objProducto.codigo == "")
            {
                mensaje = mensaje + "Es necesario el codigo del Producto\n";
            }


            if (objProducto.descripcion == "")
            {
                mensaje = mensaje + "Es necesario la descripcion del Producto\n";
            }

            if (mensaje != string.Empty)
            {
                return false;
            }
            else
            {

                return objcd_Producto.Editar(objProducto, out mensaje);
            }

        }


        public bool Eliminar(Producto objProducto, out string mensaje)
        {
            return objcd_Producto.Eliminar(objProducto, out mensaje);
        }



    }
}
