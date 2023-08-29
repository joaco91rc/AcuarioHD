using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;
namespace CapaNegocio
{
    public class CN_Usuario
    {

        private CD_Usuario objcd_usuario = new CD_Usuario();

        public List<Usuario> Listar() {
            return objcd_usuario.Listar();
        }

        public int Registrar(Usuario objUsuario, out string mensaje)
        {
            mensaje = string.Empty;
            if (objUsuario.nombreCompleto == "")
            {
                mensaje = mensaje + "Es necesario el nombre del Usuario\n";
            }

            if (objUsuario.documento == "")
            {
                mensaje = mensaje + "Es necesario el documento del Usuario\n";
            }


            if (objUsuario.clave == "")
            {
                mensaje = mensaje + "Es necesario la clave del Usuario\n";
            }

            if (mensaje != string.Empty)
            {
                return 0;
            }
            else
            {

                return objcd_usuario.Registrar(objUsuario, out mensaje);
            }
        }

        public bool Editar(Usuario objUsuario, out string mensaje)
        {
            mensaje = string.Empty;
            if (objUsuario.nombreCompleto == "")
            {
                mensaje = mensaje + "Es necesario el nombre del Usuario\n";
            }

            if (objUsuario.documento == "")
            {
                mensaje = mensaje + "Es necesario el documento del Usuario\n";
            }


            if (objUsuario.clave == "")
            {
                mensaje = mensaje + "Es necesario la clave del Usuario\n";
            }

            if (mensaje != string.Empty)
            {
                return false;
            }
            else
            {

                return objcd_usuario.Editar(objUsuario, out mensaje);
            }

        }


        public bool Eliminar(Usuario objUsuario, out string mensaje)
        {
            return objcd_usuario.Eliminar(objUsuario, out mensaje);
        }


    }
}
