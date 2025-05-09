using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static string connectionString = "Server=localhost;Database=Comercio;Trusted_Connection=True;";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Menú CRUD:");
                Console.WriteLine("1 - Agregar producto");
                Console.WriteLine("2 - Listar productos");
                Console.WriteLine("3 - Modificar producto");
                Console.WriteLine("4 - Eliminar producto");
                Console.WriteLine("0 - Salir");
                Console.Write("Opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1": AgregarProducto(); break;
                    case "2": ListarProducto(); break;
                    case "3": ModificarProducto(); break;
                    case "4": EliminarProducto(); break;
                    case "0": return;
                    default: Console.WriteLine("Opción inválida."); break;
                }

                Console.WriteLine("\nPresione una tecla para continuar...");
                Console.ReadKey();

            }
        }
        static void AgregarProducto()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                Console.Write("Nombre: ");
                string nombre = Console.ReadLine();
                Console.Write("Descripción: ");
                string descripcion = Console.ReadLine();
                Console.Write("Precio: ");
                decimal precio = Convert.ToDecimal(Console.ReadLine());
                Console.Write("Stock: ");
                int stock = Convert.ToInt32(Console.ReadLine());
                Console.Write("ID de categoría: ");
                int categoriaId = Convert.ToInt32(Console.ReadLine());

                string insertQuery = "INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, CategoriaId) VALUES (@nombre, @descripcion, @precio, @stock, @categoriaId)";
                SqlCommand cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                cmd.Parameters.AddWithValue("@precio", precio);
                cmd.Parameters.AddWithValue("@stock", stock);
                cmd.Parameters.AddWithValue("@categoriaId", categoriaId);

                cmd.ExecuteNonQuery();
                Console.WriteLine("✅ Producto agregado con exito.");
            }
        }
         
        static void ListarProducto()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT P.Codigo, P.Nombre,P.Descripcion, P.Precio, P.Stock, C.Nombre AS Categoria FROM Productos P JOIN Categorias C ON P.CategoriaId = C.Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\nProductos:");
                while (reader.Read())
                {
                    Console.WriteLine($"[{reader["Codigo"]}] {reader["Nombre"]} -{reader["Descripcion"]} - ${reader["Precio"]} - Stock: {reader["Stock"]} - Categoría: {reader["Categoria"]}");
                }

                reader.Close();
            }
        }

        static void ModificarProducto()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                Console.Write("Código del producto a modificar: ");
                int codigo = Convert.ToInt32(Console.ReadLine());
                Console.Write("Nuevo precio: ");
                decimal nuevoPrecio = Convert.ToDecimal(Console.ReadLine());
                Console.Write("Nueva descripción: ");
                string nuevaDescripcion = Console.ReadLine();

                string updateQuery = "UPDATE Productos SET Precio = @precio, Descripcion = @descripcion WHERE Codigo = @codigo";
                SqlCommand cmd = new SqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@precio", nuevoPrecio);
                cmd.Parameters.AddWithValue("@descripcion", nuevaDescripcion);
                cmd.Parameters.AddWithValue("@codigo", codigo);

                cmd.ExecuteNonQuery();
                Console.WriteLine("🔄 Producto modificado.");
            }
        }
        static void EliminarProducto()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    Console.Write("Código del producto a eliminar: ");
                    int codigo = Convert.ToInt32(Console.ReadLine());

                    string deleteQuery = "DELETE FROM Productos WHERE Codigo = @codigo";
                    SqlCommand cmd = new SqlCommand(deleteQuery, conn);
                    cmd.Parameters.AddWithValue("@codigo", codigo);

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        Console.WriteLine("✅ Producto eliminado correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("⚠️ No se encontró un producto con ese código.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("❌ El código ingresado no es válido. Debe ser un número.");
                }
            }
                

            
        }
    }
    
    

    
}

