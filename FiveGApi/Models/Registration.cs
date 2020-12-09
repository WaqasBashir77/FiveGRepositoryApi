//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FiveGApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Registration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Registration()
        {
            this.Rebate_Details = new HashSet<Rebate_Details>();
        }
    
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string StaffName { get; set; }
        public string Designation { get; set; }
        public string Type { get; set; }
        public string Contact_no { get; set; }
        public string CNIC { get; set; }
        public Nullable<int> Affliate_Staff_ID { get; set; }
        public string Company { get; set; }
        public string Sub_Office { get; set; }
        public Nullable<int> GL_Mapping_ID { get; set; }
        public Nullable<decimal> Resale_Comm { get; set; }
        public string Remarks { get; set; }
        public string Flex_1 { get; set; }
        public string Flex_2 { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_ON { get; set; }
        public string Updated_By { get; set; }
        public string Updated_On { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rebate_Details> Rebate_Details { get; set; }
    }
}
