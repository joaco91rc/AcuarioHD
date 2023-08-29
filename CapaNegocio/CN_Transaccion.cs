using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Transaccion
    {
        private CD_Transaccion objcd_TransaccionCaja = new CD_Transaccion();
        public List<TransaccionCaja> Listar(int idCajaRegistradora)
        {
            return objcd_TransaccionCaja.Listar(idCajaRegistradora);
        }

        public int RegistrarMovimiento(TransaccionCaja objTransaccion, out string mensaje)
        {
            mensaje = string.Empty;
            if (objTransaccion.tipoTransaccion == "")
            {
                mensaje = mensaje + "Es necesario especificar el tipo de Movimiento\n";
            }

            if (objTransaccion.monto == 0)
            {
                mensaje = mensaje + "Es necesario especificar el Monto del Movimiento\n";
            }


            

            if (mensaje != string.Empty)
            {
                return 0;
            }
            else
            {

                return objcd_TransaccionCaja.RegistrarMovimiento(objTransaccion, out mensaje);
            }
        }
    }
}
