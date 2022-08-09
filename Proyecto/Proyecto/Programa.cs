using Proyecto.ADO.NET;

namespace Proyecto
{
    public class Programa
    {
        public static void Main(string[] args)
        {
            ProductoHandler productoHandler = new ProductoHandler();

            productoHandler.GetProducto();


            UsuarioHandler usuarioHandler = new UsuarioHandler();

            usuarioHandler.GetUsuarios();
        }
    }
}
