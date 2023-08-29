using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocio;
using CapaEntidad;

namespace CapaPresentacion
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void iconPictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }
        //Boton Ingresar
        private void iconButton1_Click(object sender, EventArgs e)

        {
            List<Usuario> test = new CN_Usuario().Listar();

            Usuario oUsuario = new CN_Usuario().Listar().Where(u => u.documento == txtUsuario.Text && u.clave == txtContrasena.Text).FirstOrDefault();

            if (oUsuario != null)
            {
                Form form1 = new Inicio(oUsuario);
                form1.Show();
                this.Hide();
                form1.FormClosing += form_closing;
            }
            
       
                
                else
                {
                    MessageBox.Show("No se encontro el Usuario", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }

            
        }

        private void form_closing(object sender, FormClosingEventArgs e) {
            this.Show();
            txtContrasena.Text = "";
            txtUsuario.Text = "";
        }

        private void txtContrasena_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                List<Usuario> test = new CN_Usuario().Listar();

                Usuario oUsuario = new CN_Usuario().Listar().Where(u => u.documento == txtUsuario.Text && u.clave == txtContrasena.Text).FirstOrDefault();

                if (oUsuario != null)
                {
                    Form form1 = new Inicio(oUsuario);
                    form1.Show();
                    this.Hide();
                    form1.FormClosing += form_closing;
                }



                else
                {
                    MessageBox.Show("No se encontro el Usuario", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
            }
        }
    }
}
