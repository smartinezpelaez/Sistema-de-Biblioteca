# 📚 Sistema de Gestión de Biblioteca

Sistema completo de gestión de biblioteca desarrollado en C# con .NET, Entity Framework Core y SQL Server. Proyecto educativo que demuestra conceptos fundamentales de programación orientada a objetos, patrones de diseño y acceso a datos.

## 🎯 Objetivo del Proyecto

Este proyecto fue creado con fines educativos para demostrar la implementación práctica de:

- **Programación Orientada a Objetos (POO)**: Encapsulación, Herencia, Polimorfismo y Abstracción
- **Principios SOLID**: Single Responsibility, Dependency Inversion
- **Patrones de Diseño**: Repository Pattern, Dependency Injection
- **Entity Framework Core**: ORM para acceso a datos
- **LINQ**: Consultas integradas al lenguaje
- **Colecciones Genéricas**: List, IEnumerable, ICollection
- **Value Types vs Reference Types**: Structs, Enums, Classes
- **Modificadores de Acceso**: public, private, protected, internal
- **Modificadores de Comportamiento**: static, readonly, const, virtual, abstract, override

## 🚀 Características

- ✅ Gestión completa de libros (CRUD)
- ✅ Registro y administración de miembros
- ✅ Sistema de préstamos y devoluciones
- ✅ Búsqueda por autor y categoría
- ✅ Estadísticas en tiempo real
- ✅ Ranking de libros más prestados
- ✅ Control de stock y disponibilidad

## 🛠️ Tecnologías Utilizadas

- **Lenguaje**: C# 10+
- **Framework**: .NET 6/7/8
- **Base de Datos**: SQL Server 2019+
- **ORM**: Entity Framework Core 7+
- **IDE**: Visual Studio 2022

## 📋 Prerequisitos

Antes de ejecutar el proyecto, asegúrate de tener instalado:

