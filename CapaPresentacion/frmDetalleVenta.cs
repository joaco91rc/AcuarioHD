using CapaEntidad;
using CapaNegocio;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
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
    public partial class frmDetalleVenta : Form
    {
        public frmDetalleVenta()
        {
            InitializeComponent();
        }

        private void frmDetalleVenta_Load(object sender, EventArgs e)
        {
            txtBuscarVenta.Select();
        }

        private void btnBuscarVenta_Click(object sender, EventArgs e)
        {
            Venta oVenta = new CN_Venta().ObtenerVenta(txtBuscarVenta.Text);
            if(oVenta.idVenta != 0)
            {
                txtnroDocumento.Text = oVenta.nroDocumento;
                dtpFecha.Text = oVenta.fechaRegistro;
                cboTipoDocumento.Text = oVenta.tipoDocumento;
                txtUsuario.Text = oVenta.oUsuario.nombreCompleto;
                txtDNI.Text = oVenta.documentoCliente;
                txtNombreCliente.Text = oVenta.nombreCliente;
                txtDescuento.Text = oVenta.descuento.ToString();
                txtMontoDescuento.Text = oVenta.montoDescuento.ToString();
                txtFormaDePago.Text = oVenta.formaPago.ToString();

                dgvData.Rows.Clear();
                foreach (DetalleVenta dv in oVenta.oDetalleVenta)
                {
                    dgvData.Rows.Add(new object[] { dv.oProducto.nombre, dv.precioVenta, dv.cantidad, dv.subTotal });
                }

                txtTotalAPagar.Text = oVenta.montoTotal.ToString("0.00");
                txtPagaCon.Text = oVenta.montoPago.ToString("0.00");
                txtCambio.Text = oVenta.montoCambio.ToString("0.00");
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            dtpFecha.Value = DateTime.Now;
            cboTipoDocumento.SelectedItem = 0;
            txtUsuario.Text = "";
            txtDNI.Text = "";
            txtNombreCliente.Text = "";
            dgvData.Rows.Clear();
            txtTotalAPagar.Text = "0.00";
            txtPagaCon.Text = "0.00";
            txtCambio.Text = "0.00";
            txtBuscarVenta.Text = "";
            txtFormaDePago.Text = "";
            txtBuscarVenta.Select();
        }

        private void btnDescargarPDF_Click(object sender, EventArgs e)
        {
            if (txtDNI.Text == "")
            {
                MessageBox.Show("No se encontraron resultados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string textoHtml = Properties.Resources.PlantillaVenta.ToString();
            Negocio odatos = new CN_Negocio().ObtenerDatos();

            textoHtml = textoHtml.Replace("@nombrenegocio", odatos.nombre.ToUpper());
            textoHtml = textoHtml.Replace("@docnegocio", odatos.CUIT.ToUpper());
            textoHtml = textoHtml.Replace("@direcnegocio", odatos.direccion.ToUpper());

            textoHtml = textoHtml.Replace("@tipodocumento", cboTipoDocumento.Text.ToString().ToUpper());
            textoHtml = textoHtml.Replace("@numerodocumento", txtnroDocumento.Text.ToUpper());

            textoHtml = textoHtml.Replace("@doccliente", txtDNI.Text);
            textoHtml = textoHtml.Replace("@nombrecliente", txtNombreCliente.Text);
            textoHtml = textoHtml.Replace("@fecharegistro", dtpFecha.Text);
            textoHtml = textoHtml.Replace("@usuarioregistro", txtUsuario.Text);


            string filas = string.Empty;

            foreach (DataGridViewRow row in dgvData.Rows)
            {

                filas += "<tr>";
                filas += "<td>" + row.Cells["producto"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["precioVenta"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["cantidad"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["subTotal"].Value.ToString() + "</td>";
                filas += "</tr>";

            }

            textoHtml = textoHtml.Replace("@filas", filas);
            textoHtml = textoHtml.Replace("@descuento", txtDescuento.Text);
            textoHtml = textoHtml.Replace("@montodescuento", txtMontoDescuento.Text);
            textoHtml = textoHtml.Replace("@montototal", txtTotalAPagar.Text);
            textoHtml = textoHtml.Replace("@pagocon", txtPagaCon.Text);
            textoHtml = textoHtml.Replace("@cambio", txtCambio.Text);
            textoHtml = textoHtml.Replace("@formapago", txtFormaDePago.Text);


            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = string.Format("Orden de Venta nro {0}.pdf", txtnroDocumento.Text);
            saveFile.Filter = "Pdf Files | *.pdf";


            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(saveFile.FileName, FileMode.Create))
                {
                    Document pdfdoc = new Document(PageSize.A4, 25, 25, 25, 25);
                    PdfWriter writer = PdfWriter.GetInstance(pdfdoc, stream);
                    pdfdoc.Open();

                    bool obtenido = true;
                    byte[] byteImage = new CN_Negocio().ObtenerLogo(out obtenido);
                    if (obtenido)
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(byteImage);
                        img.ScaleToFit(110, 110);
                        img.Alignment = iTextSharp.text.Image.UNDERLYING;
                        img.SetAbsolutePosition(pdfdoc.Left, pdfdoc.GetTop(70));
                        pdfdoc.Add(img);
                    }

                    using (StringReader sr = new StringReader(textoHtml))
                    {

                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfdoc, sr);

                    }

                    pdfdoc.Close();
                    stream.Close();

                    MessageBox.Show("Documento Generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtpFecha.Value = DateTime.Now;
                    cboTipoDocumento.SelectedItem = 0;
                    txtUsuario.Text = "";
                    txtDNI.Text = "";
                    txtNombreCliente.Text = "";
                    dgvData.Rows.Clear();
                    txtTotalAPagar.Text = "0.00";
                    txtPagaCon.Text = "0.00";
                    txtCambio.Text = "0.00";
                    txtBuscarVenta.Text = "";
                    txtFormaDePago.Text = "";
                    txtBuscarVenta.Select();

                }
            }

        }

        private void txtBuscarVenta_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {

                Venta oVenta = new CN_Venta().ObtenerVenta(txtBuscarVenta.Text);
                if (oVenta.idVenta != 0)
                {
                    txtnroDocumento.Text = oVenta.nroDocumento;
                    dtpFecha.Text = oVenta.fechaRegistro;
                    cboTipoDocumento.Text = oVenta.tipoDocumento;
                    txtUsuario.Text = oVenta.oUsuario.nombreCompleto;
                    txtDNI.Text = oVenta.documentoCliente;
                    txtNombreCliente.Text = oVenta.nombreCliente;
                    txtDescuento.Text = oVenta.descuento.ToString();
                    txtMontoDescuento.Text = oVenta.montoDescuento.ToString();
                    txtFormaDePago.Text = oVenta.formaPago.ToString();

                    dgvData.Rows.Clear();
                    foreach (DetalleVenta dv in oVenta.oDetalleVenta)
                    {
                        dgvData.Rows.Add(new object[] { dv.oProducto.nombre, dv.precioVenta, dv.cantidad, dv.subTotal });
                    }

                    txtTotalAPagar.Text = oVenta.montoTotal.ToString("0.00");
                    txtPagaCon.Text = oVenta.montoPago.ToString("0.00");
                    txtCambio.Text = oVenta.montoCambio.ToString("0.00");
                }
            }
        }
    }
    }

