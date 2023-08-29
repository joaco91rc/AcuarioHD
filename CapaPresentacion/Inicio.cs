using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaEntidad;
using FontAwesome.Sharp;
using CapaNegocio;
using CapaPresentacion.Modales;

namespace CapaPresentacion
{
    public partial class Inicio : Form
    {
        private static Usuario usuarioActual;
        private static IconMenuItem menuActivo = null;
        private static Form formularioActivo = null;
        public Inicio(Usuario objUsuario)
        {
            Environment.SetEnvironmentVariable("usuario", objUsuario.nombreCompleto);
            Environment.SetEnvironmentVariable("documentoUsuario", objUsuario.documento);
            Environment.SetEnvironmentVariable("rol", objUsuario.oRol.descripcion);
            usuarioActual = objUsuario;
            InitializeComponent();
        }

       

        private void iconPictureBox1_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("¿Esta seguro que desea cerrar la Aplicacion?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }

            
        }

        private void iconPictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            
        }

       

        private void Inicio_Load(object sender, EventArgs e)
        {
            List<Permiso> listaPermisos = new CN_Permiso().Listar(usuarioActual.idUsuario);

            foreach(IconMenuItem iconMenu in menu.Items)
            {
                bool encontrado = listaPermisos.Any(m => m.nombreMenu == iconMenu.Name);
                if(encontrado==false)
                {
                    iconMenu.Visible = false;
                }
            }

            lblUsuario.Text = Environment.GetEnvironmentVariable("usuario");
            lblDocumento.Text = Environment.GetEnvironmentVariable("documentoUsuario");
            lblRol.Text = Environment.GetEnvironmentVariable("rol");

        }

        private void AbrirFormulario(IconMenuItem menu, Form formulario)
        {
            if (menuActivo != null)
            {
                menuActivo.BackColor = Color.FromArgb(8, 83, 150);
                menuActivo.ForeColor = Color.FromArgb(224, 224, 224);
            }
            menu.BackColor = Color.FromArgb(178, 214, 243);
            menu.ForeColor = Color.FromArgb(8, 83, 150);
            menuActivo = menu;

            if (formularioActivo != null)
            {
                formularioActivo.Close();
            }
            formularioActivo = formulario;
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            formulario.BackColor = Color.FromArgb(63, 61, 64);
            contenedor.Controls.Add(formulario);
            formulario.Show();

        }
       

        private void subMenuCategoria_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuMantenedor, new frmCategoria());
        }

        private void subMenuProducto_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuMantenedor, new frmProducto());
        }

        private void subMenuRegistrarVenta_Click(object sender, EventArgs e)
        {
            List<CajaRegistradora> lista = new CN_CajaRegistradora().Listar();

            CajaRegistradora cajaAbierta = lista.Where(c => c.estado == true).FirstOrDefault();

            if (cajaAbierta == null)
            {
                MessageBox.Show("No hay cajas Abiertas. Por favor abra una caja para poder Registrar una Venta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                AbrirFormulario(menuVentas, new frmVentas(usuarioActual));
            }


        }

        private void subMenuDetalleVenta_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuVentas, new frmDetalleVenta());
        }

        private void subMenuRegistrarCompra_Click(object sender, EventArgs e)
        {
            List<CajaRegistradora> lista = new CN_CajaRegistradora().Listar();

            CajaRegistradora cajaAbierta = lista.Where(c => c.estado == true).FirstOrDefault();

            if (cajaAbierta == null)
            {
                MessageBox.Show("No hay cajas Abiertas. Por favor abra una caja para poder Registrar una Compra", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                AbrirFormulario(menuCompras, new frmCompras(usuarioActual));
            }



        }

        private void subMenuDetalleCompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuCompras, new frmDetalleCompra());
        }

        private void menuClientes_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmClientes());
        }

        private void menuProveedores_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmProveedores());
        }

        

        private void menuUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmUsuarios());
        }

        

        private void subMenuNegocio_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuMantenedor, new frmNegocio());
        }

        private void subMenuReporteCompras_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuReportes, new frmReporteCompras());
        }

        private void subMenuReporteVentas_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuReportes, new frmReporteVentas());
        }

      

        private void iconButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Esta seguro que desea cerrar Sesión?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void subMenuCajaDiaria_Click(object sender, EventArgs e)
        {
            List<CajaRegistradora> lista = new CN_CajaRegistradora().Listar();

            CajaRegistradora cajaAbierta = lista.Where(c => c.estado == true).FirstOrDefault();

            if (cajaAbierta == null)
            {
                MessageBox.Show("No hay cajas Abiertas. Por favor abra una caja para poder ver la Caja Diaria", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                AbrirFormulario(menuCajaRegistradora, new frmCajaRegistradora());
            }

           
        }

        private void subMenuAperturaCaja_Click(object sender, EventArgs e)
        {

            List<CajaRegistradora> lista = new CN_CajaRegistradora().Listar();

            CajaRegistradora cajaAbierta = lista.Where(c => c.estado == true).FirstOrDefault();

            if (cajaAbierta != null)
            {
                MessageBox.Show("Ya hay una Caja Abierta en curso. Por favor cierre la Caja en curso antes de Abrir otra caja", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                mdAperturacaja mdAperturacaja = new mdAperturacaja();
                mdAperturacaja.ShowDialog();
            }


           
        }

        private void subMenuConsultaCaja_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuCajaRegistradora, new frmDetalleCaja());
        }
    }
}
