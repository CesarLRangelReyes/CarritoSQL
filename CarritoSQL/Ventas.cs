using System;
using System.Data;
using System.Windows.Forms;

namespace CarritoSQL
{
    public partial class Ventas : Form
    {
        Carrito car = new Carrito();
        DataTable dato = new DataTable();
        double total;
        int fila = 0;
        public Ventas()
        {
            InitializeComponent();
            NombreCampos();
            MostrarProducto();
            NumFolio();
        }
        public void NombreCampos()
        {
            dato.Columns.Add("Codigo Producto");
            dato.Columns.Add("Producto");
            dato.Columns.Add("Marca");
            dato.Columns.Add("Cantidad a Vender");
            dato.Columns.Add("Costo");
            dato.Columns.Add("Total");
            dgvCarrito.DataSource = dato;
        }
        public void MostrarProducto()
        {
            txtProductos.ValueMember = "Nombre_Producto";
            txtProductos.DisplayMember = "Nombre_Producto";
            txtProductos.DataSource = car.NombreProducto();
        }
        public void NumFolio() {
            Random folio = new Random();
            int fol = folio.Next(1000,20000);
            txtFolio.Text = Convert.ToString(fol);
        }

        private void txtProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtProductos.SelectedValue.ToString() != null)
            {
                car.Producto = txtProductos.SelectedValue.ToString();
                txtMarca.ValueMember = "Marca";
                txtMarca.DisplayMember = "Marca";
                txtMarca.DataSource = car.NombreMarca();
            }
        }

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            car.Codigo = txtCodigo.Text;
            car.CodigoProducto();
            txtProductos.Text = car.Producto;
            txtMarca.Text = car.Marca;
            txtProductos.Enabled = false;
            txtMarca.Enabled = false;
        }

        private void Agregar_Click(object sender, EventArgs e)
        {
            car.Codigo = txtCodigo.Text;
            if (car.CantidadProducto() > Convert.ToInt32(txtNumerico.Value) && txtNumerico.Value > 0)
            {
                car.AgregarProducto();
                DataRow row = dato.NewRow();
                row["Codigo Producto"] = txtCodigo.Text;
                row["Producto"] = car.Producto;
                row["Marca"] = car.Marca;
                row["Cantidad a Vender"] = txtNumerico.Value.ToString();
                row["Costo"] = car.Costo;
                row["Total"] = Convert.ToInt32(txtNumerico.Value.ToString()) * car.Costo;
                dato.Rows.Add(row);

                total = total + Convert.ToInt32(txtNumerico.Value.ToString()) * car.Costo;
                lbTotal.Text = Convert.ToString(total);
                int modificar;
                modificar = car.Venta - Convert.ToInt32(txtNumerico.Value.ToString());
                car.Venta = modificar;
                car.RestarSumarProdcuto();

                car.Folio = Convert.ToInt32(txtFolio.Text);
                car.Fecha = txtFecha.Text;
                car.Codigo = txtCodigo.Text;
                car.Producto = txtProductos.Text;
                car.Marca = txtMarca.Text;
                car.Venta = Convert.ToInt32(txtNumerico.Value.ToString());
                car.Costo = car.Costo;
                double all = 0;
                all = all + Convert.ToInt32(txtNumerico.Value.ToString()) * car.Costo;
                car.Total = Convert.ToInt32(all);
                car.AddFolio();

                txtCodigo.Text = "";
                txtProductos.Enabled = true;
                txtMarca.Enabled = true;
                txtProductos.Text = "------Seleccione Producto------";
                txtMarca.Text = "--------Seleccina la Marca-------";
                txtNumerico.Value = 0;
            }
            else
            {
                MessageBox.Show("La cantidad existente es: " + car.Venta);
            }
        }

        private void Eliminar_Click(object sender, EventArgs e)
        {
            if (dgvCarrito.SelectedRows.Count > 0)
            {
                string codigo = dgvCarrito.CurrentRow.Cells[0].Value.ToString();
                int cantidad = Convert.ToInt32(dgvCarrito.CurrentRow.Cells[3].Value.ToString());
                int costo = Convert.ToInt32(dgvCarrito.CurrentRow.Cells[4].Value.ToString());
                double all = cantidad * costo;
                car.Codigo = codigo;
                car.CantidadProducto();

                int res = car.Venta + cantidad;
                car.Venta = res;
                car.RestarSumarProdcuto();
                car.DeleteFolio();

                total = total - all;
                lbTotal.Text = Convert.ToString(total);
                int fil = dgvCarrito.CurrentRow.Index;
                dgvCarrito.Rows.RemoveAt(fil);
            }
        }

        private void txtEfectivo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float total;
                float devolucion;
                total = float.Parse(lbTotal.Text);
                devolucion = float.Parse(txtEfectivo.Text) - total;
                lbDevolucion.Text = Convert.ToString(devolucion);
            }
            catch {
                lbDevolucion.Text = "0.00";
            }
        }

        private void txtVender_Click(object sender, EventArgs e)
        {
            Factura.CreaTicket Ticket1 = new Factura.CreaTicket();

            Ticket1.TextoCentro("Empresa xxxxx "); //imprime una linea de descripcion
            Ticket1.TextoCentro("**********************************");

            Ticket1.TextoIzquierda("Dirc: xxxx");
            Ticket1.TextoIzquierda("Tel:xxxx ");
            Ticket1.TextoIzquierda("Rnc: xxxx");
            Ticket1.TextoIzquierda("");
            Ticket1.TextoCentro("Factura de Venta"); //imprime una linea de descripcion
            Ticket1.TextoIzquierda("No Fac:" + txtFolio.Text);
            Ticket1.TextoIzquierda("Fecha:" + DateTime.Now.ToShortDateString() + " Hora:" + DateTime.Now.ToShortTimeString());
            Ticket1.TextoIzquierda("Le Atendio: xxxx");
            Ticket1.TextoIzquierda("");
            Factura.CreaTicket.LineasGuion();

            Factura.CreaTicket.EncabezadoVenta();
            Factura.CreaTicket.LineasGuion();
            foreach (DataGridViewRow r in dgvCarrito.Rows)
            {
                // PROD                     //PrECIO                                    CANT                         TOTAL
                Ticket1.AgregaArticulo(r.Cells[1].Value.ToString(), int.Parse(r.Cells[3].Value.ToString()), double.Parse(r.Cells[4].Value.ToString()), double.Parse(r.Cells[5].Value.ToString())); //imprime una linea de descripcion

            }

            Factura.CreaTicket.LineasGuion();
            Ticket1.TextoIzquierda(" ");
            Ticket1.AgregaTotales("Total", double.Parse(lbTotal.Text)); // imprime linea con total
            Ticket1.TextoIzquierda(" ");
            Ticket1.AgregaTotales("Efectivo Entregado:", double.Parse(txtEfectivo.Text));
            Ticket1.AgregaTotales("Efectivo Devuelto:", double.Parse(lbDevolucion.Text));

            // Ticket1.LineasTotales(); // imprime linea 

            Ticket1.TextoIzquierda(" ");
            Ticket1.TextoCentro("**********************************");
            Ticket1.TextoCentro("*     Gracias por preferirnos    *");

            Ticket1.TextoCentro("**********************************");
            Ticket1.TextoIzquierda(" ");
            string impresora = "Microsoft XPS Document Writer";
            Ticket1.ImprimirTiket(impresora);
            MessageBox.Show("Gracias por preferirnos");

            fila = 0;
            while (dgvCarrito.RowCount > 0) {
                dgvCarrito.Rows.Remove(dgvCarrito.CurrentRow);
            }
            NumFolio();
            lbDevolucion.Text = lbTotal.Text = "0.00";
        }
    }
}
