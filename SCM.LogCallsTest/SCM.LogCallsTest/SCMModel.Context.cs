//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SCM.LogCallsTest
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
    
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<QIN> QINs { get; set; }
        public virtual DbSet<ServiceRequest> ServiceRequests { get; set; }
    
        public virtual int q_log_call(string tdate, string ttime, string textcode, string tlineno, string theader, string tno, string tdur)
        {
            var tdateParameter = tdate != null ?
                new ObjectParameter("tdate", tdate) :
                new ObjectParameter("tdate", typeof(string));
    
            var ttimeParameter = ttime != null ?
                new ObjectParameter("ttime", ttime) :
                new ObjectParameter("ttime", typeof(string));
    
            var textcodeParameter = textcode != null ?
                new ObjectParameter("textcode", textcode) :
                new ObjectParameter("textcode", typeof(string));
    
            var tlinenoParameter = tlineno != null ?
                new ObjectParameter("tlineno", tlineno) :
                new ObjectParameter("tlineno", typeof(string));
    
            var theaderParameter = theader != null ?
                new ObjectParameter("theader", theader) :
                new ObjectParameter("theader", typeof(string));
    
            var tnoParameter = tno != null ?
                new ObjectParameter("tno", tno) :
                new ObjectParameter("tno", typeof(string));
    
            var tdurParameter = tdur != null ?
                new ObjectParameter("tdur", tdur) :
                new ObjectParameter("tdur", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("q_log_call", tdateParameter, ttimeParameter, textcodeParameter, tlinenoParameter, theaderParameter, tnoParameter, tdurParameter);
        }
    
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
