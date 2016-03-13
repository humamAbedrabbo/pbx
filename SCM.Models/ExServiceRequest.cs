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
    
    public partial class ExServiceRequest
    {
        public int Id { get; set; }
        public Nullable<bool> B2BFlag { get; set; }
        public Nullable<System.DateTime> CompletionDate { get; set; }
        public Nullable<System.DateTime> InputDate { get; set; }
        public Nullable<System.DateTime> PullingDate { get; set; }
        public string Dealer { get; set; }
        public string DealerName { get; set; }
        public string DealerReceiptNo { get; set; }
        public string ASCRemarks { get; set; }
        public Nullable<System.DateTime> SchComplaintDate { get; set; }
        public Nullable<int> SchComplaintCount { get; set; }
        public string SchComplaintRemarks { get; set; }
        public string ASCClaimNo { get; set; }
        public string EsnImeiNo { get; set; }
        public string OutModel { get; set; }
        public Nullable<System.DateTime> ReceiptDate { get; set; }
        public Nullable<System.DateTime> TransferSendDate { get; set; }
        public Nullable<System.DateTime> TransferReceiptDate { get; set; }
        public Nullable<System.DateTime> FirstPromiseDate { get; set; }
        public string Schedule { get; set; }
        public Nullable<System.DateTime> PromiseDate { get; set; }
        public Nullable<System.DateTime> Schedule1 { get; set; }
        public Nullable<int> DelayFromPromiseDate { get; set; }
        public Nullable<int> DelayFromReceiptDate { get; set; }
        public Nullable<System.DateTime> TransferApprovalDate { get; set; }
    
        public virtual ServiceRequest ServiceRequest { get; set; }
    }
}
