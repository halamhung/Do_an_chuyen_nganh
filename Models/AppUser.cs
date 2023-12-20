using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HKQTravellingAuthenication.Models
{
    public class AppUser : IdentityUser
    {
        [Column(TypeName = "nvarchar")]
        [StringLength(400)]
        public string? HomeAdress { get; set; }

        // [Required]       
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(200)]
        public string? FullName { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(200)]
        public string? NewCitizenIdentification { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(200)]
        public string? OldCitizenIdentification { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(10)]
        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfInssuance { get; set; } //ngày cấp căn cước công dân
    }
}
