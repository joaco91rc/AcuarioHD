using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaPresentacion;

namespace CapaPresentacion.Modales
{
    public partial class mdAperturacaja : Form
    {
        private Usuario _Usuario;
        public mdAperturacaja(Usuario oUsuario = null)
        {
            _Usuario = oUsuario;
            InitializeComponent();
        }

        private void iconPictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void iconPictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            CajaRegistradora objCaja = new CajaRegistradora()
            {
               usuarioAperturaCaja = Environment.GetEnvironmentVariable("usuario"),
               saldo = Convert.ToDecimal(txtSaldoInicial.Text)
            };
            List<CajaRegistradora> lista = new CN_CajaRegistradora().Listar();
            CajaRegistradora cajaAbierta = lista.Where(c => c.estado == true).FirstOrDefault();

            if(cajaAbierta == null) {
                int idCajaGenerado = new CN_CajaRegistradora().AperturaCaja(objCaja, out mensaje);
                this.Close();
                MessageBox.Show("Caja Abierta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ya existe una Caja Abierta. Cierrela e intente  nuevamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            
            
        }

        private void mdAperturacaja_Load(object sender, EventArgs e)
        {
            txtFechaApertura.Text = DateTime.Now.ToString();

            txtUsuario.Text = Environment.GetEnvironmentVariable("usuario");

            






        }
    }
}
