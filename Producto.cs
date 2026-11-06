namespace InventarioTiendita;

/// <summary>
/// Representa un producto del inventario de la tiendita.
/// Equivale al STRUCT Producto del pseudocódigo.
/// </summary>
public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Categoria { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }

    public Producto(int id, string nombre, string categoria, decimal precio, int stock)
    {
        Id = id;
        Nombre = nombre;
        Categoria = categoria;
        Precio = precio;
        Stock = stock;
    }

    /// <summary>Devuelve el producto formateado como fila de tabla.</summary>
    public string ComoFila()
    {
        return $"{Id,-6} {Nombre,-22} {Categoria,-16} {Precio,10:C2} {Stock,8}";
    }

    /// <summary>Encabezado con las mismas columnas que <see cref="ComoFila"/>.</summary>
    public static string EncabezadoTabla()
    {
        return "ID".PadRight(6) + " " +
               "NOMBRE".PadRight(22) + " " +
               "CATEGORIA".PadRight(16) + " " +
               "PRECIO".PadLeft(10) + " " +
               "STOCK".PadLeft(8);
    }
}
