//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GraduateSoftware.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Admin
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Admin()
        {
            this.AdminGraduateVerifications = new HashSet<AdminGraduateVerification>();
        }
    
        public string AdminID { get; set; }
        public string AdminName { get; set; }
        public string AdminLastName { get; set; }
        public string AdminPassword { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminGraduateVerification> AdminGraduateVerifications { get; set; }
    }
}
