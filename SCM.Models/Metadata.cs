using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SCM
{
    #region Centers
    public class CenterMetadata
    {
        [Display(Name = "Center_Id", ResourceType = typeof(AppViews))]
        public int Id { get; set; }
        [Display(Name = "Center_Name", ResourceType = typeof(AppViews))]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Display(Name = "Center_Code", ResourceType = typeof(AppViews))]
        [Required]
        [StringLength(20)]
        public string Code { get; set; }
    }
    [MetadataType(typeof(CenterMetadata))]
    public partial class Center
    {
    }
    #endregion

    #region Cities
    public class CityMetadata
    {
        [Display(Name = "City_Id", ResourceType = typeof(AppViews))]
        public int Id { get; set; }
        [Display(Name = "City_Name", ResourceType = typeof(AppViews))]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
    [MetadataType(typeof(CityMetadata))]
    public partial class City
    {
    }
    #endregion

    #region Regions
    public class RegionMetadata
    {
        [Display(Name = "Region_Id", ResourceType = typeof(AppViews))]
        public int Id { get; set; }
        [Display(Name = "Region_CityId", ResourceType = typeof(AppViews))]
        [Required]
        public int CityId { get; set; }
        [Display(Name = "Region_Name", ResourceType = typeof(AppViews))]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
    [MetadataType(typeof(RegionMetadata))]
    public partial class Region
    {
    }
    #endregion

    #region Departments
    public class DepartmentMetadata
    {
        [Display(Name = "Department_Id", ResourceType = typeof(AppViews))]
        public int Id { get; set; }
        [Display(Name = "Department_Name", ResourceType = typeof(AppViews))]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
    [MetadataType(typeof(DepartmentMetadata))]
    public partial class Department
    {
    }
    #endregion

    #region Products
    public class ProductMetadata
    {
        [Display(Name = "Product_Id", ResourceType = typeof(AppViews))]
        public string Id { get; set; }
        [Display(Name = "Product_Name", ResourceType = typeof(AppViews))]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Display(Name = "IsActive", ResourceType = typeof(AppViews))]
        public bool IsActive { get; set; }
    }
    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
    }
    #endregion

    #region Engineers
    public class EngineerMetadata
    {
        [Display(Name = "Engineer_Id", ResourceType = typeof(AppViews))]
        public int Id { get; set; }
        [Display(Name = "Engineer_DepartmentId", ResourceType = typeof(AppViews))]
        [Required]
        public int DepartmentId { get; set; }
        [Display(Name = "Engineer_Name", ResourceType = typeof(AppViews))]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Display(Name = "Engineer_Phone", ResourceType = typeof(AppViews))]
        [StringLength(20)]
        public string Phone { get; set; }
        [Display(Name = "IsActive", ResourceType = typeof(AppViews))]
        public bool IsActive { get; set; }
    }
    [MetadataType(typeof(EngineerMetadata))]
    public partial class Engineer
    {
    }
    #endregion

    #region Customers
    public class CustomerMetadata
    {
        [Display(Name = "Customer_Id", ResourceType = typeof(AppViews))]
        public int Id { get; set; }
        [Display(Name = "Customer_Name", ResourceType = typeof(AppViews))]
        [Required]
        [StringLength(75)]
        public string Name { get; set; }
        [Display(Name = "Customer_Phone", ResourceType = typeof(AppViews))]
        [StringLength(20)]
        public string Phone { get; set; }
        [Display(Name = "Customer_Mobile", ResourceType = typeof(AppViews))]
        [StringLength(20)]
        public string Mobile { get; set; }
        [Display(Name = "Customer_CityId", ResourceType = typeof(AppViews))]
        public Nullable<int> CityId { get; set; }
        [Display(Name = "Customer_RegionId", ResourceType = typeof(AppViews))]
        public Nullable<int> RegionId { get; set; }
        [Display(Name = "Customer_Address", ResourceType = typeof(AppViews))]
        [StringLength(200)]
        public string Address { get; set; }
        [Display(Name = "Customer_IsBlackListed", ResourceType = typeof(AppViews))]
        public bool IsBlackListed { get; set; }
        [Display(Name = "Customer_Comments", ResourceType = typeof(AppViews))]
        public string Comments { get; set; }
    }
    [MetadataType(typeof(CustomerMetadata))]
    public partial class Customer
    {
    }
    #endregion

    #region Tags
    public class TagMetadata
    {
        [Display(Name = "Tag_Id", ResourceType = typeof(AppViews))]
        public int Id { get; set; }
        [Display(Name = "Tag_TagType", ResourceType = typeof(AppViews))]
        [Required]
        [StringLength(1)]
        public string TagType { get; set; }
        [Display(Name = "Tag_Name", ResourceType = typeof(AppViews))]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Display(Name = "Tag_Format", ResourceType = typeof(AppViews))]
        [StringLength(50)]
        public string Format { get; set; }
    }
    [MetadataType(typeof(TagMetadata))]
    public partial class Tag
    {
    }
    #endregion

    #region ServiceRequests
    public class ServiceRequestMetadata
    {
        [Display(Name = "SR_Id", ResourceType = typeof(AppViews))]
        public int Id { get; set; }
        [Display(Name = "SR_CustomerId", ResourceType = typeof(AppViews))]
        [Required]
        public int CustomerId { get; set; }
        [Display(Name = "SR_RequestDate", ResourceType = typeof(AppViews))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true)]
        [Required]
        public System.DateTime RequestDate { get; set; }
        [Display(Name = "SR_StatusId", ResourceType = typeof(AppViews))]
        [Required]
        public int StatusId { get; set; }
        [Display(Name = "SR_StatusDate", ResourceType = typeof(AppViews))]
        [Required]
        public System.DateTime StatusDate { get; set; }
        [Display(Name = "SR_CenterId", ResourceType = typeof(AppViews))]
        [Required]
        public int CenterId { get; set; }
        [Display(Name = "SR_RQN", ResourceType = typeof(AppViews))]
        [StringLength(20)]
        public string RQN { get; set; }
        [Display(Name = "SR_ReceiptNo", ResourceType = typeof(AppViews))]
        [StringLength(20)]
        public string ReceiptNo { get; set; }
        [Display(Name = "SR_DepartmentId", ResourceType = typeof(AppViews))]
        public Nullable<int> DepartmentId { get; set; }
        [Display(Name = "SR_ProductId", ResourceType = typeof(AppViews))]
        public string ProductId { get; set; }
        [Display(Name = "SR_Model", ResourceType = typeof(AppViews))]
        [StringLength(50)]
        public string Model { get; set; }
        [Display(Name = "SR_SN", ResourceType = typeof(AppViews))]
        [StringLength(50)]
        public string SN { get; set; }
        [Display(Name = "SR_EngineerId", ResourceType = typeof(AppViews))]
        public Nullable<int> EngineerId { get; set; }
        [Display(Name = "SR_Description", ResourceType = typeof(AppViews))]
        public string Description { get; set; }
        [Display(Name = "SR_Remarks", ResourceType = typeof(AppViews))]
        [StringLength(100)]
        public string Remarks { get; set; }
        [Display(Name = "SR_Cost", ResourceType = typeof(AppViews))]
        public Nullable<int> Cost { get; set; }
        [Display(Name = "SR_ClosingDate", ResourceType = typeof(AppViews))]
        public Nullable<System.DateTime> ClosingDate { get; set; }
        [Display(Name = "SR_PendingReasonId", ResourceType = typeof(AppViews))]
        public Nullable<int> PendingReasonId { get; set; }
        [Display(Name = "SR_CancelReasonId", ResourceType = typeof(AppViews))]
        public Nullable<int> CancelReasonId { get; set; }
        [Display(Name = "SR_CreatedBy", ResourceType = typeof(AppViews))]
        public string CreatedBy { get; set; }
        [Display(Name = "SR_CreatedOn", ResourceType = typeof(AppViews))]
        public System.DateTime CreatedOn { get; set; }
        [Display(Name = "SR_UpdatedBy", ResourceType = typeof(AppViews))]
        public string UpdatedBy { get; set; }
        [Display(Name = "SR_UpdatedOn", ResourceType = typeof(AppViews))]
        public System.DateTime UpdatedOn { get; set; }
        [Display(Name = "SR_IsDeleted", ResourceType = typeof(AppViews))]
        public bool IsDeleted { get; set; }
    }
    [MetadataType(typeof(ServiceRequestMetadata))]
    public partial class ServiceRequest
    {
    }
    #endregion

    #region PendingReasons
    public class PendingReasonMetadata
    {
        [Display(Name = "PendingReason_Id", ResourceType = typeof(AppViews))]
        public int Id { get; set; }
        [Display(Name = "PendingReason_Reason", ResourceType = typeof(AppViews))]
        [Required]
        [StringLength(75)]
        public string Reason { get; set; }
    }
    [MetadataType(typeof(PendingReasonMetadata))]
    public partial class PendingReason
    {
    }
    #endregion

    #region CancelReasons
    public class CancelReasonMetadata
    {
        [Display(Name = "CancelReason_Id", ResourceType = typeof(AppViews))]
        public int Id { get; set; }
        [Display(Name = "CancelReason_Reason", ResourceType = typeof(AppViews))]
        [Required]
        [StringLength(75)]
        public string Reason { get; set; }
    }
    [MetadataType(typeof(CancelReasonMetadata))]
    public partial class CancelReason
    {
    }
    #endregion

    #region PhoneCalls
    public class PhoneCallMetadata
    {
        [Display(Name = "PhoneCall_Id", ResourceType = typeof(AppViews))]
        public int Id { get; set; }
        [Display(Name = "PhoneCall_CallingId", ResourceType = typeof(AppViews))]
        [StringLength(20)]
        public string CallingId { get; set; }
        [Display(Name = "PhoneCall_CustomerId", ResourceType = typeof(AppViews))]
        public int CustomerId { get; set; }
        [Display(Name = "PhoneCall_ServiceRequestId", ResourceType = typeof(AppViews))]
        public Nullable<int> ServiceRequestId { get; set; }
        [Display(Name = "PhoneCall_Ext", ResourceType = typeof(AppViews))]
        public string Ext { get; set; }
        [Display(Name = "PhoneCall_CallTime", ResourceType = typeof(AppViews))]
        [Required]
        public Nullable<System.DateTime> CallTime { get; set; }
        [Display(Name = "PhoneCall_Remarks", ResourceType = typeof(AppViews))]
        [StringLength(100)]
        public string Remarks { get; set; }
        [Display(Name = "PhoneCall_CreatedBy", ResourceType = typeof(AppViews))]
        public string CreatedBy { get; set; }
        [Display(Name = "PhoneCall_CreatedOn", ResourceType = typeof(AppViews))]
        public System.DateTime CreatedOn { get; set; }
    }
    [MetadataType(typeof(PhoneCallMetadata))]
    public partial class PhoneCall
    {
    }
    #endregion

    #region ExServiceRequests
    public class ExServiceRequestMetaData
    {
        [Display(Name = "ExtRequest", ResourceType = typeof(ExSvc))]
        public int Id { get; set; }
        [Display(Name = "B2BFlag", ResourceType = typeof(ExSvc))]
        public Nullable<bool> B2BFlag { get; set; }
        [Display(Name = "CompletionDate", ResourceType = typeof(ExSvc))]
        public Nullable<System.DateTime> CompletionDate { get; set; }
        [Display(Name = "InputDate", ResourceType = typeof(ExSvc))]
        public Nullable<System.DateTime> InputDate { get; set; }
        [Display(Name = "PullingDate", ResourceType = typeof(ExSvc))]
        public Nullable<System.DateTime> PullingDate { get; set; }
        [Display(Name = "Dealer", ResourceType = typeof(ExSvc))]
        public string Dealer { get; set; }
        [Display(Name = "DealerName", ResourceType = typeof(ExSvc))]
        public string DealerName { get; set; }
        [Display(Name = "DealerReceiptNo", ResourceType = typeof(ExSvc))]
        public string DealerReceiptNo { get; set; }
        [Display(Name = "ASCRemarks", ResourceType = typeof(ExSvc))]
        public string ASCRemarks { get; set; }
        [Display(Name = "SchComplaintDate", ResourceType = typeof(ExSvc))]
        public Nullable<System.DateTime> SchComplaintDate { get; set; }
        [Display(Name = "SchComplaintCount", ResourceType = typeof(ExSvc))]
        public Nullable<int> SchComplaintCount { get; set; }
        [Display(Name = "SchComplaintRemarks", ResourceType = typeof(ExSvc))]
        public string SchComplaintRemarks { get; set; }
        [Display(Name = "ASCClaimNo", ResourceType = typeof(ExSvc))]
        public string ASCClaimNo { get; set; }
        [Display(Name = "EsnImeiNo", ResourceType = typeof(ExSvc))]
        public string EsnImeiNo { get; set; }
        [Display(Name = "OutModel", ResourceType = typeof(ExSvc))]
        public string OutModel { get; set; }
        [Display(Name = "ReceiptDate", ResourceType = typeof(ExSvc))]
        public Nullable<System.DateTime> ReceiptDate { get; set; }
        [Display(Name = "TransferSendDate", ResourceType = typeof(ExSvc))]
        public Nullable<System.DateTime> TransferSendDate { get; set; }
        [Display(Name = "TransferReceiptDate", ResourceType = typeof(ExSvc))]
        public Nullable<System.DateTime> TransferReceiptDate { get; set; }
        [Display(Name = "FirstPromiseDate", ResourceType = typeof(ExSvc))]
        public Nullable<System.DateTime> FirstPromiseDate { get; set; }
        [Display(Name = "Schedule", ResourceType = typeof(ExSvc))]
        public string Schedule { get; set; }
        [Display(Name = "PromiseDate", ResourceType = typeof(ExSvc))]
        public Nullable<System.DateTime> PromiseDate { get; set; }
        [Display(Name = "Schedule1", ResourceType = typeof(ExSvc))]
        public Nullable<System.DateTime> Schedule1 { get; set; }
        [Display(Name = "DelayFromPromiseDate", ResourceType = typeof(ExSvc))]
        public Nullable<int> DelayFromPromiseDate { get; set; }
        [Display(Name = "DelayFromReceiptDate", ResourceType = typeof(ExSvc))]
        public Nullable<int> DelayFromReceiptDate { get; set; }
        [Display(Name = "TransferApprovalDate", ResourceType = typeof(ExSvc))]
        public Nullable<System.DateTime> TransferApprovalDate { get; set; }

    }
    [MetadataType(typeof(ExServiceRequestMetaData))]
    public partial class ExServiceRequest
    {
    }
    #endregion

    #region Reports
    public class ReportMetadata
    {
        [Display(Name = "Report_Id", ResourceType = typeof(AppViews))]
        public string Id { get; set; }
        [Display(Name = "Report_Name", ResourceType = typeof(AppViews))]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Report_TName", ResourceType = typeof(AppViews))]
        [Required]
        public string TemplateName { get; set; }
        [Display(Name = "Report_Title", ResourceType = typeof(AppViews))]
        public string Title { get; set; }
        [Display(Name = "Report_SubTitle", ResourceType = typeof(AppViews))]
        public string SubTitle { get; set; }
        [Display(Name = "Report_FilterByDate", ResourceType = typeof(AppViews))]
        public bool FilterByDate { get; set; }
        [Display(Name = "Customer", ResourceType = typeof(AppViews))]
        public bool FilterByCustomer { get; set; }
        [Display(Name = "Department", ResourceType = typeof(AppViews))]
        public bool FilterByDepartment { get; set; }
        [Display(Name = "Engineer", ResourceType = typeof(AppViews))]
        public bool FilterByEngineer { get; set; }
        [Display(Name = "Product", ResourceType = typeof(AppViews))]
        public bool FilterByProduct { get; set; }
        [Display(Name = "SR_StatusId", ResourceType = typeof(AppViews))]
        public bool FilterByStatus { get; set; }
        [Display(Name = "City", ResourceType = typeof(AppViews))]
        public bool FilterByCity { get; set; }
        [Display(Name = "Region", ResourceType = typeof(AppViews))]
        public bool FilterByRegion { get; set; }
        [Display(Name = "Tag", ResourceType = typeof(AppViews))]
        public bool FilterByTag { get; set; }

    }
    [MetadataType(typeof(ReportMetadata))]
    public partial class Report
    {
    }
    #endregion
}
