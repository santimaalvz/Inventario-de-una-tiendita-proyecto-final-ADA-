// Arreglo que almacena como máximo 100 productos durante la ejecución.
// Solo se recorren las posiciones de 0 a totalProductos - 1.

Producto[] inventario = new Producto[100];
// Cantidad actual de productos registrados en el arreglo.
int totalProductos = 0;

// Arreglo de usuarios registrados durante la ejecución.
Usuario[] usuarios = new Usuario[100];
int totalUsuarios = 2;
usuarios[0] = new Usuario
{
    Username = "admin",
    Password = "1234",
    IsAdmin = true
};
usuarios[1] = new Usuario
{
    Username = "cliente",
    Password = "1234",
    IsAdmin = false
};

int intentos = 0;
bool acceso = false;
string usuario = "";
bool esAdministrador = false;
// El descuento se conserva como decimal para evitar errores de redondeo.
decimal descuento = 0;

while (!acceso)
{
    MostrarEncabezado("SISTEMA DE INVENTARIO");
    Console.WriteLine("  1. Registrar nuevo usuario");
    Console.WriteLine("  2. Ingresar al sistema");
    Console.WriteLine();
    int opcionInicio = LeerEntero("  Seleccione una opción: ", 1, 2);

    if (opcionInicio == 1)
    {
        CrearUsuario(usuarios, ref totalUsuarios);
    }
    else
    {
        MostrarEncabezado("INICIO DE SESIÓN");
        Console.WriteLine("Ingrese sus credenciales para continuar.");
        Console.WriteLine();

        // Se permiten hasta tres intentos para ingresar al sistema.
        while (intentos < 3 && !acceso)
        {
            Console.Write("  Usuario: ");
            usuario = (Console.ReadLine() ?? "").Trim();
            Console.Write("  Contraseña: ");
            string contraseña = (Console.ReadLine() ?? "").Trim();
            int posicionUsuario = BuscarUsuario(usuarios, totalUsuarios, usuario, contraseña);

            if (posicionUsuario != -1)
            {
                acceso = true;
                esAdministrador = usuarios[posicionUsuario].IsAdmin;
            }
            else
            {
                intentos++;
                Console.WriteLine("[AVISO] Usuario o contraseña incorrectos " +
                                  $"(intento {intentos} de 3).");
            }
        }

        if (!acceso)
        {
            Console.WriteLine("\nAcceso bloqueado. Se agotaron los intentos permitidos.");
            return;
        }
    }
}

if (esAdministrador)
{
    Console.WriteLine("\nBienvenido, administrador.");
}
else
{
    Console.WriteLine($"\nBienvenido, {usuario}.");
}

// El cliente selecciona un tier que determina el porcentaje de descuento.
if (!esAdministrador)
{
    MostrarEncabezado("SELECCIÓN DE TIER");
    Console.WriteLine("  1. Tier Básico - 5%");
    Console.WriteLine("  2. Tier Plata  - 10%");
    Console.WriteLine("  3. Tier Oro    - 15%");
    Console.WriteLine();
    int tier = LeerEntero("  Elige tu tier: ", 1, 3);

    if (tier == 1)
    {
        descuento = Convert.ToDecimal(0.05);
    }
    else if (tier == 2)
    {
        descuento = Convert.ToDecimal(0.10);
    }
    else
    {
        descuento = Convert.ToDecimal(0.15);
    }
}

