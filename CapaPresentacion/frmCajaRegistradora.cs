using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmCajaRegistradora : Form
    {
        public frmCajaRegistradora()
        {
            InitializeComponent();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            List<CajaRegistradora> lista = new CN_CajaRegistradora().Listar();

            CajaRegistradora cajaAbierta = lista.Where(c => c.estado == true).FirstOrDefault();

            if (cajaAbierta != null)
            {
                int idCajaAbierta = cajaAbierta.idCajaRegistradora;

                string mensaje = string.Empty;
                CajaRegistradora objCajaRegistradora = new CajaRegistradora()
                {
                    idCajaRegistradora = idCajaAbierta,
                    fechaCierre = DateTime.Now.ToString(),
                    saldo = Convert.ToDecimal(txtSaldo.Text) + cajaAbierta.saldo

                };
                bool resultado = new CN_CajaRegistradora().CerrarCaja(objCajaRegistradora, out mensaje);
                MessageBox.Show("Caja Registradora de la fecha: " + cajaAbierta.fechaApertura + "Cerrada ", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("No hay ninguna Caja para Cerrar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

        private void frmCajaRegistradora_Load(object sender, EventArgs e)
        {
            
            cboTipoMovimiento.Items.Add(new OpcionCombo() { Valor = 1, Texto = "ENTRADA" });
            cboTipoMovimiento.Items.Add(new OpcionCombo() { Valor = 0, Texto = "SALIDA" });
            cboTipoMovimiento.DisplayMember = "Texto";
            cboTipoMovimiento.ValueMember = "Valor";
            cboTipoMovimiento.SelectedIndex = 0;

            foreach (DataGridViewColumn columna in dgvData.Columns)
            {

                if (columna.Visible == true )
                {
                    cboBusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });

                }


            }
            cboBusqueda.DisplayMember = "Texto";
            cboBusqueda.ValueMember = "Valor";
            cboBusqueda.SelectedIndex = 3;

            CajaRegistradora cajaAbierta = new CN_CajaRegistradora().Listar().Where(x => x.estado == true).FirstOrDefault();

            if (cajaAbierta != null)
            {
                txtSaldoInicial.Text= cajaAbierta.saldo.ToString("0.00");
                txtSaldo.Text = cajaAbierta.saldo.ToString("0.00");
                List<TransaccionCaja> listaTransacciones = new CN_Transaccion().Listar(cajaAbierta.idCajaRegistradora);

                foreach (TransaccionCaja item in listaTransacciones)
                {
                    dgvData.Rows.Add(new object[] {
                        "",
                        item.idCajaRegistradora,
                        item.idTransaccion,
                        item.hora,
                        item.tipoTransaccion,
                        item.monto,
                        item.docAsociado,
                        item.usuarioTransaccion
                             });

                }
                calcularTotal(cajaAbierta);
            }




        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            
            

            string mensaje = string.Empty;

            List<CajaRegistradora> lista = new CN_CajaRegistradora().Listar();

            CajaRegistradora cajaAbierta = lista.Where(c => c.estado == true).FirstOrDefault();

            if (cajaAbierta != null)

            {
                decimal montoCalculado = Convert.ToDecimal(txtMonto.Text);
                if (cboTipoMovimiento.Text == "SALIDA")
                {
                    montoCalculado = montoCalculado * -1;
                    
                }

                TransaccionCaja objTransaccion = new TransaccionCaja()
                {
                    idCajaRegistradora = cajaAbierta.idCajaRegistradora,
                    
                    hora = dtpFecha.Value.ToString(),
                    tipoTransaccion = cboTipoMovimiento.Text,
                    monto = montoCalculado,
                    docAsociado = txtDocAsociado.Text,
                    usuarioTransaccion = Environment.GetEnvironmentVariable("usuario")
                };




                int idTransaccionGenerado = new CN_Transaccion().RegistrarMovimiento(objTransaccion, out mensaje);

                


                if (idTransaccionGenerado != 0)
                {
                    objTransaccion.idTransaccion = idTransaccionGenerado;
                    dgvData.Rows.Add(new object[] { "",
                        idTransaccionGenerado,
                        idCajaRegistradora,
                        dtpFecha.Value.ToString(),
                        cboTipoMovimiento.Text,
                        montoCalculado,
                        txtDocAsociado.Text,
                        objTransaccion.usuarioTransaccion


            });
                    calcularTotal(cajaAbierta);
                    Limpiar();
                    txtMonto.Select();
                }
                else
                {
                    MessageBox.Show(mensaje,"Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                }


            }
            


        }


        private void calcularTotal(CajaRegistradora objCajaRegistradora)
        {
            decimal total = 0;
            if (dgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.Visible == true)
                        total += Convert.ToDecimal(row.Cells["monto"].Value.ToString());

                }
                txtSaldo.Text = (total + objCajaRegistradora.saldo).ToString("0.00");
            }
        }

        private void Limpiar()
        {

           
            cboTipoMovimiento.SelectedItem = 1;
            txtMonto.Text = "";
            txtDocAsociado.Text = "";
            
            txtMonto.Select();

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cboBusqueda.SelectedItem).Valor.ToString();
            List<CajaRegistradora> lista = new CN_CajaRegistradora().Listar();

            CajaRegistradora cajaAbierta = lista.Where(c => c.estado == true).FirstOrDefault();
            if (dgvData.Rows.Count > 0)
            {

                foreach (DataGridViewRow row in dgvData.Rows)
                {

                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))
                    {
                        row.Visible = true;
                        
                    }
                    else
                        row.Visible = false;


                }
                calcularTotal(cajaAbierta);


            }
        }

        


        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            List<CajaRegistradora> lista = new CN_CajaRegistradora().Listar();

            CajaRegistradora cajaAbierta = lista.Where(c => c.estado == true).FirstOrDefault();
            txtBusqueda.Clear();
            foreach (DataGridViewRow row in dgvData.Rows)
                row.Visible = true;
            calcularTotal(cajaAbierta);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData== Keys.Enter)
            {
                string columnaFiltro = ((OpcionCombo)cboBusqueda.SelectedItem).Valor.ToString();
                List<CajaRegistradora> lista = new CN_CajaRegistradora().Listar();

                CajaRegistradora cajaAbierta = lista.Where(c => c.estado == true).FirstOrDefault();
                if (dgvData.Rows.Count > 0)
                {

                    foreach (DataGridViewRow row in dgvData.Rows)
                    {

                        if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))
                        {
                            row.Visible = true;

                        }
                        else
                            row.Visible = false;


                    }
                    calcularTotal(cajaAbierta);


                }
            }
        }
    }
}
