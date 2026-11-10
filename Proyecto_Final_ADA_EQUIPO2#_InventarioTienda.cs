// Arreglo que almacena como máximo 100 productos durante la ejecución.
// Cada posición contiene un producto y totalProductos indica cuántas posiciones se usan.
// Cantidad actual de productos registrados en el arreglo.
Producto[] inventario = new Producto[100];
//Usuarios necesarios para el programa
Usuario[] usuarios = new Usuario[100];
usuarios[0].Username = "admin";
usuarios[0].Password = "1234";
usuarios[0].IsAdmin = true;

usuarios[1].Username = "cliente";
usuarios[1].Password = "1234";
usuarios[1].IsAdmin = false;

int totalProductos = 0;
// Contador de intentos fallidos de inicio de sesión.
int intentos = 0;
// Indica si las credenciales ingresadas son correctas.
bool acceso = false;
// Guarda el nombre del usuario que inició sesión.
string usuario = "";
// Porcentaje de descuento del cliente. El administrador mantiene el valor 0.
decimal descuento = 0;
Console.Write("Que desea hacer?\n1. Registrar nuevo usuario\n2. Ingresar usuario");
// Se permiten hasta tres intentos para ingresar al sistema.
while (intentos < 3 && !acceso)
{
    Console.Write("Usuario: ");
    usuario = (Console.ReadLine() ?? "").Trim();
    Console.Write("Contraseña: ");
    string contraseña = (Console.ReadLine() ?? "").Trim();

    // Primero se rechazan entradas vacías para no procesar datos incompletos.
    if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contraseña))
    {
        intentos++;
        Console.WriteLine("El usuario y la contraseña no pueden estar vacíos.");
    }
    // Solo se permiten los dos usuarios definidos y la contraseña establecida.
    else if ((usuario == "admin" || usuario == "cliente") && contraseña == "1234")
    {
        acceso = true;
    }
    else
    {
        intentos++;
        Console.WriteLine("Usuario o contraseña incorrectos.");
    }
}

// Si se agotaron los intentos, el programa termina sin mostrar ningún menú.
if (!acceso)
{
    Console.WriteLine("Acceso bloqueado.");
    return;
}

// El cliente selecciona un tier que determina el porcentaje de descuento.
if (usuario == "cliente")
{
    int tier;
    do
    {
        Console.WriteLine("1. Tier Básico - 5%");
        Console.WriteLine("2. Tier Plata - 10%");
        Console.WriteLine("3. Tier Oro - 15%");
        tier = LeerEntero("Elige tu tier: ", 1, 3);

        // Se asigna el descuento correspondiente al tier seleccionado.
        if (tier == 1)
        {
            descuento = 0.05m;
        }
        else if (tier == 2)
        {
            descuento = 0.10m;
        }
        else if (tier == 3)
        {
            descuento = 0.15m;
        }
    } while (tier < 1 || tier > 3);
}

// Esta variable controla la opción elegida en cualquiera de los menús.
int opcion;
// El administrador tiene permisos para modificar y consultar todo el inventario.
if (usuario == "admin")
{
    do
    {
        Console.WriteLine();
        Console.WriteLine("MENÚ DE ADMINISTRADOR");
        Console.WriteLine("1. Agregar producto");
        Console.WriteLine("2. Ver productos");
        Console.WriteLine("3. Buscar producto");
        Console.WriteLine("4. Vender producto");
        Console.WriteLine("5. Agregar stock");
        Console.WriteLine("6. Editar producto");
        Console.WriteLine("7. Eliminar producto");
        Console.WriteLine("8. Ver alertas");
        Console.WriteLine("9. Generar reportes");
        Console.WriteLine("10. Salir");
        opcion = LeerEntero("Elige una opción: ", 1, 10);

        // Opción 1: registrar un producto nuevo.
        if (opcion == 1)
        {
            if (totalProductos == inventario.Length)
            {
                Console.WriteLine("El inventario está lleno.");
            }
            else
            {
                Producto producto;
                producto.Id = LeerIdNuevo(inventario, totalProductos);
                producto.Nombre = LeerTexto("Nombre: ");
                producto.Categoria = LeerTexto("Categoría: ");
                producto.Precio = LeerDecimal("Precio: ", 0.01m);
                producto.Stock = LeerEntero("Stock inicial: ", 0, int.MaxValue);
                inventario[totalProductos] = producto;
                totalProductos++;
                Console.WriteLine("Producto agregado.");
            }
        }
        // Opción 2: mostrar todos los productos registrados.
        else if (opcion == 2)
        {
            MostrarProductos(inventario, totalProductos);
        }
        // Opción 3: buscar y mostrar un producto por su ID.
        else if (opcion == 3)
        {
            int id = LeerEntero("Id del producto: ", 1, int.MaxValue);
            int posicion = BuscarProducto(inventario, totalProductos, id);
            if (posicion == -1)
            {
                Console.WriteLine("Producto no encontrado.");
            }
            else
            {
                MostrarProducto(inventario[posicion]);
            }
        }
        // Opción 4: realizar una venta sin permitir stock negativo.
        else if (opcion == 4)
        {
            Vender(inventario, totalProductos, 0);
        }
        // Opción 5: aumentar el stock de un producto existente.
        else if (opcion == 5)
        {
            AgregarStock(inventario, totalProductos);
        }
        // Opción 6: cambiar los datos de un producto existente.
        else if (opcion == 6)
        {
            EditarProducto(inventario, totalProductos);
        }
        // Opción 7: eliminar un producto y compactar el arreglo.
        else if (opcion == 7)
        {
            EliminarProducto(inventario, ref totalProductos);
        }
        // Opción 8: avisar si algún producto está agotado o tiene poco stock.
        else if (opcion == 8)
        {
            VerAlertas(inventario, totalProductos);
        }
        // Opción 9: calcular información general del inventario.
        else if (opcion == 9)
        {
            Reporte(inventario, totalProductos);
        }
    } while (opcion != 10);
}
// El cliente solo puede consultar y comprar; no puede cambiar el inventario.
else
{
    do
    {
        Console.WriteLine();
        Console.WriteLine("MENÚ DE CLIENTE");
        Console.WriteLine("1. Ver inventario");
        Console.WriteLine("2. Comprar producto");
        Console.WriteLine("3. Salir");
        opcion = LeerEntero("Elige una opción: ", 1, 3);

        // Opción 1: consultar los productos disponibles.
        if (opcion == 1)
        {
            MostrarProductos(inventario, totalProductos);
        }
        // Opción 2: comprar un producto aplicando su descuento.
        else if (opcion == 2)
        {
            Vender(inventario, totalProductos, descuento);
        }
    } while (opcion != 3);
}

