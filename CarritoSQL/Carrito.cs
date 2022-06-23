using System;
using System.Data.SqlClient;
using System.Data;

namespace CarritoSQL
{
    public class Carrito
    {
        private string codigo;
        private string producto;
        private string marca;
        private int venta;
        private double costo;
        private int total;
        private int folio;
        private string fecha;

        SqlConnection conexion = new SqlConnection("server = DESKTOP-N1Q1432; database= Pimaria_Y_Foranea; integrated security = true");

        public string Codigo { get => codigo; set => codigo = value; }
        public string Producto { get => producto; set => producto = value; }
        public string Marca { get => marca; set => marca = value; }
        public int Venta { get => venta; set => venta = value; }
        public double Costo { get => costo; set => costo = value; }
        public int Total { get => total; set => total = value; }
        public int Folio { get => folio; set => folio = value; }
        public string Fecha { get => fecha; set => fecha = value; }

        public DataTable NombreProducto()
        {
            DataTable tabla = new DataTable();
            conexion.Open();
            SqlCommand cmd = new SqlCommand("select Nombre_Producto from Producto group by Nombre_Producto", conexion);
            SqlDataAdapter leer = new SqlDataAdapter(cmd);
            leer.Fill(tabla);
            conexion.Close();
            DataRow fila = tabla.NewRow();
            fila["Nombre_Producto"] = "------Seleccione Producto------";
            tabla.Rows.InsertAt(fila, 0);
            return tabla;
        }
        public DataTable NombreMarca()
        {
            DataTable tabla = new DataTable();
            conexion.Open();
            SqlCommand cmd = new SqlCommand("select Marca from Producto where Nombre_Producto = @producto group by Marca", conexion);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@producto", Producto);
            SqlDataAdapter leer = new SqlDataAdapter(cmd);
            leer.Fill(tabla);
            conexion.Close();
            DataRow fila = tabla.NewRow();
            fila["Marca"] = "--------Seleccina la Marca-------";
            tabla.Rows.InsertAt(fila, 0);
            return tabla;
        }
        public void CodigoProducto()
        {
            conexion.Open();
            SqlCommand cmd = new SqlCommand("select Nombre_Producto, Marca from Producto where Codigo_Producto = @codigo", conexion);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@codigo", Codigo);
            SqlDataReader leer = cmd.ExecuteReader();
            while (leer.Read())
            {
                Producto = leer["Nombre_Producto"].ToString();
                Marca = leer["Marca"].ToString();
            }
            conexion.Close();
        }
        public int CantidadProducto()
        {
            conexion.Open();
            SqlCommand cmd = new SqlCommand("select Cantidad_Producto from Producto where Codigo_Producto = @codigo", conexion);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@codigo", Codigo);
            SqlDataReader leer = cmd.ExecuteReader();
            while (leer.Read())
            {
                Venta = Convert.ToInt32(leer["Cantidad_Producto"]);
            }
            conexion.Close();
            return Venta;
        }
        public void AgregarProducto()
        {
            conexion.Open();
            SqlCommand cmd = new SqlCommand("select Nombre_Producto, Marca, Costo_Venta from Producto where Codigo_Producto = @codigo", conexion);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@codigo", Codigo);
            SqlDataReader leer = cmd.ExecuteReader();
            while (leer.Read())
            {
                Producto = leer["Nombre_Producto"].ToString();
                Marca = leer["Marca"].ToString();
                Costo = Convert.ToDouble(leer["Costo_Venta"].ToString());
            }
            conexion.Close();
        }
        public void RestarSumarProdcuto()
        {
            conexion.Open();
            SqlCommand cmd = new SqlCommand("update Producto set Cantidad_Producto = @cantidad where Codigo_Producto = @codigo", conexion);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@cantidad", Venta);
            cmd.Parameters.AddWithValue("@codigo", Codigo);
            cmd.ExecuteNonQuery();
            conexion.Close();
        }
        public void AddFolio() {
            conexion.Open();
            SqlCommand cmd = new SqlCommand("insert into Folios values(@folio,@fecha,@codigo,@producto,@marca,@venta,@costo,@total)",conexion);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@folio",Folio);
            cmd.Parameters.AddWithValue("@fecha", Convert.ToDateTime(Fecha));
            cmd.Parameters.AddWithValue("@codigo",Codigo);
            cmd.Parameters.AddWithValue("@producto",Producto);
            cmd.Parameters.AddWithValue("@marca",Marca);
            cmd.Parameters.AddWithValue("@venta",Venta);
            cmd.Parameters.AddWithValue("@costo",Costo);
            cmd.Parameters.AddWithValue("@total",Total);
            cmd.ExecuteNonQuery();
            conexion.Close();
        }
        public void DeleteFolio() {
            conexion.Open();
            SqlCommand cmd = new SqlCommand("delete from Folios where Codigo_Producto = @codigo",conexion);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@codigo", Codigo);
            cmd.ExecuteNonQuery();
            conexion.Close();
        }
    }
}
