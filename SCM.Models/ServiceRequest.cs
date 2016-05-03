//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SCM
{
    using System;
    using System.Collections.Generic;
    
    public partial class ServiceRequest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ServiceRequest()
        {
            this.Tags = new HashSet<Tag>();
        }
    
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public System.DateTime RequestDate { get; set; }
        public int StatusId { get; set; }
        public System.DateTime StatusDate { get; set; }
        public int CenterId { get; set; }
        public string RQN { get; set; }
        public string ReceiptNo { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string ProductId { get; set; }
        public string Model { get; set; }
        public string SN { get; set; }
        public Nullable<int> EngineerId { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> ClosingDate { get; set; }
        public Nullable<int> PendingReasonId { get; set; }
        public Nullable<int> CancelReasonId { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual CancelReason CancelReason { get; set; }
        public virtual Center Center { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Department Department { get; set; }
        public virtual Engineer Engineer { get; set; }
        public virtual PendingReason PendingReason { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ExServiceRequest ExServiceRequest { get; set; }
        public virtual Product Product { get; set; }
    }
}