Console.WriteLine("Programa finalizado.");

// Lee un número entero y repite la solicitud hasta que esté dentro del rango.
// mensaje: texto que se muestra; minimo y maximo: límites permitidos.
// Devuelve el número validado para que el resto del programa pueda usarlo con seguridad.
static int LeerEntero(string mensaje, int minimo, int maximo)
{
    int valor;
    string entrada;
    // El do-while garantiza que el usuario vea la solicitud al menos una vez.
    do
    {
        Console.Write(mensaje);
        entrada = Console.ReadLine() ?? "";
        if (!int.TryParse(entrada, out valor) || valor < minimo || valor > maximo)
        {
            Console.WriteLine("Escribe un número válido entre " + minimo + " y " + maximo + ".");
            valor = minimo - 1;
        }
    } while (valor < minimo || valor > maximo);

    return valor;
}

// Lee un precio decimal y acepta únicamente valores mayores o iguales al mínimo.
// Devuelve el precio validado.
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
            Console.WriteLine("El valor debe ser un número mayor o igual a " + minimo + ".");
            valor = minimo - 1;
        }
    } while (valor < minimo);

    return valor;
}

// Lee nombres y categorías. No permite cadenas vacías o formadas solo por espacios.
// Devuelve el texto recortado de espacios al inicio y al final.
static string LeerTexto(string mensaje)
{
    string texto;
    do
    {
        Console.Write(mensaje);
        texto = (Console.ReadLine() ?? "").Trim();
        if (string.IsNullOrWhiteSpace(texto))
        {
            Console.WriteLine("El texto no puede estar vacío.");
        }
    } while (string.IsNullOrWhiteSpace(texto));

    return texto;
}

// Solicita un ID nuevo y verifica que no esté repetido en el inventario.
// Repite la solicitud hasta recibir un entero positivo que no exista.
static int LeerIdNuevo(Producto[] inventario, int totalProductos)
{
    int id;
    do
    {
        id = LeerEntero("Id: ", 1, int.MaxValue);
        if (BuscarProducto(inventario, totalProductos, id) != -1)
        {
            Console.WriteLine("Ese ID ya existe. Usa otro.");
            id = 0;
        }
    } while (id <= 0);

    return id;
}

// Busca un producto recorriendo únicamente las posiciones ocupadas del arreglo.
// Devuelve la posición encontrada o -1 si el ID no existe.
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

// Muestra en una sola línea todos los datos de un producto.
// Es void porque realiza una acción de salida y no necesita devolver un valor.
static void MostrarProducto(Producto producto)
{
    Console.WriteLine("Id: " + producto.Id + " | Nombre: " + producto.Nombre +
                      " | Categoría: " + producto.Categoria + " | Precio: " +
                      producto.Precio.ToString("C") + " | Stock: " + producto.Stock);
}

// Recorre el inventario y muestra cada producto usando MostrarProducto.
// Si no hay productos, informa al usuario y termina la función.
static void MostrarProductos(Producto[] inventario, int totalProductos)
{
    if (totalProductos == 0)
    {
        Console.WriteLine("No hay productos registrados.");
        return;
    }

    for (int i = 0; i < totalProductos; i++)
    {
        MostrarProducto(inventario[i]);
    }
}

