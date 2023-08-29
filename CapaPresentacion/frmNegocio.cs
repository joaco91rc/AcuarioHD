using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmNegocio : Form
    {
        public frmNegocio()
        {
            InitializeComponent();
        }

        public Image ByteToImage(byte[] imageBytes )
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image imagen = new Bitmap(ms);

            return imagen;
        }

        private void frmNegocio_Load(object sender, EventArgs e)
        {
            bool obtenido = true;
            byte[] byteImagen = new CN_Negocio().ObtenerLogo(out obtenido);
            if (obtenido)
            {
                picLogo.Image = ByteToImage(byteImagen);
            }

            Negocio datos = new CN_Negocio().ObtenerDatos();
            txtRazonSocial.Text = datos.nombre;
            txtDocumento.Text = datos.CUIT;
            txtDireccion.Text = datos.direccion;
        }

        private void btnCargarLogo_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            OpenFileDialog oOpenFileDialog = new OpenFileDialog();
            oOpenFileDialog.Filter = "Files|*.jpg;*.jpeg;*.png";
            
            if(oOpenFileDialog.ShowDialog()== DialogResult.OK)
            {
                byte[] byteImage = File.ReadAllBytes(oOpenFileDialog.FileName);
                bool respuesta = new CN_Negocio().ActualizarLogo(byteImage, out mensaje);

                if (respuesta)
                    picLogo.Image = ByteToImage(byteImage);
                else
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            Negocio objNegocio = new Negocio()
            {  
                nombre = txtRazonSocial.Text,
                CUIT = txtDocumento.Text,
                direccion = txtDireccion.Text
            };

            bool respuesta = new CN_Negocio().Guardardatos(objNegocio, out mensaje);

            if (respuesta)
                MessageBox.Show("Los cambios fueron guardados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("No se pudieron guardar los cambios", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
