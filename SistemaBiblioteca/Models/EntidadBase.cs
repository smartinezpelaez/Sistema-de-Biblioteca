using System.ComponentModel.DataAnnotations;

namespace SistemaBiblioteca.Models
{
    public abstract class EntidadBase
    {
        public int Id { get; set; }
    }


    public class Categoria : EntidadBase
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; }

        // Navegación - Relación uno a muchos
        public virtual ICollection<Libro> Libros { get; set; }
    }

    public class Libro : EntidadBase
    {
        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(100)]
        public string Autor { get; set; }

        [Required]
        [StringLength(20)]
        public string ISBN { get; set; }

        public int CategoriaId { get; set; }
        public bool Disponible { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public int Stock { get; set; }

        // Navegación
        public virtual Categoria Categoria { get; set; }
        public virtual ICollection<Prestamo> Prestamos { get; set; }

        // Método virtual - Demuestra polimorfismo
        public virtual string ObtenerInformacion()
        {
            return $"{Titulo} por {Autor} - ISBN: {ISBN}";
        }
    }

    public class Miembro : EntidadBase
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }

        // Navegación
        public virtual ICollection<Prestamo> Prestamos { get; set; }
    }

    public class Prestamo : EntidadBase
    {
        public int LibroId { get; set; }
        public int MiembroId { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaDevolucionEsperada { get; set; }
        public DateTime? FechaDevolucionReal { get; set; }
        public bool Devuelto { get; set; }

        // Navegación
        public virtual Libro Libro { get; set; }
        public virtual Miembro Miembro { get; set; }

        // Propiedad calculada - Solo lectura
        public int DiasTranscurridos
        {
            get
            {
                DateTime fechaFin = FechaDevolucionReal ?? DateTime.Now;
                return (fechaFin - FechaPrestamo).Days;
            }
        }
    }

}
