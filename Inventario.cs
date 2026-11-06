namespace InventarioTiendita;

/// <summary>
/// Maneja el arreglo de productos y todas las operaciones del inventario.
/// Equivale a "Producto inventario[100]" y "totalProductos" del pseudocódigo.
/// Las búsquedas son secuenciales (búsqueda lineal, O(n)).
/// </summary>
public class Inventario
{
    public const int Capacidad = 100;
    public const int UmbralStockBajo = 5;

    private readonly Producto[] _productos = new Producto[Capacidad];
    private int _totalProductos = 0;

    public int TotalProductos => _totalProductos;
    public bool EstaVacio => _totalProductos == 0;
    public bool EstaLleno => _totalProductos >= Capacidad;

    /// <summary>Agrega un producto al final del arreglo. Devuelve false si ya no hay espacio.</summary>
    public bool Agregar(Producto producto)
    {
        if (EstaLleno)
            return false;

        _productos[_totalProductos] = producto;
        _totalProductos++;
        return true;
    }

    /// <summary>Recorre los productos registrados (solo las posiciones usadas del arreglo).</summary>
    public IEnumerable<Producto> Todos()
    {
        for (int i = 0; i < _totalProductos; i++)
            yield return _productos[i];
    }

    /// <summary>Busca por ID con búsqueda lineal. Devuelve null si no existe.</summary>
    public Producto? BuscarPorId(int id)
    {
        for (int i = 0; i < _totalProductos; i++)
        {
            if (_productos[i].Id == id)
                return _productos[i];
        }
        return null;
    }

    /// <summary>Busca por nombre (coincidencia parcial, sin distinguir mayúsculas).</summary>
    public List<Producto> BuscarPorNombre(string nombre)
    {
        var encontrados = new List<Producto>();
        for (int i = 0; i < _totalProductos; i++)
        {
            if (_productos[i].Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase))
                encontrados.Add(_productos[i]);
        }
        return encontrados;
    }

    /// <summary>Elimina el producto con ese ID desplazando los siguientes una posición a la izquierda.</summary>
    public bool Eliminar(int id)
    {
        for (int i = 0; i < _totalProductos; i++)
        {
            if (_productos[i].Id != id)
                continue;

            for (int j = i; j < _totalProductos - 1; j++)
                _productos[j] = _productos[j + 1];

            _productos[_totalProductos - 1] = null!;
            _totalProductos--;
            return true;
        }
        return false;
    }

    /// <summary>Productos agotados (stock = 0).</summary>
    public List<Producto> Agotados()
    {
        var lista = new List<Producto>();
        foreach (var p in Todos())
        {
            if (p.Stock == 0)
                lista.Add(p);
        }
        return lista;
    }

    /// <summary>Productos con stock bajo (de 1 hasta el umbral).</summary>
    public List<Producto> StockBajo()
    {
        var lista = new List<Producto>();
        foreach (var p in Todos())
        {
            if (p.Stock > 0 && p.Stock <= UmbralStockBajo)
                lista.Add(p);
        }
        return lista;
    }

    /// <summary>Suma de precio * stock de todos los productos.</summary>
    public decimal ValorTotal()
    {
        decimal total = 0;
        foreach (var p in Todos())
            total += p.Precio * p.Stock;
        return total;
    }

    /// <summary>Producto con el precio más alto (null si el inventario está vacío).</summary>
    public Producto? MasCaro()
    {
        Producto? mayor = null;
        foreach (var p in Todos())
        {
            if (mayor == null || p.Precio > mayor.Precio)
                mayor = p;
        }
        return mayor;
    }

    /// <summary>Producto con el precio más bajo (null si el inventario está vacío).</summary>
    public Producto? MasBarato()
    {
        Producto? menor = null;
        foreach (var p in Todos())
        {
            if (menor == null || p.Precio < menor.Precio)
                menor = p;
        }
        return menor;
    }

    /// <summary>Promedio de los precios registrados (0 si no hay productos).</summary>
    public decimal PrecioPromedio()
    {
        if (EstaVacio)
            return 0;

        decimal suma = 0;
        foreach (var p in Todos())
            suma += p.Precio;

        return suma / _totalProductos;
    }

    /// <summary>Total de unidades en existencia.</summary>
    public int UnidadesTotales()
    {
        int unidades = 0;
        foreach (var p in Todos())
            unidades += p.Stock;
        return unidades;
    }
}
