using Proyecto.ADO.NET;

namespace Proyecto
{
    public class Programa
    {
        public static void Main(string[] args)
        {
            ProductoHandler productoHandler = new ProductoHandler();

            productoHandler.GetProducto();

            ProductoVendidoHandler productoVendidoHandler = new ProductoVendidoHandler();

            productoVendidoHandler.GetProductosVendidos();

            UsuarioHandler usuarioHandler = new UsuarioHandler();

            usuarioHandler.GetUsuarios();

            VentaHandler ventaHandler = new VentaHandler();

            ventaHandler.GetVenta();
        }
    }
}
