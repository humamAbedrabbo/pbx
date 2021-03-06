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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SCMContext : DbContext
    {
        public SCMContext()
            : base("name=SCMContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CallArchive> CallArchives { get; set; }
        public virtual DbSet<CancelReason> CancelReasons { get; set; }
        public virtual DbSet<Center> Centers { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Engineer> Engineers { get; set; }
        public virtual DbSet<ExServiceRequest> ExServiceRequests { get; set; }
        public virtual DbSet<PendingReason> PendingReasons { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<QIN> QINs { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<ServiceRequest> ServiceRequests { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
    
        public virtual ObjectResult<q_take_call_Result> q_take_call(string user, string userext)
        {
            var userParameter = user != null ?
                new ObjectParameter("user", user) :
                new ObjectParameter("user", typeof(string));
    
            var userextParameter = userext != null ?
                new ObjectParameter("userext", userext) :
                new ObjectParameter("userext", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<q_take_call_Result>("q_take_call", userParameter, userextParameter);
        }
    }
}