int opcion;
// El administrador puede consultar y modificar todo el inventario.
if (esAdministrador)
{
    do
    {
        MostrarEncabezado("MENÚ DE ADMINISTRADOR");
        Console.WriteLine("  1. Agregar producto");
        Console.WriteLine("  2. Ver productos");
        Console.WriteLine("  3. Buscar producto");
        Console.WriteLine("  4. Vender producto");
        Console.WriteLine("  5. Agregar stock");
        Console.WriteLine("  6. Editar producto");
        Console.WriteLine("  7. Eliminar producto");
        Console.WriteLine("  8. Ver alertas");
        Console.WriteLine("  9. Generar reportes");
        Console.WriteLine("  10. Salir");
        Console.WriteLine();
        opcion = LeerEntero("  Seleccione una opción: ", 1, 10);

        if (opcion == 1)
        {
            if (totalProductos == inventario.Length)
            {
                Console.WriteLine("\n[AVISO] El inventario está lleno.");
            }
            else
            {
                Producto producto;
                producto.Id = LeerIdNuevo(inventario, totalProductos);
                producto.Nombre = LeerTexto("  Nombre: ");
                producto.Categoria = LeerTexto("  Categoría: ");
                producto.Precio = LeerDecimal("  Precio: ", 0.01m);
                producto.Stock = LeerEntero("  Stock inicial: ", 0, int.MaxValue);
                inventario[totalProductos] = producto;
                totalProductos++;
                Console.WriteLine("\n[OK] Producto agregado correctamente.");
            }
        }
        else if (opcion == 2)
        {
            MostrarProductos(inventario, totalProductos);
        }
        else if (opcion == 3)
        {
            int id = LeerEntero("  Id del producto: ", 1, int.MaxValue);
            int posicion = BuscarProducto(inventario, totalProductos, id);
            if (posicion == -1)
            {
                Console.WriteLine("\n[AVISO] Producto no encontrado.");
            }
            else
            {
                MostrarProducto(inventario[posicion]);
            }
        }
        else if (opcion == 4)
        {
            Vender(inventario, totalProductos, 0);
        }
        else if (opcion == 5)
        {
            AgregarStock(inventario, totalProductos);
        }
        else if (opcion == 6)
        {
            EditarProducto(inventario, totalProductos);
        }
        else if (opcion == 7)
        {
            EliminarProducto(inventario, ref totalProductos);
        }
        else if (opcion == 8)
        {
            VerAlertas(inventario, totalProductos);
        }
        else if (opcion == 9)
        {
            Reporte(inventario, totalProductos);
        }
    } while (opcion != 10);
}
// El cliente solo puede consultar y comprar.
else
{
    do
    {
        MostrarEncabezado("MENÚ DE CLIENTE");
        Console.WriteLine("  1. Ver inventario");
        Console.WriteLine("  2. Comprar producto");
        Console.WriteLine("  3. Salir");
        Console.WriteLine();
        opcion = LeerEntero("  Seleccione una opción: ", 1, 3);

        if (opcion == 1)
        {
            MostrarProductos(inventario, totalProductos);
        }
        else if (opcion == 2)
        {
            Vender(inventario, totalProductos, descuento);
        }
    } while (opcion != 3);
}

Console.WriteLine("\nPrograma finalizado. Gracias por utilizar el sistema.");

// Muestra un encabezado uniforme para separar las secciones de la consola.
static void MostrarEncabezado(string titulo)
{
    Console.WriteLine();
    Console.WriteLine("==================================================");
    Console.WriteLine($"                 {titulo}");
    Console.WriteLine("==================================================");
}

// Lee un entero válido dentro del rango indicado.
static int LeerEntero(string mensaje, int minimo, int maximo)
{
    int valor;
    string entrada;
    do
    {
        Console.Write(mensaje);
        entrada = Console.ReadLine() ?? "";
        if (!int.TryParse(entrada, out valor) || valor < minimo || valor > maximo)
        {
            Console.WriteLine("[AVISO] Escribe un número válido entre " + minimo + " y " + maximo + ".");
            valor = minimo - 1;
        }
    } while (valor < minimo || valor > maximo);

    return valor;
}

// Lee un precio decimal mayor o igual al mínimo indicado.
static decimal LeerDecimal(string mensaje, decimal minimo)
{
    decimal valor;
    string entrada;
    do
    {
        Console.Write(mensaje);
        entrada = Console.ReadLine() ?? "";
        if (!decimal.TryParse(entrada, out valor) || valor < minimo)
        {
            Console.WriteLine("[AVISO] El valor debe ser un número mayor o igual a " + minimo + ".");
            valor = minimo - 1;
        }
    } while (valor < minimo);

    return valor;
}

// Lee texto no vacío y elimina espacios al inicio y al final.
static string LeerTexto(string mensaje)
{
    string texto;
    do
    {
        Console.Write(mensaje);
        texto = (Console.ReadLine() ?? "").Trim();
        if (string.IsNullOrWhiteSpace(texto))
        {
            Console.WriteLine("[AVISO] El texto no puede estar vacío.");
        }
    } while (string.IsNullOrWhiteSpace(texto));

    return texto;
}

// Solicita un ID positivo que no esté repetido.
static int LeerIdNuevo(Producto[] inventario, int totalProductos)
{
    int id;
    do
    {
        id = LeerEntero("Id: ", 1, int.MaxValue);
        if (BuscarProducto(inventario, totalProductos, id) != -1)
        {
            Console.WriteLine("[AVISO] Ese ID ya existe. Usa otro.");
            id = 0;
        }
    } while (id <= 0);

    return id;
}

