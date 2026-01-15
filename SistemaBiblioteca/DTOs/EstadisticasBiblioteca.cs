namespace SistemaBiblioteca.DTOs
{
    public struct EstadisticasBiblioteca
    {
        public int TotalLibros { get; set; }
        public int LibrosDisponibles { get; set; }
        public int LibrosPrestados { get; set; }
        public int TotalMiembros { get; set; }
        public int MiembrosActivos { get; set; }

        public override string ToString()
        {
            return $"Total Libros: {TotalLibros}, Disponibles: {LibrosDisponibles}, " +
                   $"Prestados: {LibrosPrestados}, Miembros: {TotalMiembros}";
        }
    }

    // Enum - Value Type
    public enum EstadoLibro
    {
        Disponible = 0,
        Prestado = 1,
        Mantenimiento = 2,
        Perdido = 3
    }
}
