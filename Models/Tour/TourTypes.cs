using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HKQTravellingAuthenication.Models.Tour
{
    public class TourTypes
    {
        [Key]
        [Column("TOUR_TYPE_ID")]
        [Display(Name = "Mã loại du lịch")]
        public long TourTypeId { get; set; }

        [Required]
        [Column("TOUR_TYPE_NAME")]
        [MaxLength(100)]
        [Display(Name = "Tên loại du lịch")]
        public string TourTypeName { get; set; }
    }
}
