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
    
    public partial class WorkAreaDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WorkAreaDetail()
        {
            this.Graduates = new HashSet<Graduate>();
        }
    
        public int WADID { get; set; }
        public string WorkAreaDetailName { get; set; }
        public Nullable<int> WorkAreaID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Graduate> Graduates { get; set; }
        public virtual WorkArea WorkArea { get; set; }
    }
}
