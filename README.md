<div align="center">

# 🏪 Inventario de una Tiendita

**Sistema de inventario en consola con roles de administrador y cliente**

Proyecto final de Análisis y Diseño de Algoritmos (ADA) · Equipo 2

![C#](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=csharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Consola](https://img.shields.io/badge/Interfaz-Consola-4D4D4D?style=for-the-badge)
![Compila](https://img.shields.io/badge/Build-passing-success?style=for-the-badge)

</div>

---

## 📑 Contenido

| Sección | Qué encontrarás |
|---------|-----------------|
| [📌 Qué es](#que-es) | El proyecto en pocas palabras |
| [✨ Características](#caracteristicas) | Lo que el programa sabe hacer |
| [👥 Usuarios y roles](#usuarios) | Credenciales y tiers de descuento |
| [🖥️ Vista previa](#vista-previa) | Salida real de la consola |
| [🧾 Los menús](#menus) | Cada opción, explicada |
| [🚀 Cómo ejecutarlo](#ejecutar) | Clonar y correr |
| [🗂️ Estructura](#estructura) | Archivos y piezas del código |
| [🧠 Algoritmos y complejidad](#complejidad) | Análisis Big-O con diagramas |
| [🎓 Cobertura académica](#cobertura) | Dónde está cada tema de ADA |
| [🗺️ Qué se hará](#roadmap) | Avance y pendientes |
| [🤝 Integrantes](#integrantes) | El equipo |

---

<a id="que-es"></a>

## 📌 Qué es

Una aplicación de consola que lleva el control de los productos de una tiendita: dar de alta
mercancía, registrar ventas, reabastecer el stock, avisar cuándo algo se está agotando y sacar
reportes del valor del inventario.

El acceso está protegido con un inicio de sesión que distingue **dos roles**:

| Rol | Puede hacer |
|-----|-------------|
| 🛠️ **Administrador** | Control total: alta, edición, baja, reportes y alertas |
| 🛒 **Cliente** | Solo consultar y comprar, con descuento según su tier |

Por dentro no usa base de datos ni colecciones dinámicas: está construido sobre un **arreglo de
tamaño fijo** de `struct Producto` con **búsqueda lineal**, y eso es intencional. El objetivo
académico es implementar a mano las estructuras de datos y poder analizar su complejidad, no
delegarle el trabajo a una librería.

---

<a id="caracteristicas"></a>

## ✨ Características

| | Característica | Detalle |
|---|---|---|
| 🔐 | **Inicio de sesión** | Máximo 3 intentos, rechaza campos vacíos |
| 👥 | **Dos roles** | Menús distintos según los permisos |
| 🏅 | **Tiers de descuento** | Básico 5%, Plata 10%, Oro 15% |
| 📦 | **Gestión de productos** | Alta, consulta, edición y baja |
| 🔎 | **Búsqueda por ID** | Con validación de IDs duplicados al registrar |
| 💰 | **Venta** | Desglose de subtotal, descuento y total |
| 🔔 | **Alertas** | Productos agotados y con stock bajo |
| 📊 | **Reportes** | Valor total, extremos de precio y promedio |
| ✅ | **Entrada blindada** | Ningún dato inválido tumba el programa |

---

<a id="usuarios"></a>

## 👥 Usuarios y roles

| Usuario   | Contraseña | Rol           | Acceso                               |
|-----------|------------|---------------|--------------------------------------|
| `admin`   | `1234`     | Administrador | Todo: alta, edición, baja y reportes |
| `cliente` | `1234`     | Cliente       | Solo ver el inventario y comprar     |

> [!WARNING]
> Las credenciales están escritas directamente en el código. Sirven para evaluar el proyecto,
> no para un entorno real — sacarlas de ahí es parte del roadmap.

Al entrar como cliente, el programa pide elegir un tier antes de mostrar el menú:

| Opción | Tier       | Descuento | Ejemplo sobre $100 |
|--------|------------|-----------|--------------------|
| `1`    | 🥉 Básico  | 5%        | Pagas $95          |
| `2`    | 🥈 Plata   | 10%       | Pagas $90          |
| `3`    | 🥇 Oro     | 15%       | Pagas $85          |

---

<a id="vista-previa"></a>

## 🖥️ Vista previa

Salida del programa, tal como la imprime la consola.

<table>
<tr><td width="50%" valign="top">

**Inicio de sesión**

```text
Que desea hacer?
1. Registrar nuevo usuario
2. Ingresar usuarioUsuario: admin
Contraseña: 1234
```

</td><td width="50%" valign="top">

**Selección de tier (solo cliente)**

```text
1. Tier Básico - 5%
2. Tier Plata - 10%
3. Tier Oro - 15%
Elige tu tier: 2
```

</td></tr>
<tr><td width="50%" valign="top">

**Menú de administrador**

```text
MENÚ DE ADMINISTRADOR
1. Agregar producto
2. Ver productos
3. Buscar producto
4. Vender producto
5. Agregar stock
6. Editar producto
7. Eliminar producto
8. Ver alertas
9. Generar reportes
10. Salir
Elige una opción: 2
```

</td><td width="50%" valign="top">

**Menú de cliente**

```text
MENÚ DE CLIENTE
1. Ver inventario
2. Comprar producto
3. Salir
Elige una opción: 1
```

</td></tr>
</table>

**Listado del inventario**

```text
Id: 1 | Nombre: Coca Cola 600ml | Categoría: Bebidas | Precio: $18.50 | Stock: 24
Id: 2 | Nombre: Sabritas | Categoría: Botanas | Precio: $21.00 | Stock: 3
Id: 3 | Nombre: Pan Bimbo | Categoría: Panadería | Precio: $42.00 | Stock: 0
```

<table>
<tr><td width="50%" valign="top">

**Compra con tier Plata (10%)**

```text
Id del producto: 1
Cantidad: 4
Subtotal: $74.00
Descuento: $7.40
Total: $66.60
```

</td><td width="50%" valign="top">

**Alertas de inventario**

```text
Sabritas: stock bajo
Pan Bimbo: agotado
```

</td></tr>
</table>

**Reporte general**

```text
Valor total del inventario: $507.00
Producto más caro: Pan Bimbo
Producto más barato: Coca Cola 600ml
Precio promedio: $27.17
```

**Validación de la entrada** — un dato inválido no rompe nada, se vuelve a pedir:

```text
Elige una opción: abc
Escribe un número válido entre 1 y 10.
Elige una opción: 99
Escribe un número válido entre 1 y 10.
Elige una opción: 2
```

---

<a id="menus"></a>

## 🧾 Los menús

### 🛠️ Menú de administrador

| #  | Opción            | Qué hace                                                     |
|----|-------------------|--------------------------------------------------------------|
| 1  | Agregar producto  | Pide ID (verifica que no exista), nombre, categoría y precio |
| 2  | Ver productos     | Lista todos los productos registrados                        |
| 3  | Buscar producto   | Busca por ID y muestra sus datos                             |
| 4  | Vender producto   | Descuenta el stock, sin descuento aplicado                   |
| 5  | Agregar stock     | Reabastece unidades y evita el desbordamiento del entero     |
| 6  | Editar producto   | Reemplaza nombre, categoría, precio y stock                  |
| 7  | Eliminar producto | Borra el producto y compacta el arreglo                      |
| 8  | Ver alertas       | Agotados (0 unidades) y stock bajo (5 o menos)               |
| 9  | Generar reportes  | Valor total, más caro, más barato y precio promedio          |
| 10 | Salir             | Termina el programa                                          |

### 🛒 Menú de cliente

| # | Opción           | Qué hace                                 |
|---|------------------|------------------------------------------|
| 1 | Ver inventario   | Consulta los productos disponibles       |
| 2 | Comprar producto | Compra aplicando el descuento de su tier |
| 3 | Salir            | Termina el programa                      |

---

<a id="ejecutar"></a>

## 🚀 Cómo ejecutarlo

> [!NOTE]
> Requiere el [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).

```bash
git clone git@github.com:santimaalvz/Inventario-de-una-tiendita-proyecto-final-ADA-.git
cd Inventario-de-una-tiendita-proyecto-final-ADA-
dotnet run
```

El proyecto también se abre directo en Visual Studio o Rider con el archivo
`InventarioTiendita.csproj`.

---

<a id="estructura"></a>

## 🗂️ Estructura

### Archivos del repositorio

| Archivo                                           | Para qué sirve                      |
|---------------------------------------------------|-------------------------------------|
| `Proyecto_Final_ADA_EQUIPO2#_InventarioTienda.cs` | Todo el código del programa         |
| `InventarioTiendita.csproj`                       | Configuración del proyecto .NET 8   |
| `.gitignore`                                      | Excluye `bin/`, `obj/` y temporales |

### Piezas del código

| Pieza                                      | Responsabilidad                                          |
|--------------------------------------------|----------------------------------------------------------|
| `struct Producto`                          | Agrupa ID, nombre, categoría, precio y stock             |
| `struct Usuario`                           | Agrupa usuario, contraseña y si es administrador         |
| Instrucciones de nivel superior            | Login, selección de tier y los dos menús                 |
| `LeerEntero` / `LeerDecimal` / `LeerTexto` | Lectura validada: repiten hasta recibir un dato correcto |
| `LeerIdNuevo`                              | Pide un ID y verifica que no esté repetido               |
| `BuscarProducto`                           | Búsqueda lineal por ID, devuelve la posición o `-1`      |
| `MostrarProducto` / `MostrarProductos`     | Salida de un producto o de todo el inventario            |
| `Vender`                                   | Valida stock, calcula subtotal, descuento y total        |
| `AgregarStock` / `EditarProducto`          | Mantenimiento del inventario                             |
| `EliminarProducto`                         | Borra y compacta el arreglo                              |
| `VerAlertas`                               | Avisos de agotado y de stock bajo                        |
| `Reporte`                                  | Valor total, extremos de precio y promedio               |

---

<a id="complejidad"></a>

## 🧠 Algoritmos y complejidad

El inventario es un `Producto[100]` acompañado de `totalProductos`, que indica cuántas
posiciones están ocupadas. Todas las búsquedas son **secuenciales**.

### 🔎 Búsqueda lineal — `BuscarProducto` → O(n)

Recorre posición por posición hasta encontrar el ID. En el peor caso revisa las `n` ocupadas.

```text
Buscar el ID 7 en un inventario de 5 productos

  posicion    0       1       2       3       4
            +-----+ +-----+ +-----+ +-----+ +-----+
    id      |  3  | |  5  | |  9  | |  7  | |  2  |
            +-----+ +-----+ +-----+ +-----+ +-----+
               X       X       X    ENCONTRADO
               1       2       3       4   <- comparaciones
```

### 🗑️ Borrado con compactación — `EliminarProducto` → O(n)

No deja huecos: desplaza a la izquierda todo lo que venía después y reduce el contador.

```text
Eliminar el producto de la posicion 1

  antes     [ A ] [ B ] [ C ] [ D ] [   ]      totalProductos = 4
                   quitar   \     \
                             \     \  se desplazan una posicion
                              v     v
  despues   [ A ] [ C ] [ D ] [   ] [   ]      totalProductos = 3
```

### 📋 Resumen

| Operación           | Complejidad | Por qué                                  |
|---------------------|-------------|------------------------------------------|
| Agregar producto    | 🟢 **O(1)** | Inserta directo en la siguiente posición |
| Buscar por ID       | 🟡 **O(n)** | Búsqueda lineal, recorre hasta encontrar |
| Validar un ID nuevo | 🟡 **O(n)** | Se apoya en la misma búsqueda lineal     |
| Eliminar producto   | 🟡 **O(n)** | Desplaza los elementos posteriores       |
| Listar productos    | 🟡 **O(n)** | Recorre solo las posiciones ocupadas     |
| Alertas y reporte   | 🟡 **O(n)** | Un recorrido completo del inventario     |

### ⚙️ Constantes de diseño

| Valor  | Significado                                             |
|--------|---------------------------------------------------------|
| `100`  | Máximo de productos distintos que caben en el arreglo   |
| `5`    | Umbral a partir del cual el producto aparece en alertas |
| `0.01` | Precio mínimo aceptado al registrar o editar            |
| `3`    | Intentos permitidos de inicio de sesión                 |

---

<a id="cobertura"></a>

## 🎓 Cobertura académica

Dónde está implementado cada tema de la materia:

| Tema | Dónde está en el código |
|------|-------------------------|
| Arreglos de tamaño fijo | `Producto[100]` y `Usuario[100]` |
| Registros / estructuras | `struct Producto`, `struct Usuario` |
| Búsqueda lineal | `BuscarProducto` |
| Recorridos con `for` | `MostrarProductos`, `VerAlertas`, `Reporte` |
| Ciclos `while` y `do-while` | Login, menús y funciones de lectura |
| Condicionales anidados | Validación del login y de la venta |
| Funciones con parámetros y valor de retorno | `LeerEntero`, `LeerDecimal`, `BuscarProducto` |
| Paso por referencia | `EliminarProducto(..., ref totalProductos)` |
| Desplazamiento de elementos | `EliminarProducto` |
| Acumuladores y contadores | `totalProductos`, `valorTotal`, `sumaPrecios` |
| Búsqueda de máximo y mínimo | `Reporte` (`masCaro`, `masBarato`) |
| Validación de entrada | `LeerEntero`, `LeerDecimal`, `LeerTexto`, `LeerIdNuevo` |
| Análisis de complejidad | [Sección de complejidad](#complejidad) |

---

<a id="roadmap"></a>

## 🗺️ Qué se hará

### ✅ Ya hecho

- [x] `struct Producto` y arreglo de tamaño fijo con contador de posiciones usadas
- [x] Inicio de sesión con 3 intentos y rechazo de campos vacíos
- [x] Dos roles con menús separados: administrador y cliente
- [x] Tiers de descuento del cliente aplicados en la compra
- [x] Alta, consulta, búsqueda, edición y baja de productos
- [x] Venta con desglose de subtotal, descuento y total
- [x] Alertas de agotado y stock bajo
- [x] Reporte de valor total, extremos de precio y promedio
- [x] Validación de todas las entradas del usuario
- [x] `struct Usuario` declarado y el arreglo `usuarios` inicializado al inicio del programa
- [x] `.csproj` y `.gitignore` en el repositorio: el proyecto compila y se ejecuta al clonar

### 🔜 Pendiente

#### Fase 1 — Terminar de integrar el `struct Usuario`
- [ ] Validar el login recorriendo el arreglo `usuarios` en vez de comparar las credenciales a mano
- [ ] Derivar el rol de `IsAdmin` en lugar de comparar el nombre con `"admin"`

> El arreglo ya se llena al arrancar, pero todavía no se lee: el login sigue con las
> credenciales escritas en el `if`. Cerrar esto es lo que permite agregar más usuarios sin
> tocar la lógica.

#### Fase 2 — Registro de usuarios
- [ ] Leer la opción del menú "Registrar nuevo usuario / Ingresar usuario"
- [ ] Implementar el alta de un usuario nuevo en el arreglo `usuarios`
- [ ] Añadir el salto de línea que falta tras `"2. Ingresar usuario"`

> El programa ya muestra ese menú al arrancar, pero no lee la respuesta: cae directo al inicio
> de sesión. El arreglo tiene espacio para 100 usuarios y solo se usan 2.

#### Fase 3 — Moneda
- [ ] Fijar la cultura `es-MX` al formatear los precios

> `ToString("C")` usa la configuración regional del equipo, así que el mismo programa muestra
> pesos en una máquina en español de México y dólares en una configurada en inglés.

#### Fase 4 — Persistencia
- [ ] Guardar y cargar el inventario en archivo (CSV o JSON)

> Hoy todo se pierde al cerrar el programa; es la limitación más grande que tiene.

#### Fase 5 — Búsqueda y ordenamiento
- [ ] Buscar también por nombre, con coincidencia parcial
- [ ] Ordenar por precio, nombre o stock, y añadir búsqueda binaria

> Permite comparar O(n) contra O(log n) con datos reales, que es justo el tema de la materia.

#### Fase 6 — Historial de ventas
- [ ] Registrar cada venta con fecha, producto, cantidad y total
- [ ] Reporte de ventas del día

> Permite saber qué se vende y cuánto se ingresó, no solo qué queda en existencia.

#### Fase 7 — Calidad
- [ ] Separar la lógica del inventario en funciones probables y añadir pruebas con xUnit

> Verifica el borrado con desplazamiento y los cálculos del reporte sin probar a mano en el menú.

---

<a id="integrantes"></a>

## 🤝 Integrantes

| # | Nombre                      |
|---|-----------------------------|
| 1 | Juan Pablo Beltran Salom    |
| 2 | Santiago Morales Alvarez    |
| 3 | Rafael Morgado Briseño      |
| 4 | Geovanni Marrero Echeverria |
| 5 | Jose Eduardo Tejada Miranda |

<div align="center">

---

Proyecto académico · Análisis y Diseño de Algoritmos · Equipo 2

</div>