// Realiza una venta:
// 1. Solicita ID y cantidad.
// 2. Busca el producto.
// 3. Comprueba que exista stock suficiente.
// 4. Calcula subtotal, descuento y total.
// 5. Descuenta la cantidad del inventario solo después de validar todo.
static void Vender(Producto[] inventario, int totalProductos, decimal descuento)
{
    int id = LeerEntero("Id del producto: ", 1, int.MaxValue);
    int cantidad = LeerEntero("Cantidad: ", 1, int.MaxValue);
    int posicion = BuscarProducto(inventario, totalProductos, id);

    if (posicion == -1)
    {
        Console.WriteLine("Producto no encontrado.");
        return;
    }

    if (cantidad > inventario[posicion].Stock)
    {
        Console.WriteLine("Stock insuficiente.");
        return;
    }

    decimal subtotal = inventario[posicion].Precio * cantidad;
    decimal descuentoAplicado = subtotal * descuento;
    decimal total = subtotal - descuentoAplicado;
    inventario[posicion].Stock -= cantidad;

    Console.WriteLine("Subtotal: " + subtotal.ToString("C"));
    Console.WriteLine("Descuento: " + descuentoAplicado.ToString("C"));
    Console.WriteLine("Total: " + total.ToString("C"));
}

// Aumenta el stock de un producto. También verifica que la suma no supere
// el valor máximo permitido por un entero.
static void AgregarStock(Producto[] inventario, int totalProductos)
{
    int id = LeerEntero("Id del producto: ", 1, int.MaxValue);
    int cantidad = LeerEntero("Cantidad a agregar: ", 1, int.MaxValue);
    int posicion = BuscarProducto(inventario, totalProductos, id);

    if (posicion == -1)
    {
        Console.WriteLine("Producto no encontrado.");
    }
    else if (cantidad > int.MaxValue - inventario[posicion].Stock)
    {
        Console.WriteLine("La cantidad supera el límite permitido.");
    }
    else
    {
        inventario[posicion].Stock += cantidad;
        Console.WriteLine("Stock actualizado.");
    }
}

// Busca un producto y reemplaza sus datos editables mediante entradas validadas.
static void EditarProducto(Producto[] inventario, int totalProductos)
{
    int id = LeerEntero("Id del producto: ", 1, int.MaxValue);
    int posicion = BuscarProducto(inventario, totalProductos, id);

    if (posicion == -1)
    {
        Console.WriteLine("Producto no encontrado.");
        return;
    }

    inventario[posicion].Nombre = LeerTexto("Nuevo nombre: ");
    inventario[posicion].Categoria = LeerTexto("Nueva categoría: ");
    inventario[posicion].Precio = LeerDecimal("Nuevo precio: ", 0.01m);
    inventario[posicion].Stock = LeerEntero("Nuevo stock: ", 0, int.MaxValue);
    Console.WriteLine("Producto editado.");
}

// Elimina un producto desplazando los siguientes elementos una posición hacia atrás.
// ref permite modificar totalProductos fuera de la función.
static void EliminarProducto(Producto[] inventario, ref int totalProductos)
{
    int id = LeerEntero("Id del producto: ", 1, int.MaxValue);
    int posicion = BuscarProducto(inventario, totalProductos, id);

    if (posicion == -1)
    {
        Console.WriteLine("Producto no encontrado.");
        return;
    }

    for (int i = posicion; i < totalProductos - 1; i++)
    {
        inventario[i] = inventario[i + 1];
    }

    // Se reduce el contador y se limpia la última posición para no dejar datos antiguos.
    totalProductos--;
    inventario[totalProductos] = new Producto();
    Console.WriteLine("Producto eliminado.");
}

// Revisa el stock de cada producto y muestra alertas de agotamiento o stock bajo.
static void VerAlertas(Producto[] inventario, int totalProductos)
{
    bool hayAlertas = false;
    for (int i = 0; i < totalProductos; i++)
    {
        if (inventario[i].Stock == 0)
        {
            Console.WriteLine(inventario[i].Nombre + ": agotado");
            hayAlertas = true;
        }
        else if (inventario[i].Stock <= 5)
        {
            Console.WriteLine(inventario[i].Nombre + ": stock bajo");
            hayAlertas = true;
        }
    }

    if (!hayAlertas)
    {
        Console.WriteLine("No hay alertas.");
    }
}

// Calcula el valor total del inventario, el promedio de precios
// y los productos con precio máximo y mínimo.
static void Reporte(Producto[] inventario, int totalProductos)
{
    if (totalProductos == 0)
    {
        Console.WriteLine("No hay productos registrados.");
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

    Console.WriteLine("Valor total del inventario: " + valorTotal.ToString("C"));
    Console.WriteLine("Producto más caro: " + masCaro.Nombre);
    Console.WriteLine("Producto más barato: " + masBarato.Nombre);
    Console.WriteLine("Precio promedio: " + (sumaPrecios / totalProductos).ToString("C"));
}

// Struct que agrupa todos los datos relacionados con un producto.
// Se utiliza como tipo de dato para guardar elementos en el arreglo inventario.
struct Producto
{
    public int Id;
    public string Nombre;
    public string Categoria;
    public decimal Precio;
    public int Stock;
}

struct Usuario
{
    public string Username;
    public string Password;
    public bool IsAdmin;
}

