using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HKQTravellingAuthenication.Models.Tour
{
    public class Tours
    {
        [Key]
        [Column("TOUR_ID")]
        [Display(Name = "Mã tour")]
        public long TourId { get; set; }

        [Column("TOUR_NAME")]
        [MaxLength(200)]
        [Required(ErrorMessage = "Vui lòng nhập tên lịch trình.")]
        [Display(Name = "Tên lịch trình")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên lịch trình phải có độ dài từ 3 đến 50 ký tự.")]
        public string TourName { get; set; }

        [Column("DESCRIPTION")]
        [Display(Name = "Nội dung")]
        public string? Description {  get; set; }

        [Column("PRICE")]
        [Required(ErrorMessage = "Vui lòng nhập giá.")]
        [Display(Name = "Giá")]
        [Range(0, int.MaxValue, ErrorMessage = "Giá phải là số không âm.")]
        public int? Price { get; set; }

        [Column("START_DATE")]
        [Required(ErrorMessage = "Vui lòng nhập ngày bắt đầu.")]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime? StartDate { get; set; }

        [Column("END_DATE")]
        [Required(ErrorMessage = "Vui lòng nhập ngày kết thúc.")]
        [Display(Name = "Ngày kết thúc")]
        public DateTime? EndDate { get; set; }

        [Column("STATUS")]
        [Display(Name = "Trạng thái")]
        public int? Status { get; set; }

        [Column("CREATION_DATE")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "Ngày tạo")]
        public DateTime? CreationDate { get; set; }

        [Column("UPDATE_DATE")]
        [Display(Name = "Ngày cập nhật")]
        public DateTime? UpdateDate { get; set; }

        //Rules

        [Column("REMAINING")]
        [Display(Name = "SL tồn")]
        public int? Remaining { get; set; }

        [Column("PRICE_INCLUDE")]
        [MaxLength(1000)]
        [Display(Name = "Giá bao gồm")]
        public string? PriceInclude { get; set; }

        [Column("PRICE_NOT_INCLUDE")]
        [Display(Name = "Giá không bao gồm")]
        [MaxLength(1000)]
        public string? PriceNotInclude { get; set; }

        [Column("SURCHARGE")]
        [MaxLength(1000)]
        [Display(Name = "Phụ thu")]
        public string? Surcharge { get; set; }

        [Column("CANCLE_CHANGE")]
        [MaxLength(1000)]
        [Display(Name = "Hủy đổi")]
        public string? CancelChange { get; set; }

        [Column("NOTE")]
        [MaxLength(1000)]
        [Display(Name = "Lưu ý")]
        public string? Note { get; set; }

        //Khóa ngoại
        [Column("DIS_ID")]
        [Display(Name = "Tên mã giảm giá")]
        public long? DiscountId { get; set; }

        [ForeignKey("DiscountId")]
        public Discounts discounts { get; set; }

        [Column("START_LOCATION_ID")]
        [Display(Name = "Nơi khởi hành")]
        public long? StartLocationId { get; set; }

        [ForeignKey("StartLocationId")]
        public StartLocations startLocations { get; set; }

        [Column("END_LOCATION_ID")]
        [Display(Name = "Nơi kết thúc")]
        public long? EndLocationId { get; set; }

        [ForeignKey("EndLocationId")]
        public EndLocations endLocations { get; set; }

        [Column("TOUR_TYPE_ID")]
        [Display(Name = "Loại tour")]
        public long? TourTypeId { get; set; }

        [ForeignKey("TourTypeId")]
        public TourTypes tourTypes{ get; set; }

        // Danh sách hình ảnh cho từng tour
        public List<TourImages> TourImages { get; set; }
    }
}
