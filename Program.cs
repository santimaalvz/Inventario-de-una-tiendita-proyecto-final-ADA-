using System.Globalization;

namespace InventarioTiendita;

/// <summary>
/// Programa principal: inicio de sesión y menú del inventario de la tiendita.
/// Proyecto final de ADA.
/// </summary>
public static class Program
{
    private const string UsuarioValido = "admin";
    private const string ContrasenaValida = "1234";
    private const int MaxIntentos = 3;

    private static readonly Inventario Tienda = new();

    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        if (!IniciarSesion())
        {
            Console.WriteLine("\nAcceso bloqueado.");
            return;
        }

        MostrarMenu();
        Console.WriteLine("\nPrograma finalizado.");
    }

    // ---------------------------------------------------------------- LOGIN

    /// <summary>Pide usuario y contraseña hasta 3 veces. Devuelve true si el acceso fue concedido.</summary>
    private static bool IniciarSesion()
    {
        int intentos = 0;
        bool acceso = false;

        Console.WriteLine("=============================================");
        Console.WriteLine("   INVENTARIO DE LA TIENDITA - INICIO SESION  ");
        Console.WriteLine("=============================================");

        while (intentos < MaxIntentos && !acceso)
        {
            Console.Write("Usuario: ");
            string usuario = (Console.ReadLine() ?? string.Empty).Trim();

            Console.Write("Contraseña: ");
            string contrasena = LeerContrasena();

            if (usuario == UsuarioValido && contrasena == ContrasenaValida)
            {
                acceso = true;
                Console.WriteLine($"\nBienvenido, {usuario}.");
            }
            else
            {
                intentos++;
                int restantes = MaxIntentos - intentos;
                Console.WriteLine($"Usuario o contraseña incorrectos. Intentos restantes: {restantes}");
            }
        }

        return acceso;
    }

    /// <summary>Lee la contraseña ocultando los caracteres con asteriscos.</summary>
    private static string LeerContrasena()
    {
        // Si la entrada viene de un archivo o de una tubería no hay teclado que leer,
        // así que se lee la línea completa de forma normal.
        if (Console.IsInputRedirected)
        {
            Console.WriteLine();
            return Console.ReadLine() ?? string.Empty;
        }

        var contrasena = new System.Text.StringBuilder();

        while (true)
        {
            ConsoleKeyInfo tecla = Console.ReadKey(intercept: true);

            if (tecla.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return contrasena.ToString();
            }

            if (tecla.Key == ConsoleKey.Backspace)
            {
                if (contrasena.Length > 0)
                {
                    contrasena.Length--;
                    Console.Write("\b \b");
                }
                continue;
            }

            if (!char.IsControl(tecla.KeyChar))
            {
                contrasena.Append(tecla.KeyChar);
                Console.Write("*");
            }
        }
    }

    // ----------------------------------------------------------------- MENU

    private static void MostrarMenu()
    {
        int opcion;

        do
        {
            Console.WriteLine("\n================ MENU PRINCIPAL ================");
            Console.WriteLine(" 1. Agregar producto");
            Console.WriteLine(" 2. Ver productos");
            Console.WriteLine(" 3. Buscar producto");
            Console.WriteLine(" 4. Vender producto");
            Console.WriteLine(" 5. Agregar stock");
            Console.WriteLine(" 6. Editar producto");
            Console.WriteLine(" 7. Eliminar producto");
            Console.WriteLine(" 8. Ver alertas");
            Console.WriteLine(" 9. Generar reportes");
            Console.WriteLine("10. Salir");
            Console.WriteLine("================================================");

            opcion = LeerEntero("Elige una opción: ", 1, 10);

            switch (opcion)
            {
                case 1: AgregarProducto(); break;
                case 2: VerProductos(); break;
                case 3: BuscarProducto(); break;
                case 4: VenderProducto(); break;
                case 5: AgregarStock(); break;
                case 6: EditarProducto(); break;
                case 7: EliminarProducto(); break;
                case 8: VerAlertas(); break;
                case 9: GenerarReportes(); break;
                case 10: Console.WriteLine("\nSaliendo del sistema..."); break;
            }

        } while (opcion != 10);
    }

    // ------------------------------------------------------------- OPCION 1

    private static void AgregarProducto()
    {
        Console.WriteLine("\n--- AGREGAR PRODUCTO ---");

        if (Tienda.EstaLleno)
        {
            Console.WriteLine($"El inventario está lleno ({Inventario.Capacidad} productos). No se puede agregar más.");
            return;
        }

        int id = LeerEntero("ID del producto: ", 1, int.MaxValue);

        if (Tienda.BuscarPorId(id) != null)
        {
            Console.WriteLine("Ya existe un producto con ese ID.");
            return;
        }

        string nombre = LeerTexto("Nombre: ");
        string categoria = LeerTexto("Categoría: ");
        decimal precio = LeerDecimal("Precio en pesos: ", 0);

        Console.WriteLine("¿Cuántas unidades deseas añadir?");
        int unidades = LeerEntero("Unidades: ", 0, int.MaxValue);

        Tienda.Agregar(new Producto(id, nombre, categoria, precio, unidades));
        Console.WriteLine("Producto agregado.");
    }

    // ------------------------------------------------------------- OPCION 2

    private static void VerProductos()
    {
        Console.WriteLine("\n--- LISTA DE PRODUCTOS ---");

        if (Tienda.EstaVacio)
        {
            Console.WriteLine("No hay productos registrados.");
            return;
        }

        Console.WriteLine(Producto.EncabezadoTabla());
        Console.WriteLine(new string('-', Producto.EncabezadoTabla().Length));

        foreach (Producto producto in Tienda.Todos())
            Console.WriteLine(producto.ComoFila());

        Console.WriteLine(new string('-', Producto.EncabezadoTabla().Length));
        Console.WriteLine($"Total de productos: {Tienda.TotalProductos}");
    }

    // ------------------------------------------------------------- OPCION 3

    private static void BuscarProducto()
    {
        Console.WriteLine("\n--- BUSCAR PRODUCTO ---");

        if (Tienda.EstaVacio)
        {
            Console.WriteLine("No hay productos registrados.");
            return;
        }

        string nombreBuscado = LeerTexto("Nombre a buscar: ");
        List<Producto> encontrados = Tienda.BuscarPorNombre(nombreBuscado);

        if (encontrados.Count == 0)
        {
            Console.WriteLine("Producto no encontrado.");
            return;
        }

        Console.WriteLine(Producto.EncabezadoTabla());
        Console.WriteLine(new string('-', Producto.EncabezadoTabla().Length));

        foreach (Producto producto in encontrados)
            Console.WriteLine(producto.ComoFila());
    }

    // ------------------------------------------------------------- OPCION 4

    private static void VenderProducto()
    {
        Console.WriteLine("\n--- VENDER PRODUCTO ---");

        if (Tienda.EstaVacio)
        {
            Console.WriteLine("No hay productos registrados.");
            return;
        }

        int idBuscado = LeerEntero("ID del producto: ", 1, int.MaxValue);
        int cantidad = LeerEntero("Cantidad a vender: ", 1, int.MaxValue);

        Producto? producto = Tienda.BuscarPorId(idBuscado);

        if (producto != null && cantidad <= producto.Stock)
        {
            producto.Stock -= cantidad;
            decimal total = producto.Precio * cantidad;

            Console.WriteLine($"Venta realizada: {cantidad} x {producto.Nombre}");
            Console.WriteLine($"Total a pagar : {Producto.Moneda(total)}");
            Console.WriteLine($"Stock restante: {producto.Stock}");
        }
        else
        {
            Console.WriteLine("Producto no encontrado o stock insuficiente.");
        }
    }

    // ------------------------------------------------------------- OPCION 5

    private static void AgregarStock()
    {
        Console.WriteLine("\n--- AGREGAR STOCK ---");

        if (Tienda.EstaVacio)
        {
            Console.WriteLine("No hay productos registrados.");
            return;
        }

        int idBuscado = LeerEntero("ID del producto: ", 1, int.MaxValue);
        int cantidad = LeerEntero("Unidades a agregar: ", 1, int.MaxValue);

        Producto? producto = Tienda.BuscarPorId(idBuscado);

        if (producto != null)
        {
            producto.Stock += cantidad;
            Console.WriteLine($"Stock actualizado. {producto.Nombre} ahora tiene {producto.Stock} unidades.");
        }
        else
        {
            Console.WriteLine("Producto no encontrado.");
        }
    }

    // ------------------------------------------------------------- OPCION 6

    private static void EditarProducto()
    {
        Console.WriteLine("\n--- EDITAR PRODUCTO ---");

        if (Tienda.EstaVacio)
        {
            Console.WriteLine("No hay productos registrados.");
            return;
        }

        int idBuscado = LeerEntero("ID del producto: ", 1, int.MaxValue);
        Producto? producto = Tienda.BuscarPorId(idBuscado);

        if (producto == null)
        {
            Console.WriteLine("Producto no encontrado.");
            return;
        }

        Console.WriteLine("Datos actuales:");
        Console.WriteLine(Producto.EncabezadoTabla());
        Console.WriteLine(producto.ComoFila());
        Console.WriteLine("\n(Deja el campo vacío para conservar el valor actual)");

        Console.Write($"Nuevo nombre [{producto.Nombre}]: ");
        string nombre = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(nombre))
            producto.Nombre = nombre.Trim();

        Console.Write($"Nueva categoría [{producto.Categoria}]: ");
        string categoria = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(categoria))
            producto.Categoria = categoria.Trim();

        Console.Write($"Nuevo precio en pesos [{Producto.Cantidad(producto.Precio)}]: ");
        string textoPrecio = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(textoPrecio))
        {
            if (decimal.TryParse(textoPrecio, NumberStyles.Currency, Producto.CulturaMexico, out decimal precio) && precio >= 0)
                producto.Precio = precio;
            else
                Console.WriteLine("Precio inválido, se conservó el valor anterior.");
        }

        Console.Write($"Nuevo stock [{producto.Stock}]: ");
        string textoStock = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(textoStock))
        {
            if (int.TryParse(textoStock, out int stock) && stock >= 0)
                producto.Stock = stock;
            else
                Console.WriteLine("Stock inválido, se conservó el valor anterior.");
        }

        Console.WriteLine("Producto editado.");
    }

    // ------------------------------------------------------------- OPCION 7

    private static void EliminarProducto()
    {
        Console.WriteLine("\n--- ELIMINAR PRODUCTO ---");

        if (Tienda.EstaVacio)
        {
            Console.WriteLine("No hay productos registrados.");
            return;
        }

        int idBuscado = LeerEntero("ID del producto: ", 1, int.MaxValue);
        Producto? producto = Tienda.BuscarPorId(idBuscado);

        if (producto == null)
        {
            Console.WriteLine("Producto no encontrado.");
            return;
        }

        Console.Write($"¿Seguro que deseas eliminar \"{producto.Nombre}\"? (s/n): ");
        string respuesta = (Console.ReadLine() ?? string.Empty).Trim().ToLower();

        if (respuesta != "s" && respuesta != "si" && respuesta != "sí")
        {
            Console.WriteLine("Operación cancelada.");
            return;
        }

        Tienda.Eliminar(idBuscado);
        Console.WriteLine("Producto eliminado.");
    }

    // ------------------------------------------------------------- OPCION 8

    private static void VerAlertas()
    {
        Console.WriteLine("\n--- ALERTAS DE INVENTARIO ---");

        if (Tienda.EstaVacio)
        {
            Console.WriteLine("No hay productos registrados.");
            return;
        }

        List<Producto> agotados = Tienda.Agotados();
        List<Producto> stockBajo = Tienda.StockBajo();

        if (agotados.Count == 0 && stockBajo.Count == 0)
        {
            Console.WriteLine("Sin alertas: todos los productos tienen stock suficiente.");
            return;
        }

        foreach (Producto producto in agotados)
            Console.WriteLine($"[AGOTADO]    {producto.Nombre} - agotado (0 unidades)");

        foreach (Producto producto in stockBajo)
            Console.WriteLine($"[STOCK BAJO] {producto.Nombre} - quedan {producto.Stock} unidades");
    }

    // ------------------------------------------------------------- OPCION 9

    private static void GenerarReportes()
    {
        Console.WriteLine("\n--- REPORTES ---");

        if (Tienda.EstaVacio)
        {
            Console.WriteLine("No hay productos registrados.");
            return;
        }

        decimal valorTotal = Tienda.ValorTotal();
        Producto masCaro = Tienda.MasCaro()!;
        Producto masBarato = Tienda.MasBarato()!;
        decimal promedio = Tienda.PrecioPromedio();

        Console.WriteLine($"Productos distintos    : {Tienda.TotalProductos}");
        Console.WriteLine($"Unidades en existencia : {Tienda.UnidadesTotales()}");
        Console.WriteLine($"Valor total inventario : {Producto.Moneda(valorTotal)}");
        Console.WriteLine($"Producto más caro      : {masCaro.Nombre} ({Producto.Moneda(masCaro.Precio)})");
        Console.WriteLine($"Producto más barato    : {masBarato.Nombre} ({Producto.Moneda(masBarato.Precio)})");
        Console.WriteLine($"Precio promedio        : {Producto.Moneda(promedio)}");
    }

    // -------------------------------------------------------- LECTURA SEGURA

    /// <summary>Lee un entero dentro del rango indicado, repitiendo hasta que el dato sea válido.</summary>
    private static int LeerEntero(string mensaje, int minimo, int maximo)
    {
        while (true)
        {
            Console.Write(mensaje);
            string entrada = Console.ReadLine() ?? string.Empty;

            if (int.TryParse(entrada, out int valor) && valor >= minimo && valor <= maximo)
                return valor;

            Console.WriteLine($"Dato inválido. Escribe un número entero entre {minimo} y {maximo}.");
        }
    }

    /// <summary>Lee un decimal mayor o igual al mínimo indicado.</summary>
    private static decimal LeerDecimal(string mensaje, decimal minimo)
    {
        while (true)
        {
            Console.Write(mensaje);
            string entrada = Console.ReadLine() ?? string.Empty;

            // NumberStyles.Currency acepta también que el usuario escriba el signo de pesos.
            if (decimal.TryParse(entrada, NumberStyles.Currency, Producto.CulturaMexico, out decimal valor) && valor >= minimo)
                return valor;

            Console.WriteLine($"Dato inválido. Escribe una cantidad en pesos mayor o igual a {Producto.Cantidad(minimo)}.");
        }
    }

    /// <summary>Lee una cadena que no puede quedar vacía.</summary>
    private static string LeerTexto(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string entrada = (Console.ReadLine() ?? string.Empty).Trim();

            if (entrada.Length > 0)
                return entrada;

            Console.WriteLine("Este campo no puede quedar vacío.");
        }
    }
}