// Busca un usuario comparando el nombre y la contraseña.
// Devuelve su posición o -1 si las credenciales no coinciden.
static int BuscarUsuario(Usuario[] usuarios, int totalUsuarios,
                         string username, string password)
{
    for (int i = 0; i < totalUsuarios; i++)
    {
        if (usuarios[i].Username == username && usuarios[i].Password == password)
        {
            return i;
        }
    }

    return -1;
}

// Registra un usuario nuevo y lo agrega a la siguiente posición disponible.
// Los usuarios registrados comienzan con permisos de cliente.
static void CrearUsuario(Usuario[] usuarios, ref int totalUsuarios)
{
    MostrarEncabezado("REGISTRO DE USUARIO");

    if (totalUsuarios == usuarios.Length)
    {
        Console.WriteLine("[AVISO] No hay espacio para registrar más usuarios.");
        return;
    }

    string username = LeerTexto("  Nuevo usuario: ");
    if (BuscarUsuarioPorNombre(usuarios, totalUsuarios, username) != -1)
    {
        Console.WriteLine("[AVISO] Ese nombre de usuario ya está registrado.");
        return;
    }

    string password = LeerTexto("  Nueva contraseña: ");
    usuarios[totalUsuarios] = new Usuario
    {
        Username = username,
        Password = password,
        IsAdmin = false
    };
    totalUsuarios++;
    Console.WriteLine("\n[OK] Usuario registrado correctamente.");
}

// Busca un nombre de usuario sin revisar la contraseña.
static int BuscarUsuarioPorNombre(Usuario[] usuarios, int totalUsuarios, string username)
{
    for (int i = 0; i < totalUsuarios; i++)
    {
        if (usuarios[i].Username == username)
        {
            return i;
        }
    }

    return -1;
}

// Busca secuencialmente un producto y devuelve su posición o -1.
static int BuscarProducto(Producto[] inventario, int totalProductos, int id)
{
    for (int i = 0; i < totalProductos; i++)
    {
        if (inventario[i].Id == id)
        {
            return i;
        }
    }

    return -1;
}

// Muestra los datos de un producto en una sola línea.
static void MostrarProducto(Producto producto)
{
    Console.WriteLine($"  Id: {producto.Id} | Nombre: {producto.Nombre} | " +
                      $"Categoría: {producto.Categoria} | Precio: {producto.Precio:C} | " +
                      $"Stock: {producto.Stock}");
}

// Muestra todos los productos registrados.
static void MostrarProductos(Producto[] inventario, int totalProductos)
{
    MostrarEncabezado("INVENTARIO DE PRODUCTOS");

    if (totalProductos == 0)
    {
        Console.WriteLine("[AVISO] No hay productos registrados.");
        return;
    }

    for (int i = 0; i < totalProductos; i++)
    {
        MostrarProducto(inventario[i]);
    }
}

// Valida una venta, calcula su total y actualiza el stock.
static void Vender(Producto[] inventario, int totalProductos, decimal descuento)
{
    int id = LeerEntero("  Id del producto: ", 1, int.MaxValue);
    int cantidad = LeerEntero("  Cantidad: ", 1, int.MaxValue);
    int posicion = BuscarProducto(inventario, totalProductos, id);

    if (posicion == -1)
    {
        Console.WriteLine("\n[AVISO] Producto no encontrado.");
        return;
    }

    if (cantidad > inventario[posicion].Stock)
    {
        Console.WriteLine("\n[AVISO] Stock insuficiente.");
        return;
    }

    decimal subtotal = inventario[posicion].Precio * cantidad;
    decimal descuentoAplicado = subtotal * descuento;
    decimal total = subtotal - descuentoAplicado;
    inventario[posicion].Stock -= cantidad;

    Console.WriteLine("\n------------------ RESUMEN DE COMPRA ------------------");
    Console.WriteLine("  Subtotal:  " + subtotal.ToString("C"));
    Console.WriteLine("  Descuento: " + descuentoAplicado.ToString("C"));
    Console.WriteLine("  Total:     " + total.ToString("C"));
    Console.WriteLine("--------------------------------------------------------");
}

