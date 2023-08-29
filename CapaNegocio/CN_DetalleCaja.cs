using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_DetalleCaja
    {

        private CD_DetalleCaja objcd_DetalleCaja = new CD_DetalleCaja();

        public List<DetalleCaja> DetalleCaja(string fechaConsulta)
        {
            return objcd_DetalleCaja.DetalleCaja(fechaConsulta);
        }

        public List<DetalleCaja> Listar(string fecha)
        {
            return objcd_DetalleCaja.Listar(fecha);
        }
    }
}
