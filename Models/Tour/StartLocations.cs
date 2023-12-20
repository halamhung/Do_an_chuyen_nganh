using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HKQTravellingAuthenication.Models.Tour
{
    public class StartLocations
    {
        [Key]
        [Column("START_LOCATION_ID")]
        [Display(Name = "Mã điểm đi")]
        public long StartLocationId { get; set; }

        [Required]
        [Column("START_LOCATION_NAME")]
        [Display(Name = "Tên điểm đi")]
        [MaxLength(100)]
        public string StartLocationName { get; set; }
    }
}
