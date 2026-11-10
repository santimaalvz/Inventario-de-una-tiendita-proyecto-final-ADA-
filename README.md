# 🏪 Inventario de una Tiendita

Sistema de inventario en consola para una tienda pequeña, con roles de administrador y cliente.
Proyecto final de **Análisis y Diseño de Algoritmos (ADA)** — Equipo 2.

![C#](https://img.shields.io/badge/C%23-12.0-239120?style=flat-square&logo=csharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![Consola](https://img.shields.io/badge/Interfaz-Consola-4D4D4D?style=flat-square)
![Estado](https://img.shields.io/badge/Estado-En%20desarrollo-orange?style=flat-square)
![Licencia](https://img.shields.io/badge/Licencia-MIT-blue?style=flat-square)

---

## 📌 Qué es

Una aplicación de consola que lleva el control de los productos de una tiendita: dar de alta
mercancía, registrar ventas, reabastecer el stock, avisar cuándo algo se está agotando y sacar
reportes del valor del inventario.

El acceso está protegido con un inicio de sesión que distingue **dos roles**:

- **Administrador** — control total del inventario (alta, edición, baja, reportes y alertas)
- **Cliente** — solo consulta y compra, con un **descuento según su tier**

Por dentro no usa base de datos ni colecciones dinámicas: está construido sobre un **arreglo de
tamaño fijo** de `struct Producto` con **búsqueda lineal**, y eso es intencional. El objetivo
académico es implementar a mano las estructuras de datos y poder analizar su complejidad, no
delegarle el trabajo a una librería.

---

## ✨ Características

- 🔐 **Inicio de sesión** con máximo 3 intentos y rechazo de campos vacíos
- 👥 **Dos roles** con menús distintos según los permisos de cada uno
- 🏅 **Tiers de descuento** para el cliente: Básico 5%, Plata 10%, Oro 15%
- 📦 **Alta, consulta, edición y baja** de productos (ID, nombre, categoría, precio y stock)
- 🔎 **Búsqueda por ID** con validación de IDs duplicados al registrar
- 💰 **Venta** con desglose de subtotal, descuento aplicado y total
- 🔔 **Alertas automáticas** de productos agotados y de stock bajo
- 📊 **Reportes** del valor total, producto más caro y más barato, y precio promedio
- ✅ **Validación de toda la entrada**: ningún dato inválido tumba el programa, lo vuelve a pedir

---

## 👥 Usuarios y roles

| Usuario   | Contraseña | Rol           | Puede                                |
|-----------|------------|---------------|--------------------------------------|
| `admin`   | `1234`     | Administrador | Todo: alta, edición, baja y reportes |
| `cliente` | `1234`     | Cliente       | Solo ver el inventario y comprar     |

> ⚠️ Las credenciales están escritas directamente en el código. Sirven para evaluar el proyecto,
> no para un entorno real — sacarlas de ahí es parte del roadmap.

Al entrar como cliente, el programa pide elegir un tier antes de mostrar el menú:

| Opción | Tier    | Descuento |
|--------|---------|-----------|
| 1      | Básico  | 5%        |
| 2      | Plata   | 10%       |
| 3      | Oro     | 15%       |

---

## 🧾 Menú de administrador

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

## 🛒 Menú de cliente

| # | Opción           | Qué hace                                 |
|---|------------------|------------------------------------------|
| 1 | Ver inventario   | Consulta los productos disponibles       |
| 2 | Comprar producto | Compra aplicando el descuento de su tier |
| 3 | Salir            | Termina el programa                      |

---

## 🚀 Cómo ejecutarlo

**Requisito:** [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

El repositorio contiene el archivo fuente suelto, sin archivo de proyecto, así que hay que
crear uno para compilarlo:

```bash
git clone git@github.com:santimaalvz/Inventario-de-una-tiendita-proyecto-final-ADA-.git
cd Inventario-de-una-tiendita-proyecto-final-ADA-

dotnet new console -o InventarioTiendita
cp "Proyecto_Final_ADA_EQUIPO2#_InventarioTienda.cs" InventarioTiendita/Program.cs

cd InventarioTiendita
dotnet run
```

El código usa instrucciones de nivel superior, por eso funciona directo como `Program.cs`.

---

## 🗂️ Estructura del código

Todo vive en un solo archivo. Estas son sus piezas:

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

## 🧠 Estructuras de datos y complejidad

El inventario es un `Producto[100]` acompañado de `totalProductos`, que indica cuántas
posiciones están ocupadas. Todas las búsquedas son **secuenciales**, así que el análisis
queda así:

| Operación           | Complejidad | Por qué                                  |
|---------------------|-------------|------------------------------------------|
| Agregar producto    | **O(1)**    | Inserta directo en la siguiente posición |
| Buscar por ID       | **O(n)**    | Búsqueda lineal, recorre hasta encontrar |
| Validar un ID nuevo | **O(n)**    | Se apoya en la misma búsqueda lineal     |
| Eliminar producto   | **O(n)**    | Desplaza los elementos posteriores       |
| Listar productos    | **O(n)**    | Recorre solo las posiciones ocupadas     |
| Alertas y reporte   | **O(n)**    | Un recorrido completo del inventario     |

Constantes de diseño:

- **100** → máximo de productos distintos que caben en el arreglo
- **5 unidades** → umbral a partir del cual el producto aparece en las alertas
- **0.01** → precio mínimo aceptado al registrar o editar

---

## 🗺️ Qué se hará

### Ya hecho

- [x] `struct Producto` y arreglo de tamaño fijo con contador de posiciones usadas
- [x] Inicio de sesión con 3 intentos y rechazo de campos vacíos
- [x] Dos roles con menús separados: administrador y cliente
- [x] Tiers de descuento del cliente aplicados en la compra
- [x] Alta, consulta, búsqueda, edición y baja de productos
- [x] Venta con desglose de subtotal, descuento y total
- [x] Alertas de agotado y stock bajo
- [x] Reporte de valor total, extremos de precio y promedio
- [x] Validación de todas las entradas del usuario

### Pendiente

**Fase 1 — Integrar el `struct Usuario`**
- [ ] Cerrar la declaración del arreglo `usuarios` y moverla antes de los menús
- [ ] Validar el login recorriendo ese arreglo en vez de comparar las credenciales a mano
- [ ] Derivar el rol de `IsAdmin` en lugar de comparar el nombre con `"admin"`
  → *El struct ya existe, pero todavía no se usa: el login sigue con las credenciales escritas
  en el `if`. Cerrar esto es lo que permite agregar más usuarios sin tocar la lógica.*

**Fase 2 — Archivo de proyecto**
- [ ] Añadir el `.csproj` al repositorio
  → *Hoy el repo solo tiene el `.cs` suelto, así que quien clone tiene que crear el proyecto a
  mano antes de poder ejecutarlo.*

**Fase 3 — Moneda**
- [ ] Fijar la cultura `es-MX` al formatear los precios
  → *`ToString("C")` usa la configuración regional del equipo, así que el mismo programa muestra
  pesos en una máquina en español de México y dólares en una configurada en inglés.*

**Fase 4 — Persistencia**
- [ ] Guardar y cargar el inventario en archivo (CSV o JSON)
  → *Hoy todo se pierde al cerrar el programa; es la limitación más grande que tiene.*

**Fase 5 — Búsqueda y ordenamiento**
- [ ] Buscar también por nombre, con coincidencia parcial
- [ ] Ordenar por precio, nombre o stock, y añadir búsqueda binaria
  → *Permite comparar O(n) contra O(log n) con datos reales, que es justo el tema de la materia.*

**Fase 6 — Historial de ventas**
- [ ] Registrar cada venta con fecha, producto, cantidad y total
- [ ] Reporte de ventas del día
  → *Permite saber qué se vende y cuánto se ingresó, no solo qué queda en existencia.*

**Fase 7 — Calidad**
- [ ] Separar la lógica del inventario en funciones probables y añadir pruebas con xUnit
  → *Verifica el borrado con desplazamiento y los cálculos del reporte sin probar a mano en el menú.*

---

## 👤 Autor

**Santiago Morales Alvarez** — [@santimaalvz](https://github.com/santimaalvz)

🎓 0308821@up.edu.mx  ·  📧 santi.morales08@outlook.com

## 📄 Licencia

MIT. Proyecto desarrollado con fines académicos.
