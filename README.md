# Inventario-de-una-tiendita-proyecto-final-ADA-

Aplicación de consola en C# que gestiona el inventario de una tiendita.
Es la implementación del pseudocódigo del proyecto final de ADA.

## Archivos

| Archivo | Contenido |
|---|---|
| `Producto.cs` | Clase `Producto` (id, nombre, categoría, precio, stock) y su formato de tabla. |
| `Inventario.cs` | Arreglo de 100 productos y las operaciones: agregar, buscar, eliminar, alertas y cálculos de reportes. |
| `Program.cs` | Inicio de sesión, menú y lectura validada de datos. |
| `InventarioTiendita.csproj` | Proyecto .NET 8. |

## Cómo ejecutarlo

Requiere el [SDK de .NET 8](https://dotnet.microsoft.com/download) o superior.

```bash
dotnet run
```

## Inicio de sesión

- Usuario: `admin`
- Contraseña: `1234`

Se permiten 3 intentos; al tercer fallo el programa muestra "Acceso bloqueado" y termina.

## Menú

| Opción | Función |
|---|---|
| 1 | Agregar producto (valida que el ID no esté repetido) |
| 2 | Ver productos en forma de tabla |
| 3 | Buscar producto por nombre (coincidencia parcial) |
| 4 | Vender producto: descuenta stock y calcula el total |
| 5 | Agregar stock a un producto existente |
| 6 | Editar producto (enter deja el valor actual) |
| 7 | Eliminar producto (pide confirmación) |
| 8 | Ver alertas: agotados (stock 0) y stock bajo (1 a 5) |
| 9 | Reportes: valor total, más caro, más barato y precio promedio |
| 10 | Salir |

## Notas de implementación

- El inventario usa un arreglo fijo de 100 posiciones y un contador `totalProductos`, igual que el pseudocódigo.
- Las búsquedas son **secuenciales** (búsqueda lineal, O(n)).
- Al eliminar, los elementos posteriores se desplazan una posición a la izquierda para no dejar huecos.
- Toda la entrada numérica se valida con `TryParse`, así que un dato inválido no truena el programa: lo vuelve a pedir.
