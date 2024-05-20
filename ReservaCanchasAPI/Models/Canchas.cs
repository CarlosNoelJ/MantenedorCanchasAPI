using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservaCanchasAPI.Models
{
    public class Canchas
    {
        [Key]
        public string? CanchaId { get; set; }
        [Required]
        public string? TCanchaId { get; set; }
        [Required]
        [StringLength(2)]
        public string? Estado { get; set; }
        [ForeignKey("TCanchaId")]
        [NotMapped]
        public required TipoCancha TipoCancha { get; set; }

    }
}