// Aumenta el stock sin superar el límite de int.
static void AgregarStock(Producto[] inventario, int totalProductos)
{
    int id = LeerEntero("  Id del producto: ", 1, int.MaxValue);
    int cantidad = LeerEntero("  Cantidad a agregar: ", 1, int.MaxValue);
    int posicion = BuscarProducto(inventario, totalProductos, id);

    if (posicion == -1)
    {
        Console.WriteLine("\n[AVISO] Producto no encontrado.");
    }
    else if (cantidad > int.MaxValue - inventario[posicion].Stock)
    {
        Console.WriteLine("\n[AVISO] La cantidad supera el límite permitido.");
    }
    else
    {
        inventario[posicion].Stock += cantidad;
        Console.WriteLine("\n[OK] Stock actualizado.");
    }
}

// Reemplaza los datos editables de un producto existente.
static void EditarProducto(Producto[] inventario, int totalProductos)
{
    int id = LeerEntero("  Id del producto: ", 1, int.MaxValue);
    int posicion = BuscarProducto(inventario, totalProductos, id);

    if (posicion == -1)
    {
        Console.WriteLine("\n[AVISO] Producto no encontrado.");
        return;
    }

    inventario[posicion].Nombre = LeerTexto("  Nuevo nombre: ");
    inventario[posicion].Categoria = LeerTexto("  Nueva categoría: ");
    inventario[posicion].Precio = LeerDecimal("  Nuevo precio: ", 0.01m);
    inventario[posicion].Stock = LeerEntero("  Nuevo stock: ", 0, int.MaxValue);
    Console.WriteLine("\n[OK] Producto editado correctamente.");
}

// Elimina un producto y compacta el arreglo.
// ref actualiza el total de productos en el programa principal.
static void EliminarProducto(Producto[] inventario, ref int totalProductos)
{
    int id = LeerEntero("  Id del producto: ", 1, int.MaxValue);
    int posicion = BuscarProducto(inventario, totalProductos, id);

    if (posicion == -1)
    {
        Console.WriteLine("\n[AVISO] Producto no encontrado.");
        return;
    }

    for (int i = posicion; i < totalProductos - 1; i++)
    {
        inventario[i] = inventario[i + 1];
    }

    // Se limpia la posición que quedó libre.
    totalProductos--;
    inventario[totalProductos] = new Producto();
    Console.WriteLine("\n[OK] Producto eliminado correctamente.");
}

// Muestra alertas de agotamiento o stock bajo.
static void VerAlertas(Producto[] inventario, int totalProductos)
{
    MostrarEncabezado("ALERTAS DE STOCK");
    bool hayAlertas = false;
    for (int i = 0; i < totalProductos; i++)
    {
        if (inventario[i].Stock == 0)
        {
            Console.WriteLine($"  [AGOTADO] {inventario[i].Nombre}");
            hayAlertas = true;
        }
        else if (inventario[i].Stock <= 5)
        {
            Console.WriteLine($"  [STOCK BAJO] {inventario[i].Nombre}");
            hayAlertas = true;
        }
    }

    if (!hayAlertas)
    {
        Console.WriteLine("  No hay alertas.");
    }
}

// Calcula el valor total, precios extremos y promedio.
static void Reporte(Producto[] inventario, int totalProductos)
{
    MostrarEncabezado("REPORTE DEL INVENTARIO");

    if (totalProductos == 0)
    {
        Console.WriteLine("[AVISO] No hay productos registrados.");
        return;
    }

    decimal valorTotal = 0;
    decimal sumaPrecios = 0;
    Producto masCaro = inventario[0];
    Producto masBarato = inventario[0];

    for (int i = 0; i < totalProductos; i++)
    {
        valorTotal += inventario[i].Precio * inventario[i].Stock;
        sumaPrecios += inventario[i].Precio;
        if (inventario[i].Precio > masCaro.Precio)
        {
            masCaro = inventario[i];
        }

        if (inventario[i].Precio < masBarato.Precio)
        {
            masBarato = inventario[i];
        }
    }

    Console.WriteLine($"  Valor total del inventario: {valorTotal:C}");
    Console.WriteLine($"  Producto más caro:          {masCaro.Nombre}");
    Console.WriteLine($"  Producto más barato:        {masBarato.Nombre}");
    Console.WriteLine($"  Precio promedio:            {sumaPrecios / totalProductos:C}");
}

// Agrupa los datos necesarios para representar un producto.
struct Producto
{
    public int Id;
    public string Nombre;
    public string Categoria;
    public decimal Precio;
    public int Stock;
}

// Agrupa las credenciales y el tipo de acceso de cada usuario.
struct Usuario
{
    public string Username;
    public string Password;
    public bool IsAdmin;
}