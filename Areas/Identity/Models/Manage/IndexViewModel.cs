// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HKQTravellingAuthenication.Areas.Identity.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public EditExtraProfileModel profile { get; set; }
        public bool HasPassword { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }

        public string? PhoneNumber { get; set; }

        public bool TwoFactor { get; set; }

        public bool BrowserRemembered { get; set; }

        public string AuthenticatorKey { get; set; }

        [Display(Name = "Họ và tên")]
        public string? FullName { get; set; }

        [Display(Name = "Địa chỉ")]
        public string? HomeAdress { get; set; }

        [Display(Name = "Ngày sinh")]
        public DateTime? BirthDate { get; set; }

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
