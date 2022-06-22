using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CarritoSQL
{
    public partial class Ventas : Form
    {
        Carrito car = new Carrito();
        DataTable dato = new DataTable();
        double total;
        public Ventas()
        {
            InitializeComponent();
            NombreCampos();
            MostrarProducto();
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
                txtCodigo.Text = "";
                txtProductos.Enabled = true;
                txtMarca.Enabled = true;
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
                total = total - all;
                lbTotal.Text = Convert.ToString(total);
                int fil = dgvCarrito.CurrentRow.Index;
                dgvCarrito.Rows.RemoveAt(fil);
            }
        }
    }
}
