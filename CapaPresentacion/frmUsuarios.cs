using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaPresentacion.Utilidades;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class frmUsuarios : Form
    {
        public frmUsuarios()
        {
            InitializeComponent();
        }

        private void frmUsuarios_Load(object sender, EventArgs e)
        {

            cboEstado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "No Activo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            List<Rol> listaRol = new CN_ROL().Listar();

            foreach (Rol item in listaRol)
            {
                cboRol.Items.Add(new OpcionCombo() { Valor = item.idRol, Texto = item.descripcion });
            }
            cboRol.DisplayMember = "Texto";
            cboRol.ValueMember = "Valor";
            cboRol.SelectedIndex = 0;

            foreach (DataGridViewColumn columna in dgvData.Columns)
            {

                if (columna.Visible == true && columna.Name != "btnSeleccionar")
                {
                    cboBusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });

                }


            }

            cboBusqueda.DisplayMember = "Texto";
            cboBusqueda.ValueMember = "Valor";
            cboBusqueda.SelectedIndex = 0;

            //Mostrar todos los usuarios
            List<Usuario> listaUsuario = new CN_Usuario().Listar();

            foreach (Usuario item in listaUsuario)
            {
                dgvData.Rows.Add(new object[] { "",item.idUsuario,item.documento,item.nombreCompleto,item.correo,item.clave,item.oRol.idRol,item.oRol.descripcion,

                    item.estado==true?1:0,
                    item.estado==true? "Activo": "No Activo"
                    });
            }




        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            Usuario objUsuario = new Usuario()
            {
                idUsuario = Convert.ToInt32(txtIdUsuario.Text),
                documento = txtDocumento.Text,
                nombreCompleto = txtNombreCompleto.Text,
                correo = txtEmail.Text,
                clave =txtClave.Text,
                oRol = new Rol { idRol = Convert.ToInt32(((OpcionCombo)cboRol.SelectedItem).Valor)},
                estado = Convert.ToInt32(((OpcionCombo)cboEstado.SelectedItem).Valor) == 1? true : false
            };

            if (objUsuario.idUsuario == 0)
            {

                int idUsuarioGenerado = new CN_Usuario().Registrar(objUsuario, out mensaje);


                if (idUsuarioGenerado != 0)
                {
                    dgvData.Rows.Add(new object[] { "",idUsuarioGenerado,txtDocumento.Text,txtNombreCompleto.Text,txtEmail.Text,txtClave.Text,

                ((OpcionCombo)cboRol.SelectedItem).Valor.ToString(),
                ((OpcionCombo)cboRol.SelectedItem).Texto.ToString(),
                ((OpcionCombo)cboEstado.SelectedItem).Valor.ToString(),
                ((OpcionCombo)cboEstado.SelectedItem).Texto.ToString()
            });
                    
                    Limpiar();
                }
                else
                {

                    MessageBox.Show(mensaje);
                }


            }
            else { 
            
                bool resultado = new CN_Usuario().Editar(objUsuario, out mensaje);
                if (resultado)
                {
                    DataGridViewRow row = dgvData.Rows[Convert.ToInt32(txtIndice.Text)];
                    row.Cells["idusuario"].Value = txtIdUsuario.Text;
                    row.Cells["documento"].Value = txtDocumento.Text;
                    row.Cells["nombreCompleto"].Value = txtNombreCompleto.Text;
                    row.Cells["correo"].Value = txtEmail.Text;
                    row.Cells["clave"].Value = txtClave.Text;
                    row.Cells["idRol"].Value = ((OpcionCombo)cboRol.SelectedItem).Valor.ToString();
                    row.Cells["rol"].Value = ((OpcionCombo)cboRol.SelectedItem).Texto.ToString();
                    row.Cells["estadoValor"].Value = ((OpcionCombo)cboEstado.SelectedItem).Valor.ToString();
                    row.Cells["estado"].Value = ((OpcionCombo)cboEstado.SelectedItem).Texto.ToString();

                    
                    Limpiar();
                    MessageBox.Show("Datos Modificados.Por favor Inicie Sesion de Nuevo.", "Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    Application.Restart();

                }
                else {

                    MessageBox.Show(mensaje);
                }

            }

            

            
        }

        private void Limpiar() {
            txtIndice.Text = "-1";
            txtIdUsuario.Text = "0";
            txtDocumento.Text = "";
            txtClave.Text = "";
            txtConfirmarClave.Text = "";
            txtEmail.Text = "";
            txtNombreCompleto.Text = "";
            cboRol.SelectedIndex = 0;
            cboEstado.SelectedIndex = 0;
            txtDocumento.Select();
        }

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {


            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0) {

                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.check20.Width;
                var h = Properties.Resources.check20.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Width - h) / 2;
                e.Graphics.DrawImage(Properties.Resources.check20, new Rectangle(x, y, w, h));
                e.Handled = true;
            }


        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Columns[e.ColumnIndex].Name == "btnSeleccionar") {
                int indice = e.RowIndex;
                
                if (indice >= 0) {
                    txtIndice.Text = indice.ToString();
                    txtUsuarioSeleccionado.Text = dgvData.Rows[indice].Cells["nombreCompleto"].Value.ToString();
                    txtIdUsuario.Text = dgvData.Rows[indice].Cells["idUsuario"].Value.ToString();
                    txtDocumento.Text = dgvData.Rows[indice].Cells["documento"].Value.ToString();
                    txtNombreCompleto.Text = dgvData.Rows[indice].Cells["nombreCompleto"].Value.ToString();
                    txtEmail.Text = dgvData.Rows[indice].Cells["correo"].Value.ToString();
                    txtClave.Text = dgvData.Rows[indice].Cells["clave"].Value.ToString();
                    txtConfirmarClave.Text = dgvData.Rows[indice].Cells["clave"].Value.ToString();

                    foreach (OpcionCombo oc in cboRol.Items) {

                        if (Convert.ToInt32(oc.Valor) == (Convert.ToInt32(dgvData.Rows[indice].Cells["idRol"].Value)))
                        {
                            int indiceCombo = cboRol.Items.IndexOf(oc);
                            cboRol.SelectedIndex = indiceCombo;
                            break;

                        }

                    }

                    foreach (OpcionCombo oc in cboEstado.Items)
                    {

                        if (Convert.ToInt32(oc.Valor) == (Convert.ToInt32(dgvData.Rows[indice].Cells["EstadoValor"].Value)))
                        {
                            int indiceCombo = cboEstado.Items.IndexOf(oc);
                            cboEstado.SelectedIndex = indiceCombo;
                            break;

                        }

                    }

                }
            
            }
        }

        private void btnLimpiarDatos_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtIdUsuario.Text) != 0) {

                if (MessageBox.Show("Desea eliminar el usuario?", "Confirmar Eliminacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    string mensaje = string.Empty;
                    Usuario objUsuario = new Usuario()
                    {
                        idUsuario = Convert.ToInt32(txtIdUsuario.Text),
                        
                    };

                    bool respuesta = new CN_Usuario().Eliminar(objUsuario, out mensaje);
                    
                    if (respuesta)
                    {

                        dgvData.Rows.RemoveAt(Convert.ToInt32(txtIndice.Text));
                        txtIndice.Text = "-1";
                        txtIdUsuario.Text = "0";
                    }

                    else
                    {

                        MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);


                    }
                    
                    
                    
                    
                    }
                }
            
            
            }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cboBusqueda.SelectedItem).Valor.ToString();

            if(dgvData.Rows.Count > 0)
            {

                foreach (DataGridViewRow row in dgvData.Rows)
                {

                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))
                        row.Visible = true;
                    else
                        row.Visible = false;


                }

            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBusqueda.Clear();
            foreach(DataGridViewRow row in dgvData.Rows)
              row.Visible = true;
            
        }
    }
    }