- [Visual Studio 2022](https://visualstudio.microsoft.com/) (Community, Professional o Enterprise)
- [SQL Server 2019+](https://www.microsoft.com/sql-server/sql-server-downloads) o SQL Server Express
- [.NET 6 SDK](https://dotnet.microsoft.com/download) o superior

## ⚙️ Instalación y Configuración

### 1. Clonar el Repositorio

```bash
git clone https://github.com/smartinezpelaez/sistema-biblioteca.git
cd sistema-biblioteca
```

### 2. Crear la Base de Datos

Abre **SQL Server Management Studio (SSMS)** y ejecuta el script ubicado en:

```
/Database/CreateDatabase.sql
```

Este script creará:
- Base de datos `BibliotecaDB`
- Tablas: Categorias, Libros, Miembros, Prestamos
- Datos de ejemplo para pruebas

### 3. Configurar la Cadena de Conexión

Abre el archivo `Program.cs` y modifica la constante `CONNECTION_STRING` según tu configuración:

```csharp
// Para SQL Server local con autenticación de Windows
private const string CONNECTION_STRING = 
    "Server=localhost;Database=BibliotecaDB;Trusted_Connection=True;TrustServerCertificate=True;";

// Para SQL Server Express
private const string CONNECTION_STRING = 
    "Server=localhost\\SQLEXPRESS;Database=BibliotecaDB;Trusted_Connection=True;TrustServerCertificate=True;";

// Para SQL Server con usuario y contraseña
private const string CONNECTION_STRING = 
    "Server=localhost;Database=BibliotecaDB;User Id=tu_usuario;Password=tu_password;TrustServerCertificate=True;";
```

### 4. Instalar Paquetes NuGet

En Visual Studio, abre la **Consola del Administrador de Paquetes** y ejecuta:

```bash
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
```

O usando .NET CLI:

```bash
dotnet restore
```

### 5. Compilar y Ejecutar

Presiona **F5** en Visual Studio o ejecuta:

```bash
dotnet run
```

## 📖 Uso del Sistema

Al ejecutar la aplicación, verás un menú interactivo con las siguientes opciones:

```
--- MENÚ PRINCIPAL ---
1. Ver todos los libros
2. Buscar libros por autor
3. Realizar préstamo
4. Devolver libro
5. Ver estadísticas
6. Libros más prestados
7. Préstamos activos
0. Salir
```

### Ejemplos de Uso

#### Realizar un Préstamo
```
Opción: 3
ID del libro: 1
ID del miembro: 1
Días de préstamo: 14
✓ Préstamo realizado exitosamente
```

#### Buscar Libros por Autor
```
Opción: 2
Ingrese nombre del autor: García Márquez
=== LIBROS DE 'García Márquez' ===
Cien Años de Soledad por Gabriel García Márquez - ISBN: 978-0307474728
Categoría: Ficción
```

## 🗂️ Estructura del Proyecto

```
SistemaBiblioteca/
│
├── Models/                    # Entidades del dominio
│   ├── EntidadBase.cs        # Clase base abstracta
│   ├── Libro.cs
│   ├── Miembro.cs
│   ├── Prestamo.cs
│   └── Categoria.cs
│
├── DTOs/                      # Data Transfer Objects
│   ├── EstadisticasBiblioteca.cs (struct)
│   └── EstadoLibro.cs (enum)
│
├── Interfaces/                # Contratos (abstracción)
│   ├── IRepositorio.cs
│   └── IServicioBiblioteca.cs
│
├── Data/                      # Contexto de base de datos
│   └── BibliotecaContext.cs
│
├── Repositorios/              # Implementación de repositorios
│   └── RepositorioGenerico.cs
│
├── Servicios/                 # Lógica de negocio
│   └── ServicioBiblioteca.cs
│
├── Database/                  # Scripts SQL
│   └── CreateDatabase.sql
│
├── Program.cs                 # Punto de entrada
├── .gitignore
└── README.md
```

## 🎓 Conceptos Demostrados

### Programación Orientada a Objetos

#### Encapsulación
```csharp
public class Libro : EntidadBase
{
    private decimal _precio; // Campo privado
    
    public decimal Precio    // Propiedad pública
    {
        get => _precio;
        set => _precio = value > 0 ? value : 0;
    }
}
```

#### Herencia
```csharp
public abstract class EntidadBase
{
    public int Id { get; set; }
}

public class Libro : EntidadBase  // Hereda de EntidadBase
{
    public string Titulo { get; set; }
}
```

#### Polimorfismo
```csharp
public virtual string ObtenerInformacion()
{
    return $"{Titulo} por {Autor}";
}

// En clase derivada
public override string ObtenerInformacion()
{
    return base.ObtenerInformacion() + $" - Edición Especial";
}
```

#### Abstracción
```csharp
public interface IRepositorio<T>
{
    IEnumerable<T> ObtenerTodos();
    T ObtenerPorId(int id);
}
```

### Tipos de Datos

#### Value Types (Structs)
```csharp
public struct EstadisticasBiblioteca
{
    public int TotalLibros { get; set; }
    public int LibrosDisponibles { get; set; }
}
```

#### Reference Types (Classes)
```csharp
public class Libro  // Reference type
{
    public string Titulo { get; set; }
}
```

### Colecciones Genéricas

```csharp
// List<T> - Lista dinámica
List<Libro> libros = new List<Libro>();

// IEnumerable<T> - Secuencia de lectura
IEnumerable<Libro> query = context.Libros.Where(l => l.Disponible);

// ICollection<T> - Navegación en EF
public virtual ICollection<Prestamo> Prestamos { get; set; }
```

### LINQ

```csharp
// Consultas complejas
var topLibros = context.Prestamos
    .GroupBy(p => p.LibroId)
    .Select(g => new { 
        LibroId = g.Key, 
        Total = g.Count() 
    })
    .OrderByDescending(x => x.Total)
    .Take(5);
```

## 🗄️ Modelo de Base de Datos

```
┌─────────────┐         ┌──────────┐         ┌───────────┐
│ Categorias  │         │  Libros  │         │  Miembros │
├─────────────┤         ├──────────┤         ├───────────┤
│ Id (PK)     │────┐    │ Id (PK)  │    ┌────│ Id (PK)   │
│ Nombre      │    └───<│CategoriaId│    │    │ Nombre    │
│ Descripcion │         │ Titulo   │    │    │ Email     │
└─────────────┘         │ Autor    │    │    │ Activo    │
                        │ ISBN     │    │    └───────────┘
                        │Disponible│    │
                        └──────────┘    │
                             │          │
                             └─┐     ┌──┘
                               │     │
                        ┌──────▼─────▼───┐
                        │   Prestamos    │
                        ├────────────────┤
                        │ Id (PK)        │
                        │ LibroId (FK)   │
                        │ MiembroId (FK) │
                        │ FechaPrestamo  │
                        │ Devuelto       │
                        └────────────────┘
```

## 🤝 Contribuciones

Las contribuciones son bienvenidas. Por favor:

1. Fork el proyecto
2. Crea una rama para tu característica (`git checkout -b feature/NuevaCaracteristica`)
3. Commit tus cambios (`git commit -m 'Agregar nueva característica'`)
4. Push a la rama (`git push origin feature/NuevaCaracteristica`)
5. Abre un Pull Request

## 📝 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

## 👨‍💻 Autor

**Tu Nombre**
- GitHub: [@tu-usuario](https://github.com/smartinezpelaez)
- LinkedIn: [Tu Perfil](https://linkedin.com/in/smartinezpelaez)
- Email: tu.email@ejemplo.com

## 🙏 Agradecimientos

- Documentación de [Microsoft .NET](https://docs.microsoft.com/dotnet/)
- Comunidad de [Stack Overflow](https://stackoverflow.com/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)

---

⭐ Si este proyecto te fue útil, considera darle una estrella en GitHub

**Última actualización**: Enero 2025