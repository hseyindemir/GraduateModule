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
    using System.ComponentModel.DataAnnotations;

    public partial class Graduate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Graduate()
        {
            this.AdminGraduateVerifications = new HashSet<AdminGraduateVerification>();
        }
    
        public string StudentID { get; set; }
        public string GraduateName { get; set; }
        public string GraduateLastName { get; set; }
        public Nullable<int> GraduateYear { get; set; }
        public Nullable<int> WorkAreaID { get; set; }
        public Nullable<int> WorkAreaDetailID { get; set; }
        public string GraduateCompany { get; set; }
        public string GraduateTitle { get; set; }
        public string GraduateMail { get; set; }
        public string GraduatePhone { get; set; }
        public string StudentPassword { get; set; }
    
        public virtual WorkArea WorkArea { get; set; }
        public virtual WorkAreaDetail WorkAreaDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminGraduateVerification> AdminGraduateVerifications { get; set; }
    }
}
