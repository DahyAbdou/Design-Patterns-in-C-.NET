using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Domain.Entities
{
    public partial class User
    {
        public int Id { get; set; }
        public string IdiqamaNumber { get; set; }
        public string SponsorNumber { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ProfilePicture { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatorUserId { get; set; }
        public DateTime? LastModificationDate { get; set; }
        public int? LastModifierUserId { get; set; }
        public int? NationalityId { get; set; }
        public string Gender { get; set; }
        public bool? IsPractitioner { get; set; }
        public bool? VerifiedByIAM { get; set; }
        public bool? IsSaudi { get; set; }
        public bool MobileNumberVerified { get; set; }
    }
}
