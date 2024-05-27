using System.ComponentModel.DataAnnotations;

namespace ReservaCanchasAPI.Models
{
    public class TipoCancha
    {
        [Key]
        public string? TCanchaId { get; set; }
        public string? Descripcion { get; set; }
        public decimal MontoReserva { get; set; }
        //public required ICollection<Canchas> Canchas { get; set; }
    }
}
