using System;
using System.ComponentModel.DataAnnotations;

namespace HKQTravellingAuthenication.Areas.Identity.Models.ManageViewModels
{
    public class EditExtraProfileModel
    {
        [Display(Name = "Tên tài khoản")]
        public string UserName { get; set; }

        [Display(Name = "Địa chỉ email")]
        public string UserEmail { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Địa chỉ")]
        public string? HomeAdress { get; set; }


        [Display(Name = "Ngày sinh")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Họ và tên")]
        public string? FullName { get; set; }

        [Display(Name = "Mã CCCD mới")]
        public string? NewCitizenIdentification { get; set; }

        [Display(Name = "Mã CCCD cũ")]
        public string? OldCitizenIdentification { get; set; }

        [Display(Name = "Giới tính")]
        public string? Gender { get; set; }

        [Display(Name = "Ngày cấp CCCD")]
        public DateTime? DateOfInssuance { get; set; } //ngày cấp căn cước công dân
    }
}