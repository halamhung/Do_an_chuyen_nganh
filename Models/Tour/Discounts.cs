using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HKQTravellingAuthenication.Models.Tour
{
    public class Discounts
    {
        [Key]
        [Column("DIS_ID")]
        [Display(Name = "Mã giảm giá")]
        public long DiscountId { get; set; }

        [Column("DIS_PER")]
        [Display(Name = "Phần trăm giảm")]
        public double? DiscountPercentage { get; set; }

        [Column("DIS_NAME")]
        [MaxLength(1000)]
        [Display(Name = "Tên mã giảm giá")]
        public string DiscountName { get; set; }

        [Column("DIS_DATE_START")]
        [Display(Name = "Ngày bắt đầu giảm")]
        public DateTime? DiscountDateStart { get; set; }

        [Column("DIS_DATE_END")]
        [Display(Name = "Ngày kết thúc giảm")]
        public DateTime? DiscountDateEnd { get; set; }

        [Column("STATUS")]
        [Display(Name = "Trạng thái")]
        public int? Status { get; set; }
    }
}
