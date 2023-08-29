using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using ClosedXML.Excel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
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
    public partial class frmProducto : Form
    {
        public Dictionary<string, int> diccionarioCategorias = new Dictionary<string, int>();

        List<Categoria> listaCategorias = new CN_Categoria().Listar();
            

    public frmProducto()
        {
            InitializeComponent();
        }

        private void frmProducto_Load(object sender, EventArgs e)
        {
            foreach (Categoria categoria in listaCategorias)
            {
                diccionarioCategorias[categoria.descripcion] = categoria.idCategoria;
            }



            cboEstado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "No Activo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            List<Categoria> listaCategoria = new CN_Categoria().Listar();

            foreach (Categoria item in listaCategoria)
            {
                cboCategoria.Items.Add(new OpcionCombo() { Valor = item.idCategoria, Texto = item.descripcion });
            }
            cboCategoria.DisplayMember = "Texto";
            cboCategoria.ValueMember = "Valor";
            cboCategoria.SelectedIndex = 0;

            foreach (DataGridViewColumn columna in dgvData.Columns)
            {

                if (columna.Visible == true && columna.Name != "btnSeleccionar")
                {
                    cboBusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });

                }


            }

            cboBusqueda.DisplayMember = "Texto";
            cboBusqueda.ValueMember = "Valor";
            cboBusqueda.SelectedIndex = 2;
           

            //Mostrar todos los Productos
            List<Producto> listaProducto = new CN_Producto().Listar();

            foreach (Producto item in listaProducto)
            {
                dgvData.Rows.Add(new object[] { "",item.idProducto,
                    item.codigo,
                    item.nombre,
                    item.descripcion,
                    item.oCategoria.idCategoria,
                    item.oCategoria.descripcion,
                    item.stock,
                    item.precioCompra,
                    item.precioVenta,

                    item.estado==true?1:0,
                    item.estado==true? "Activo": "No Activo"
                    });
            }
            txtBusqueda.Select();


        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            Producto objProducto = new Producto()
            {
                idProducto = Convert.ToInt32(txtIdProducto.Text),
                codigo = txtCodigo.Text,
                nombre = txtNombre.Text,
                descripcion = txtDescripcion.Text,

                oCategoria = new Categoria { idCategoria = Convert.ToInt32(((OpcionCombo)cboCategoria.SelectedItem).Valor) },
                estado = Convert.ToInt32(((OpcionCombo)cboEstado.SelectedItem).Valor) == 1 ? true : false,
                stock = Convert.ToInt32(txtStock.Text),
                precioCompra = Convert.ToDecimal(txtPrecioCompra.Text),
                precioVenta = Convert.ToDecimal(txtPrecioVenta.Text),
            };

            if (objProducto.idProducto == 0)
            {

                int idProductoGenerado = new CN_Producto().Registrar(objProducto, out mensaje);


                if (idProductoGenerado != 0)
                {
                    dgvData.Rows.Add(new object[] { "",
                        idProductoGenerado,
                        txtCodigo.Text,
                        txtNombre.Text,
                        txtDescripcion.Text,

                ((OpcionCombo)cboCategoria.SelectedItem).Valor.ToString(),
                ((OpcionCombo)cboCategoria.SelectedItem).Texto.ToString(),
                Convert.ToInt32(txtStock.Text),
                 Convert.ToDecimal(txtPrecioCompra.Text),
                 Convert.ToDecimal(txtPrecioVenta.Text),
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
            else
            {

                bool resultado = new CN_Producto().Editar(objProducto, out mensaje);
                if (resultado)
                {
                    DataGridViewRow row = dgvData.Rows[Convert.ToInt32(txtIndice.Text)];
                    row.Cells["idProducto"].Value = txtIdProducto.Text;
                    row.Cells["codigo"].Value = txtCodigo.Text;
                    row.Cells["nombre"].Value = txtNombre.Text;
                    row.Cells["descripcion"].Value = txtDescripcion.Text;
                    
                    row.Cells["idCategoria"].Value = ((OpcionCombo)cboCategoria.SelectedItem).Valor.ToString();
                    row.Cells["categoria"].Value = ((OpcionCombo)cboCategoria.SelectedItem).Texto.ToString();
                    row.Cells["estadoValor"].Value = ((OpcionCombo)cboEstado.SelectedItem).Valor.ToString();
                    row.Cells["estado"].Value = ((OpcionCombo)cboEstado.SelectedItem).Texto.ToString();
                    row.Cells["stock"].Value = txtStock.Text;
                    row.Cells["precioCompra"].Value = txtPrecioCompra.Text;
                    row.Cells["precioVenta"].Value = txtPrecioVenta.Text;

                    Limpiar();

                }
                else
                {

                    MessageBox.Show(mensaje);
                }

            }
        }

        private void Limpiar()
        {
            txtIndice.Text = "-1";
            txtIdProducto.Text = "0";
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtStock.Text = "";
            txtPrecioCompra.Text = "";
            txtPrecioVenta.Text = "";
            
            cboCategoria.SelectedIndex = 0;
            cboEstado.SelectedIndex = 0;
            txtCodigo.Select();
            txtProductoSeleccionado.Text = "Ninguno";
        }

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0)
            {

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
            if (dgvData.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    txtIndice.Text = indice.ToString();
                    txtProductoSeleccionado.Text = dgvData.Rows[indice].Cells["nombre"].Value.ToString();
                    txtIdProducto.Text = dgvData.Rows[indice].Cells["idProducto"].Value.ToString();
                    txtCodigo.Text = dgvData.Rows[indice].Cells["codigo"].Value.ToString();
                    txtNombre.Text = dgvData.Rows[indice].Cells["nombre"].Value.ToString();
                    txtDescripcion.Text = dgvData.Rows[indice].Cells["descripcion"].Value.ToString();
                    txtStock.Text = dgvData.Rows[indice].Cells["stock"].Value.ToString();
                    txtPrecioCompra.Text = dgvData.Rows[indice].Cells["precioCompra"].Value.ToString();
                    txtPrecioVenta.Text = dgvData.Rows[indice].Cells["precioVenta"].Value.ToString();


                    foreach (OpcionCombo oc in cboCategoria.Items)
                    {

                        if (Convert.ToInt32(oc.Valor) == (Convert.ToInt32(dgvData.Rows[indice].Cells["idCategoria"].Value)))
                        {
                            int indiceCombo = cboCategoria.Items.IndexOf(oc);
                            cboCategoria.SelectedIndex = indiceCombo;
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
            if (Convert.ToInt32(txtIdProducto.Text) != 0)
            {

                if (MessageBox.Show("Desea eliminar el Producto?", "Confirmar Eliminacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    Producto objProducto = new Producto()
                    {
                        idProducto = Convert.ToInt32(txtIdProducto.Text),

                    };

                    bool respuesta = new CN_Producto().Eliminar(objProducto, out mensaje);
                    if (respuesta)
                    {

                        dgvData.Rows.RemoveAt(Convert.ToInt32(txtIndice.Text));
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

            if (dgvData.Rows.Count > 0)
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
            foreach (DataGridViewRow row in dgvData.Rows)
                row.Visible = true;
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count < 1)
            {
                MessageBox.Show("No hay datos para Exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            else {
                DataTable dt = new DataTable();

                foreach (DataGridViewColumn columna in dgvData.Columns) {
                    if (columna.HeaderText == "ID PRODUCTO")
                        dt.Columns.Add(columna.HeaderText, typeof(string));
                    if (columna.HeaderText!= "" && columna.Visible )
                    {

                        
                        dt.Columns.Add(columna.HeaderText, typeof(string));

                    }
                }

                foreach (DataGridViewRow row in dgvData.Rows) {
                    if (row.Visible) {
                        dt.Rows.Add(new object[]
                        {   row.Cells[1].Value.ToString(),
                            row.Cells[2].Value.ToString(),
                            row.Cells[3].Value.ToString(),
                            row.Cells[4].Value.ToString(),
                            row.Cells[6].Value.ToString(),
                            row.Cells[7].Value.ToString(),
                            row.Cells[8].Value.ToString(),
                            row.Cells[9].Value.ToString(),
                            row.Cells[11].Value.ToString(),
                            

                        }); 
                    }
                    


                }
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.FileName = string.Format("ReporteProducto_{0}.xlsx", DateTime.Now.ToString("ddMMyyyyHHmmss"));
                saveFile.Filter = "Excel Files | *.xlsx";

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        XLWorkbook wb = new XLWorkbook();
                        var hoja = wb.Worksheets.Add(dt, "Informe Productos");
                        hoja.ColumnsUsed().AdjustToContents();
                        wb.SaveAs(saveFile.FileName);
                        MessageBox.Show("Planilla Exportada", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch
                    {
                        MessageBox.Show("Error al generar la Planilla de Excel", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }

            }
        }

        private void txtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                string columnaFiltro = ((OpcionCombo)cboBusqueda.SelectedItem).Valor.ToString();

                if (dgvData.Rows.Count > 0)
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
        }

        private void btnImportar_Click(object sender, EventArgs e)
        { 
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files | *.xls;*.xlsx";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = new XSSFWorkbook(fileStream);
                    ISheet sheet = workbook.GetSheetAt(0);
                    int indice = 0;
                    for (int rowIdx = 1; rowIdx <= sheet.LastRowNum; rowIdx++) // Comenzamos desde la segunda fila para omitir la cabecera
                    {
                        IRow row = sheet.GetRow(rowIdx);
                        if (row != null)
                        {
                            string mensaje = string.Empty;
                            Producto objProducto = CreateProductoFromExcelRow(row); // Crear objeto Producto desde la fila de Excel
                            
                            // Realiza la lógica de actualización de productos en la base de datos aquí
                            bool resultado = new CN_Producto().Editar(objProducto, out mensaje);
                            if (resultado)
                            {
                                DataGridViewRow rowDgvData = dgvData.Rows[indice];
                                rowDgvData.Cells["idProducto"].Value = objProducto.idProducto.ToString();
                                rowDgvData.Cells["codigo"].Value = objProducto.codigo.ToString();
                                rowDgvData.Cells["nombre"].Value = objProducto.nombre.ToString();
                                rowDgvData.Cells["descripcion"].Value = objProducto.descripcion.ToString();

                                rowDgvData.Cells["idCategoria"].Value = objProducto.oCategoria.idCategoria.ToString();
                                rowDgvData.Cells["categoria"].Value = objProducto.oCategoria.descripcion.ToString();
                                rowDgvData.Cells["estadoValor"].Value = 1.ToString();
                                rowDgvData.Cells["estado"].Value = "Activo";
                                rowDgvData.Cells["stock"].Value = objProducto.stock.ToString();
                                rowDgvData.Cells["precioCompra"].Value = objProducto.precioCompra.ToString();
                                rowDgvData.Cells["precioVenta"].Value = objProducto.precioVenta.ToString();
                                indice++;
                                Limpiar();

                            }
                            else
                            {

                                MessageBox.Show(mensaje);
                            }
                        }
                    }

                    MessageBox.Show("Datos importados correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private string GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;

            switch (cell.CellType)
            {
                case CellType.Numeric:
                    return cell.NumericCellValue.ToString();
                case CellType.String:
                    return cell.StringCellValue;
                    case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                default:
                    return string.Empty;
            }
        }

        private Producto CreateProductoFromExcelRow(IRow row)
        {
            string mensaje = string.Empty;
            Producto producto = new Producto();

            producto.idProducto = Convert.ToInt32(GetCellValue(row.GetCell(0)));
            producto.codigo = GetCellValue(row.GetCell(1));
            producto.nombre = GetCellValue(row.GetCell(2));
            producto.descripcion = GetCellValue(row.GetCell(3));
            producto.oCategoria = new Categoria();

            if (diccionarioCategorias.ContainsKey(GetCellValue(row.GetCell(4))))
            {
                int categoriaId = diccionarioCategorias[GetCellValue(row.GetCell(4))];

                producto.oCategoria.idCategoria = categoriaId;
                producto.oCategoria.descripcion = GetCellValue(row.GetCell(4));
            }else
            {
               
                Categoria categoriaNueva = new Categoria();
                
                categoriaNueva.descripcion = GetCellValue(row.GetCell(4));
                categoriaNueva.estado = true;

                int idCategoriaGenerado = new CN_Categoria().Registrar(categoriaNueva, out mensaje);
                if (idCategoriaGenerado == 0)
                {
                    MessageBox.Show(mensaje);
                }
                else 
                {
                    producto.oCategoria.idCategoria = idCategoriaGenerado;
                    producto.oCategoria.descripcion = categoriaNueva.descripcion;
                }


            }
                                    
            producto.estado = GetCellValue(row.GetCell(8))=="Activo"?true:false;
            producto.stock = Convert.ToInt32(GetCellValue(row.GetCell(5)));
            producto.precioCompra = Convert.ToDecimal(GetCellValue(row.GetCell(6)));
            producto.precioVenta = Convert.ToDecimal(GetCellValue(row.GetCell(7)));
            // ... ajusta para el resto de propiedades

            return producto;
        }



    }
}
