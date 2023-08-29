using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Categoria
    {

        private CD_Categoria objcd_Categoria = new CD_Categoria();

        public List<Categoria> Listar()
        {
            return objcd_Categoria.Listar();
        }

        public int Registrar(Categoria objCategoria, out string mensaje)
        {
            mensaje = string.Empty;
            if (objCategoria.descripcion == "")
            {
                mensaje = mensaje + "Es necesario el nombre del Categoria\n";
            }

            if (mensaje != string.Empty)
            {
                return 0;
            }
            else
            {

                return objcd_Categoria.Registrar(objCategoria, out mensaje);
            }
        }

        public bool Editar(Categoria objCategoria, out string mensaje)
        {
            mensaje = string.Empty;
            if (objCategoria.descripcion == "")
            {
                mensaje = mensaje + "Es necesario el nombre del Categoria\n";
            }

            if (mensaje != string.Empty)
            {
                return false;
            }
            else
            {

                return objcd_Categoria.Editar(objCategoria, out mensaje);
            }

        }


        public bool Eliminar(Categoria objCategoria, out string mensaje)
        {
            return objcd_Categoria.Eliminar(objCategoria, out mensaje);
        }



    }
}
