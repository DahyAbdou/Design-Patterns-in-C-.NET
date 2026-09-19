using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DoctorJobTitle
    {
        public int Id { get; set; }
        public string NormalizedNameAr { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public DateTime CreatedDate { get; set; }
        public string RegistryId { get; set; }
    }
}
