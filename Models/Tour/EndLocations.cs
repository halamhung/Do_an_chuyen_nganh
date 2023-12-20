using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HKQTravellingAuthenication.Models.Tour
{
    public class EndLocations
    {
        [Key]
        [Column("END_LOCATION_ID")]
        [Display(Name = "Mã điểm đến")]
        public long EndLocationId { get; set; }

        [Required]
        [Column("END_LOCATION_NAME")]
        [MaxLength(100)]
        [Display(Name = "Tên điểm đến")]
        public string EndLocationName { get; set; }
    }
}
