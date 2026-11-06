using System.Globalization;

namespace InventarioTiendita;

/// <summary>
/// Representa un producto del inventario de la tiendita.
/// Equivale al STRUCT Producto del pseudocódigo.
/// </summary>
public class Producto
{
    /// <summary>
    /// Cultura de México. Se fija aquí para que los precios se muestren y se lean
    /// siempre en pesos mexicanos, sin importar la configuración regional de la
    /// computadora donde se ejecute el programa.
    /// </summary>
    public static readonly CultureInfo CulturaMexico = CultureInfo.GetCultureInfo("es-MX");

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

    /// <summary>Formatea una cantidad como pesos mexicanos. Ejemplo: $1,234.50 MXN</summary>
    public static string Moneda(decimal monto)
    {
        return monto.ToString("C2", CulturaMexico) + " MXN";
    }

    /// <summary>Formatea una cantidad sin el símbolo, para los valores actuales al editar.</summary>
    public static string Cantidad(decimal monto)
    {
        return monto.ToString("N2", CulturaMexico);
    }

    /// <summary>Devuelve el producto formateado como fila de tabla.</summary>
    public string ComoFila()
    {
        return $"{Id,-6} {Nombre,-22} {Categoria,-16} {Moneda(Precio),16} {Stock,8}";
    }

    /// <summary>Encabezado con las mismas columnas que <see cref="ComoFila"/>.</summary>
    public static string EncabezadoTabla()
    {
        return "ID".PadRight(6) + " " +
               "NOMBRE".PadRight(22) + " " +
               "CATEGORIA".PadRight(16) + " " +
               "PRECIO".PadLeft(16) + " " +
               "STOCK".PadLeft(8);
    }
}
