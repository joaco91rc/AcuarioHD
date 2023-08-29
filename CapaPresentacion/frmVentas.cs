using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Modales;
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
    public partial class frmVentas : Form
    {
        private Usuario _Usuario;
        public frmVentas(Usuario oUsuario = null)
        {
            _Usuario = oUsuario;
            InitializeComponent();
        }

        private void frmVentas_Load(object sender, EventArgs e)
        {
            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Factura A", Texto = "Factura A" });
            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Factura B", Texto = "Factura B" });
            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Factura C", Texto = "Factura C" });
            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Remito R", Texto = "Remito R" });
            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Presupuesto", Texto = "Presupuesto" });
            cboTipoDocumento.DisplayMember = "Texto";
            cboTipoDocumento.ValueMember = "Valor";
            cboTipoDocumento.SelectedIndex = 0;

            cboFormaPago.Items.Add(new OpcionCombo() { Valor = "EFECTIVO", Texto = "EFECTIVO" });
            cboFormaPago.Items.Add(new OpcionCombo() { Valor = "DEBITO", Texto = "DEBITO" });
            cboFormaPago.Items.Add(new OpcionCombo() { Valor = "CREDITO", Texto = "CREDITO" });
            cboFormaPago.Items.Add(new OpcionCombo() { Valor = "TRANSFERENCIA", Texto = "TRANSFERENCIA" });
            cboFormaPago.Items.Add(new OpcionCombo() { Valor = "MERCADOPAGO", Texto = "MERCADO PAGO" });
            cboFormaPago.Items.Add(new OpcionCombo() { Valor = "CUENTA DNI", Texto = "CUENTA DNI" });
            cboFormaPago.Items.Add(new OpcionCombo() { Valor = "MODO", Texto = "MODO" });
            cboFormaPago.Items.Add(new OpcionCombo() { Valor = "CRYPTO", Texto = "CRYPTO" });

            cboFormaPago.DisplayMember = "Texto";
            cboFormaPago.ValueMember = "Valor";
            cboFormaPago.SelectedIndex = 0;


            dtpFecha.Text = DateTime.Now.ToString();
            txtIdProducto.Text = "0";
            txtIdProducto.Text = "0";
            txtPagaCon.Text = "";
            txtTotalAPagar.Text = "0";
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            using (var modal = new mdCliente())
            {
                var result = modal.ShowDialog();
                if (result == DialogResult.OK)
                {

                    txtDocumentoCliente.Text = modal._Cliente.documento;
                    txtNombreCliente.Text = modal._Cliente.nombreCompleto;
                    txtCodigoProducto.Select();
                }
                else
                {
                    txtDocumentoCliente.Select();
                }
            }
        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {

            using (var modal = new mdProducto())
            {
                var result = modal.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if( txtNombreCliente.Text == "ACUARIO")
                    {
                        txtIdProducto.Text = modal._Producto.idProducto.ToString();
                        txtCodigoProducto.Text = modal._Producto.codigo;
                        txtProducto.Text = modal._Producto.nombre;
                        txtPrecio.Text = modal._Producto.precioCompra.ToString("0.00");
                        txtStock.Text = modal._Producto.stock.ToString();
                        txtCantidad.Select();

                    }

                    else
                    {
                        txtIdProducto.Text = modal._Producto.idProducto.ToString();
                        txtCodigoProducto.Text = modal._Producto.codigo;
                        txtProducto.Text = modal._Producto.nombre;
                        txtPrecio.Text = modal._Producto.precioVenta.ToString("0.00");
                        txtStock.Text = modal._Producto.stock.ToString();
                        txtCantidad.Select();

                    }
                    
                }
                else
                {
                    txtCodigoProducto.Select();
                }
            }
        }

        private void txtCodigoProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Producto oProducto = new CN_Producto().Listar().Where(p => p.codigo == txtCodigoProducto.Text && p.estado == true).FirstOrDefault();
                if (oProducto != null)
                {
                    if (txtNombreCliente.Text == "ACUARIO")
                    {
                        txtCodigoProducto.BackColor = Color.ForestGreen;
                        txtIdProducto.Text = oProducto.idProducto.ToString();
                        txtProducto.Text = oProducto.nombre;
                        txtPrecio.Text = oProducto.precioCompra.ToString("0.00");
                        txtStock.Text = oProducto.stock.ToString();
                        txtCantidad.Select();
                    }
                    else
                    {
                        txtCodigoProducto.BackColor = Color.ForestGreen;
                        txtIdProducto.Text = oProducto.idProducto.ToString();
                        txtProducto.Text = oProducto.nombre;
                        txtPrecio.Text = oProducto.precioVenta.ToString("0.00");
                        txtStock.Text = oProducto.stock.ToString();
                        txtCantidad.Select();
                    }
                    
                    
                    
                }
                else
                {
                    txtCodigoProducto.BackColor = Color.IndianRed;
                    txtIdProducto.Text = "0";
                    txtProducto.Text = "";
                    txtPrecio.Text = "";
                    txtStock.Text = "";
                    txtCantidad.Value = 1;



                }
            }
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {

            decimal precio = 0;
            bool producto_existe = false;

            if (int.Parse(txtIdProducto.Text) == 0)
            {
                MessageBox.Show("Debe Seleccionar un Producto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out precio))
            {
                MessageBox.Show("Precio  - Formato Moneda incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPrecio.Select();
                return;
            }

            if (Convert.ToInt32(txtStock.Text) < Convert.ToInt32(txtCantidad.Value.ToString()))
            {
                MessageBox.Show("La cantidad ingresada deber ser menor al Stock Fisico", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return;
            }

            foreach (DataGridViewRow fila in dgvData.Rows)
            {
                if (fila.Cells["idProducto"].Value.ToString() == txtIdProducto.Text)
                {
                    producto_existe = true;
                    break;
                }

            }
            if (!producto_existe)
            {

                bool respuesta = new CN_Venta().RestarStock(Convert.ToInt32(txtIdProducto.Text), Convert.ToInt32(txtCantidad.Value.ToString()));


                if (respuesta)
                {
                    dgvData.Rows.Add(new object[]{
                    txtIdProducto.Text,
                    txtProducto.Text,
                    precio.ToString("0.00"),
                    txtCantidad.Value.ToString(),
                    (txtCantidad.Value * precio).ToString("0.00")
                });
                    calcularTotal();
                    limpiarProducto();
                    txtCodigoProducto.Select();

                }

            }
        }


        private void calcularTotal()
        {
            decimal total = 0;
            if (dgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    total += Convert.ToDecimal(row.Cells["SubTotal"].Value.ToString() + Convert.ToDecimal(txtDiferencia.Text));

                }
                txtTotalAPagar.Text = total.ToString("0.00");
            }
        }

        private void limpiarProducto()
        {
            txtIdProducto.Text = "0";
            txtProducto.Text = "";
            txtCodigoProducto.BackColor = Color.White;
            txtCodigoProducto.Text = "";
            txtPrecio.Text = "";
            txtStock.Text = "";
            txtCantidad.Value = 1;
        }

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 5)
            {

                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.trash25.Width;
                var h = Properties.Resources.trash25.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Width - h) / 2;
                e.Graphics.DrawImage(Properties.Resources.trash25, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    bool respuesta = new CN_Venta().SumarStock(Convert.ToInt32(dgvData.Rows[indice].Cells["idProducto"].Value.ToString()), Convert.ToInt32(dgvData.Rows[indice].Cells["cantidad"].Value.ToString()));


                    if (respuesta)
                    {
                        dgvData.Rows.RemoveAt(indice);
                        calcularTotal();
                    }



                }

            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {

                e.Handled = false;
            }
            else
            {
                if (txtPrecio.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true;
                }
                else
                {
                    if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void txtPagaCon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {

                e.Handled = false;
            }
            else
            {
                if (txtPagaCon.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true;
                }
                else
                {
                    if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }


        private void CalcularCambio()
        {
            if (txtTotalAPagar.Text.Trim() == "")
            {
                MessageBox.Show("No existen productos en la venta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            decimal pagacon;
            decimal total = Convert.ToDecimal(txtTotalAPagar.Text);

            if (txtPagaCon.Text.Trim() == "")
            {
                txtPagaCon.Text = "0";
            }

            if (decimal.TryParse(txtPagaCon.Text.Trim(), out pagacon))
            {
                if (pagacon < total)
                {
                    txtCambioCliente.Text = "0.00";

                }
                else
                {
                    decimal cambio = pagacon - total;
                    txtCambioCliente.Text = cambio.ToString("0.00");
                }
            }
        }

        private void txtPagaCon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                CalcularCambio();
            }
        }

        private void btnRegistrarCompra_Click(object sender, EventArgs e)
        {
            if (txtDocumentoCliente.Text == "")
            {
                MessageBox.Show("Debe ingresar el documento del cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtNombreCliente.Text == "")
            {
                MessageBox.Show("Debe ingresar el nombre del cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (dgvData.Rows.Count < 1)
            {
                MessageBox.Show("Debe ingresar productos en la Venta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (checkDescuento.Checked && txtDescuento.Text == "")
            {
                MessageBox.Show("Debe ingresar un porcentaje de descuento", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            


            DataTable detalle_venta = new DataTable();

            detalle_venta.Columns.Add("idProducto", typeof(int));
            detalle_venta.Columns.Add("precioVenta", typeof(decimal));
            detalle_venta.Columns.Add("cantidad", typeof(int));
            detalle_venta.Columns.Add("subTotal", typeof(decimal));


            foreach (DataGridViewRow row in dgvData.Rows)
            {

                detalle_venta.Rows.Add(
                    new object[]
                    {
                        Convert.ToInt32(row.Cells["idProducto"].Value.ToString()),

                        row.Cells["precio"].Value.ToString(),
                        row.Cells["cantidad"].Value.ToString(),
                        row.Cells["subTotal"].Value.ToString()
                    });
            }

            int idCorrelativo = new CN_Venta().ObtenerCorrelativo();
            string numeroDocumento = string.Format("{0:00000}", idCorrelativo);
            CalcularCambio();


            Venta oVenta = new Venta()
            {
                oUsuario = new Usuario() { idUsuario = _Usuario.idUsuario },

                tipoDocumento = ((OpcionCombo)cboTipoDocumento.SelectedItem).Texto,
                nroDocumento = numeroDocumento,
                documentoCliente = txtDocumentoCliente.Text,
                nombreCliente = txtNombreCliente.Text,
                montoPago = Convert.ToDecimal(txtPagaCon.Text),
                montoCambio = Convert.ToDecimal(txtCambioCliente.Text),
                montoTotal = Convert.ToDecimal(txtTotalAPagar.Text),
                formaPago = ((OpcionCombo)cboFormaPago.SelectedItem).Texto,
                descuento = Convert.ToDecimal(txtDescuento.Text),
                montoDescuento = Convert.ToDecimal(txtMontoDescuento.Text),
                fechaRegistro = dtpFecha.Value.ToString()

            };

            string mensaje = string.Empty;
            bool respuesta = new CN_Venta().Registrar(oVenta, detalle_venta, out mensaje);
            if (respuesta)
            {
                var result = MessageBox.Show("Numero de Venta Generado:\n" + numeroDocumento, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                    Clipboard.SetText(numeroDocumento);
                string nombreCliente = txtNombreCliente.Text;
                txtDocumentoCliente.Text = "";
                txtNombreCliente.Text = "";
                dgvData.Rows.Clear();
                calcularTotal();
                txtPagaCon.Text = "";
                txtCambioCliente.Text = "";
                
                
                txtDescuento.Text = "0";
                //txtMontoDescuento.Text = "0";
                checkDescuento.Checked = false;

                
                    List<CajaRegistradora> lista = new CN_CajaRegistradora().Listar();

                    CajaRegistradora cajaAbierta = lista.Where(c => c.estado == true).FirstOrDefault();

                    if (cajaAbierta != null)

                    {

                    if (nombreCliente == "ACUARIO")
                    {

                        TransaccionCaja objTransaccion = new TransaccionCaja()
                        {
                            idCajaRegistradora = cajaAbierta.idCajaRegistradora,

                            hora = dtpFecha.Value.ToString(),
                            tipoTransaccion = "RETIRO AL COSTO",
                            monto = Convert.ToDecimal(txtTotalAPagar.Text),
                            docAsociado = "VENTA: " + numeroDocumento + " - CLIENTE: " + nombreCliente + " - FORMA DE PAGO: " + cboFormaPago.Text,
                            usuarioTransaccion = Environment.GetEnvironmentVariable("usuario")
                        };




                        int idTransaccionGenerado = new CN_Transaccion().RegistrarMovimiento(objTransaccion, out mensaje);

                    }
                    else
                    {
                        TransaccionCaja objTransaccion = new TransaccionCaja()
                        {
                            idCajaRegistradora = cajaAbierta.idCajaRegistradora,

                            hora = dtpFecha.Value.ToString(),
                            tipoTransaccion = "ENTRADA",
                            monto = Convert.ToDecimal(txtTotalAPagar.Text),
                            docAsociado = "VENTA: " + numeroDocumento + " - CLIENTE: " + nombreCliente + " - FORMA DE PAGO: " + cboFormaPago.Text,
                            usuarioTransaccion = Environment.GetEnvironmentVariable("usuario")
                        };




                        int idTransaccionGenerado = new CN_Transaccion().RegistrarMovimiento(objTransaccion, out mensaje);


                    }
                        

                        txtTotalAPagar.Text = "0";
                        cboFormaPago.SelectedIndex = 0;
                        txtDiferencia.Text = "0";
                    txtDiferencia.Enabled = true;

                }
            }
            else
            {
                MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void checkDescuento_CheckedChanged(object sender, EventArgs e)
        {
            if (checkDescuento.Checked)
            {
                txtDescuento.Visible = true;
                txtDescuento.Enabled = true;
                txtDescuento.Text = "";
                lblPorcentaje.Visible = true;
                txtMontoDescuento.Text = "";



            }


            else
            {
                txtDescuento.Visible = false;
                txtMontoDescuento.Text = "0";
                txtMontoDescuento.Visible = false;
                txtDescuento.Text = "0";
                lblPorcentaje.Visible = false;
                lblDescuento.Visible = false;
                calcularTotal();
            }

        }

        private void txtDescuento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {

                if (Convert.ToDecimal(txtDescuento.Text) > 0 && Convert.ToDecimal(txtDescuento.Text) <= 100 && (txtDescuento.Text != ""))
                {
                    txtMontoDescuento.Visible = true;
                    txtMontoDescuento.Enabled = false;
                    decimal montoDescuento = (Convert.ToDecimal(txtTotalAPagar.Text) * Convert.ToDecimal(txtDescuento.Text)) / 100;
                    txtMontoDescuento.Text = montoDescuento.ToString("0.00");
                    txtTotalAPagar.Text = (Convert.ToDecimal(txtTotalAPagar.Text) - montoDescuento).ToString("0.00");
                    txtDescuento.Enabled = false;
                    lblDescuento.Visible = true;
                }
                else
                {
                    txtMontoDescuento.Visible = false;
                    MessageBox.Show("Ingrese un valor entre 1 y 100 para el porcentaje de descuento", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
            }
        }

        private void txtDiferencia_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {

                if (txtDiferencia.Text != "" )
                {
                    decimal diferencia = Convert.ToDecimal(txtDiferencia.Text);
                    //txtMontoDescuento.Visible = true;
                    //txtMontoDescuento.Enabled = false;
                    //decimal montoDescuento = (Convert.ToDecimal(txtTotalAPagar.Text) * Convert.ToDecimal(txtDescuento.Text)) / 100;
                    //txtMontoDescuento.Text = montoDescuento.ToString("0.00");
                    txtTotalAPagar.Text = (Convert.ToDecimal(txtTotalAPagar.Text) + diferencia).ToString("0.00");
                    txtDiferencia.Enabled = false;
                    //txtDescuento.Enabled = false;
                    //lblDescuento.Visible = true;
                }
                else
                {
                    
                    MessageBox.Show("Ingrese un valor si existe una diferencia o 0 si no lo existe", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
            }
        }
    }
}