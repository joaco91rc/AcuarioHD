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
    public partial class frmDetalleCompra : Form
    {
        public frmDetalleCompra()
        {
            InitializeComponent();
        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            Compra oCompra = new CN_Compra().ObtenerCompra(txtBuscarCompra.Text);

            if(oCompra.idCompra != 0)
            {
                txtnroDocumento.Text = oCompra.nroDocumento;
                dtpFecha.Text = oCompra.fechaRegistro;
                cboTipoDocumento.Text = oCompra.tipoDocumento;
                txtUsuario.Text = oCompra.oUsuario.nombreCompleto;
                txtCUIT.Text = oCompra.oProveedor.documento;
                txtRazonSocial.Text = oCompra.oProveedor.razonSocial;

                dgvData.Rows.Clear();

                foreach ( DetalleCompra dc in oCompra.oDetalleCompra)
                {

                    dgvData.Rows.Add(new object[] { dc.oProducto.nombre, dc.precioCompra, dc.cantidad, dc.montoTotal });
                }
                txtTotalAPagar.Text = oCompra.montoTotal.ToString("0.00");
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            dtpFecha.Value = DateTime.Now;
            cboTipoDocumento.SelectedItem = 0;
            txtUsuario.Text = "";
            txtCUIT.Text = "";
            txtRazonSocial.Text = "";
            dgvData.Rows.Clear();
            txtTotalAPagar.Text = "0.00";
            txtnroDocumento.Text = "";
            txtBuscarCompra.Text = "";
        }

        private void btnDescargarPDF_Click(object sender, EventArgs e)
        {
            if (txtCUIT.Text == "")
            {
                MessageBox.Show("No se encontraron resultados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string textoHtml = Properties.Resources.PlantillaCompra.ToString();
            Negocio odatos = new CN_Negocio().ObtenerDatos();

            textoHtml = textoHtml.Replace("@nombrenegocio", odatos.nombre.ToUpper());
            textoHtml = textoHtml.Replace("@docnegocio", odatos.CUIT.ToUpper());
            textoHtml = textoHtml.Replace("@direcnegocio", odatos.direccion.ToUpper());

            textoHtml = textoHtml.Replace("@tipodocumento", cboTipoDocumento.Text.ToString().ToUpper());
            textoHtml = textoHtml.Replace("@numerodocumento", txtnroDocumento.Text.ToUpper());

            textoHtml = textoHtml.Replace("@docproveedor", txtCUIT.Text);
            textoHtml = textoHtml.Replace("@nombreproveedor", txtRazonSocial.Text);
            textoHtml = textoHtml.Replace("@fecharegistro", dtpFecha.Text);
            textoHtml = textoHtml.Replace("@usuarioregistro", txtUsuario.Text);


            string filas = string.Empty;

            foreach (DataGridViewRow row in dgvData.Rows)
            {

                filas += "<tr>";
                filas += "<td>" + row.Cells["producto"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["precioCompra"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["cantidad"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["subTotal"].Value.ToString() + "</td>";
                filas += "</tr>";

            }

            textoHtml = textoHtml.Replace("@filas", filas);
            textoHtml = textoHtml.Replace("@montototal", txtTotalAPagar.Text);


            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = string.Format("Orden de Compra nro {0}.pdf", txtnroDocumento.Text);
            saveFile.Filter = "Pdf Files | *.pdf";
        

            if(saveFile.ShowDialog()== DialogResult.OK)
            {
                using (FileStream stream = new FileStream(saveFile.FileName, FileMode.Create))
                {
                    Document pdfdoc = new Document(PageSize.A4, 25, 25, 25, 25);
                    PdfWriter writer = PdfWriter.GetInstance(pdfdoc, stream);
                    pdfdoc.Open();

                    bool obtenido = true;
                    byte[] byteImage = new CN_Negocio().ObtenerLogo( out obtenido);
                    if (obtenido)
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(byteImage);
                        img.ScaleToFit(110,110);
                        img.Alignment = iTextSharp.text.Image.UNDERLYING;
                        img.SetAbsolutePosition(pdfdoc.Left,pdfdoc.GetTop(70));
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
                    txtCUIT.Text = "";
                    txtRazonSocial.Text = "";
                    dgvData.Rows.Clear();
                    txtTotalAPagar.Text = "0.00";
                    txtnroDocumento.Text = "";
                    txtBuscarCompra.Text = "";

                }
            }
        
        }

        private void txtBuscarCompra_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                Compra oCompra = new CN_Compra().ObtenerCompra(txtBuscarCompra.Text);

                if (oCompra.idCompra != 0)
                {
                    txtnroDocumento.Text = oCompra.nroDocumento;
                    dtpFecha.Text = oCompra.fechaRegistro;
                    cboTipoDocumento.Text = oCompra.tipoDocumento;
                    txtUsuario.Text = oCompra.oUsuario.nombreCompleto;
                    txtCUIT.Text = oCompra.oProveedor.documento;
                    txtRazonSocial.Text = oCompra.oProveedor.razonSocial;

                    dgvData.Rows.Clear();

                    foreach (DetalleCompra dc in oCompra.oDetalleCompra)
                    {

                        dgvData.Rows.Add(new object[] { dc.oProducto.nombre, dc.precioCompra, dc.cantidad, dc.montoTotal });
                    }
                    txtTotalAPagar.Text = oCompra.montoTotal.ToString("0.00");
                }
            }
        }

        private void frmDetalleCompra_Load(object sender, EventArgs e)
        {
            txtBuscarCompra.Select();
        }
    }
}

