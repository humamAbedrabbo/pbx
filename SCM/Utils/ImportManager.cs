using SCM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Globalization;

namespace SCM.Utils
{
    public class ImportManager
    {
        public int Progress { get; set; }
        public string Phase { get; set; }
        int total = 1;
        int counter = 0;

        public void Import(IEnumerable<ServiceRequestRecord> data)
        {
            int i = 1;
            Progress = 0;


            foreach(var item in data)
            {
                item.SysID = i++;
            }
            
            using (SCMContext ctx = new SCMContext())
            {
                using (var tx = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        Phase = AppViews.DataImport_Phase1;
                        ImportProducts(ctx, ref data);
                        Phase = AppViews.DataImport_Phase2;
                        ImportCities(ctx, ref data);
                        Phase = AppViews.DataImport_Phase3;
                        ImportCancelReasons(ctx, ref data);
                        Phase = AppViews.DataImport_Phase4;
                        ImportPendingReasons(ctx, ref data);
                        Phase = AppViews.DataImport_Phase5;
                        ImportEngineers(ctx, ref data);
                        Phase = AppViews.DataImport_Phase6;
                        //ImportCustomers(ctx, ref data);
                        Phase = AppViews.DataImport_Phase7;
                        //ImportRequests(ctx, ref data);
                        ImportCustomersAndRequests(ctx, ref data);
                        Phase = "done";
                        tx.Commit();
                    }
                    catch(Exception ex)
                    {
                        ex.Data["data"] = data;
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        private void ResetProgress()
        {
            total = 1;
            Progress = 0;
            counter = 0;
        }

        private void UpdateProgress()
        {
            counter++;
            if (total > 0 && counter <= total)
            {
                Progress = (int)(((double)counter / total) * 100.0);
            }
            else
                Progress = 100;
        }
        
        private void ImportProducts(SCMContext ctx, ref IEnumerable<ServiceRequestRecord> data)
        {
            ResetProgress();            

            var curList = ctx.Products.Select(x => x.Id.ToUpper()).ToList();
            var dic = new Dictionary<string, Product>();
            var objects = data.Select(x => new { Id = x.Service_Product_Code, Name = x.SVC_Product.Trim(), IsActive = true }).Distinct().Where(x => !string.IsNullOrEmpty(x.Id)).ToList();
            var objectsToAdd = objects.Where(x => !curList.Contains(x.Id.ToUpper()));

            total = objectsToAdd.Count();
            foreach (var item in objectsToAdd)
            {
                var rec = new Product() { Id = item.Id, Name = item.Name.Trim(), IsActive = item.IsActive };
                ctx.Products.Add(rec);
                ctx.SaveChanges();
                if(!dic.ContainsKey(rec.Id.ToUpper()))
                {
                    dic.Add(rec.Id.ToUpper(), rec);
                }
                UpdateProgress();
            }
            foreach (var item in ctx.Products)
            {
                if(!dic.ContainsKey(item.Id.ToUpper()))
                {
                    dic.Add(item.Id.ToUpper(), item);
                }
            }
            foreach(var item in data)
            {
                item.ProductId = item.Service_Product_Code.ToUpper();
            }
            Progress = 100;
        }

        private void ImportCities(SCMContext ctx, ref IEnumerable<ServiceRequestRecord> data)
        {
            ResetProgress();

            var curList = ctx.Cities.Select(x => x.Name.ToUpper()).ToList();
            var dic = new Dictionary<string, City>();
            var objects = data.Select(x => new { Name = x.City_Name }).Distinct().Where(x => !string.IsNullOrEmpty(x.Name)).ToList();
            var objectsToAdd = objects.Where(x => !curList.Contains(x.Name.ToUpper()));

            total = objectsToAdd.Count();
            foreach (var item in objectsToAdd)
            {
                var rec = new City() { Name = item.Name.ToUpper() };
                ctx.Cities.Add(rec);
                ctx.SaveChanges();
                if (!dic.ContainsKey(rec.Name.ToUpper()))
                {
                    dic.Add(rec.Name.ToUpper(), rec);
                }

                UpdateProgress();
            }
            foreach (var item in ctx.Cities)
            {
                if (!dic.ContainsKey(item.Name.ToUpper()))
                {
                    dic.Add(item.Name.ToUpper(), item);
                }
            }
            foreach (var item in data)
            {
                if (!string.IsNullOrEmpty(item.City_Name))
                {
                    item.CityId = dic[item.City_Name.ToUpper()].Id;
                }
                else
                {
                    item.CityId = null;
                }
            }

            Progress = 100;
        }

        private void ImportPendingReasons(SCMContext ctx, ref IEnumerable<ServiceRequestRecord> data)
        {
            ResetProgress();

            var curList = ctx.PendingReasons.Select(x => x.Reason).ToList();
            var dic = new Dictionary<string, PendingReason>();
            var objects = data.Select(x => new { Reason = x.Pending_Reason }).Distinct().Where(x => !string.IsNullOrEmpty(x.Reason)).ToList();

            var objectsToAdd = objects.Where(x => !curList.Contains(x.Reason));
            total = objectsToAdd.Count();
            foreach (var item in objectsToAdd)
            {
                var rec = new PendingReason() { Reason = item.Reason };
                ctx.PendingReasons.Add(rec);
                ctx.SaveChanges();
                if (!dic.ContainsKey(rec.Reason.ToUpper()))
                {
                    dic.Add(rec.Reason.ToUpper(), rec);
                }

                UpdateProgress();
            }
            foreach (var item in ctx.PendingReasons)
            {
                if (!dic.ContainsKey(item.Reason.ToUpper()))
                {
                    dic.Add(item.Reason.ToUpper(), item);
                }
            }
            foreach (var item in data)
            {
                if (!string.IsNullOrEmpty(item.Pending_Reason))
                {
                    item.PendingReasonId = dic[item.Pending_Reason.ToUpper()].Id;
                }
                else
                {
                    item.PendingReasonId = null;
                }
            }

            Progress = 100;
        }

        private void ImportCancelReasons(SCMContext ctx, ref IEnumerable<ServiceRequestRecord> data)
        {
            ResetProgress();

            var curList = ctx.CancelReasons.Select(x => x.Reason).ToList();
            var dic = new Dictionary<string, CancelReason>();
            var objects = data.Select(x => new { Reason = x.Cancel_Reason }).Distinct().Where(x => !string.IsNullOrEmpty(x.Reason)).ToList();

            var objectToAdd = objects.Where(x => !curList.Contains(x.Reason));
            total = objectToAdd.Count();
            foreach (var item in objectToAdd)
            {
                var rec = new CancelReason() { Reason = item.Reason };
                ctx.CancelReasons.Add(rec);
                ctx.SaveChanges();
                if (!dic.ContainsKey(rec.Reason.ToUpper()))
                {
                    dic.Add(rec.Reason.ToUpper(), rec);
                }

                UpdateProgress();
            }
            foreach (var item in ctx.CancelReasons)
            {
                if (!dic.ContainsKey(item.Reason.ToUpper()))
                {
                    dic.Add(item.Reason.ToUpper(), item);
                }
            }
            foreach (var item in data)
            {
                if (!string.IsNullOrEmpty(item.Cancel_Reason))
                {
                    item.CancelReasonId = dic[item.Cancel_Reason.ToUpper()].Id;
                }
                else
                {
                    item.CancelReasonId = null;
                }
            }

            Progress = 100;
        }

        private void ImportCustomersAndRequests(SCMContext ctx, ref IEnumerable<ServiceRequestRecord> data)
        {
            ResetProgress();
            string dateFormat = "dd/MM/yyyy";

            try
            {
                // Build db customers dictionary
                var dicCustomers = new SortedDictionary<string, int>();
                var dbCustomers = ctx.Customers.OrderBy(x => x.Name).Select(x => new {
                    Id = x.Id,
                    Name = x.Name.Trim()
                    ,
                    Phone = string.IsNullOrEmpty(x.Phone) ? "X" : x.Phone.Trim()
                    ,
                    Mobile = string.IsNullOrEmpty(x.Mobile) ? "X" : x.Mobile.Trim()
                }).Distinct().ToList();
                foreach (var cus in dbCustomers)
                {
                    if (cus.Phone != "X")
                        dicCustomers[cus.Phone + "_" + cus.Name] = cus.Id;
                    if (cus.Mobile != "X")
                        dicCustomers[cus.Mobile + "_" + cus.Name] = cus.Id;
                }

                // Build db RQN dictionary
                // var dicRQN = ctx.ServiceRequests.Select(x => new { Id = x.Id, RQN = x.RQN }).OrderBy(x => x.RQN).ToDictionary(x => x.RQN, y => y.Id);
                var dicRQN = ctx.ServiceRequests.Where(x => !string.IsNullOrEmpty(x.RQN)).Select(x => new { Id = x.Id, RQN = x.RQN }).OrderBy(x => x.RQN).ToDictionary(x => x.Id, y => y.RQN);
                var dicByPhone = ctx.ServiceRequests.Where(x => x.StatusId < 90 && !string.IsNullOrEmpty(x.RQN) && x.ProductId != null && !string.IsNullOrEmpty(x.Customer.Phone)).Select(x => new { Key = x.Customer.Phone + "_" + x.ProductId, Id = x.Id }).ToDictionary(x => x.Id, y => y.Key);
                var dicByMobile = ctx.ServiceRequests.Where(x => x.StatusId < 90 && !string.IsNullOrEmpty(x.RQN) && x.ProductId != null && !string.IsNullOrEmpty(x.Customer.Mobile)).Select(x => new { Key = x.Customer.Mobile + "_" + x.ProductId, Id = x.Id }).ToDictionary(x => x.Id, y => y.Key);

                // Build status, engineers and tags lists
                var dic = new Dictionary<string, int>();
                dic.Add("Repair Accepted", 10);
                dic.Add("Repair Pending", 20);
                dic.Add("Repair Canceled", 90);
                dic.Add("Repair Completed", 100);
                var ctags = ctx.Tags.Where(x => x.TagType == "C").ToList();
                var rtags = ctx.Tags.Where(x => x.TagType == "R").ToList();
                // var engineers = ctx.Engineers.OrderBy(x => x.Name).ToDictionary(x => x.Name, y => y.Id);

                // Set progress total
                total = data.Count();

                // Loop through data
                int batchCounter = 0;
                StringBuilder sb = new StringBuilder();
                //sb.AppendLine("declare @sid int;");
                //sb.AppendLine("declare @cid int;");

                foreach (var sr in data)
                {
                    if (batchCounter >= 9)
                    {
                        //System.IO.File.AppendAllText("e:/sql.sql", sb.ToString());
                        DbCommand cmd = ctx.Database.Connection.CreateCommand();
                        cmd.CommandText = "declare @sid int;\r\n" + "declare @cid int;\r\n" + sb.ToString();
                        cmd.CommandTimeout = 100;
                        cmd.Transaction = ctx.Database.CurrentTransaction.UnderlyingTransaction;
                        cmd.ExecuteNonQuery();

                        batchCounter = 0;
                        sb.Clear();
                        //sb.AppendLine("declare @sid int;");
                        //sb.AppendLine("declare @cid int;");
                        sb.AppendLine("set @sid = 0;");
                        sb.AppendLine("set @cid = 0;");
                    }
                    else
                    {
                        batchCounter++;
                        sb.AppendLine("set @sid = 0;");
                        sb.AppendLine("set @cid = 0;");
                    }

                    var srByPhone = string.Format("{0}_{1}", sr.Customer_Phone_No ?? "X", sr.Service_Product_Code ?? "X");
                    var srByMobile = string.Format("{0}_{1}", sr.Cellular_No ?? "X", sr.Service_Product_Code ?? "X");
                    bool isRqnRegistered = dicRQN.ContainsValue(sr.Receipt_No);
                    bool isPhoneRegistered = false;
                    bool isMobileRegistered = false;
                    if(!isRqnRegistered)
                    {
                        isPhoneRegistered = dicByPhone.Values.Contains(srByPhone);
                        if (!isPhoneRegistered)
                            isMobileRegistered = dicByMobile.Values.Contains(srByMobile);
                    }

                    if (isRqnRegistered || isPhoneRegistered || isMobileRegistered)
                    {
                        // the request exist
                        string sql = @"UPDATE ServiceRequests SET [EngineerId] = {0}, [StatusId] = {1}, [StatusDate] = (case when [RequestDate] > getdate() then [RequestDate] else getdate() end), [ProductId] = {2}, [Model] = {3}, [SN] = {4}, [ClosingDate] = {5}, [Description] = {6}, UpdatedOn = getdate(), UpdatedBy = '{7}' Where {8}; ";
                        var engId = (sr.EngineerId.HasValue && sr.EngineerId.Value > 0) ? sr.EngineerId.ToString() : "NULL";
                        var model = string.IsNullOrEmpty(sr.Model) ? "NULL" : string.Format("N'{0}'", sr.Model.Replace("'", "''"));
                        var rqn = sr.Receipt_No;
                        var sn = string.IsNullOrEmpty(sr.Serial_No) ? "NULL" : string.Format("N'{0}'", sr.Serial_No.Replace("'", "''"));
                        var productId = (!string.IsNullOrEmpty(sr.ProductId)) ? "'" + sr.ProductId + "'" : "NULL";

                        DateTime? closingDate = null;
                        if (!string.IsNullOrEmpty(sr.Completion_Date))
                        {
                            DateTime dclosing;
                            if (DateTime.TryParseExact(sr.Completion_Date, dateFormat, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dclosing))
                                closingDate = dclosing;
                        }
                        var description = string.IsNullOrEmpty(sr.CIC_Remark) ? "NULL" : string.Format("N'{0}'", sr.CIC_Remark.Replace("'", "''"));
                        var userName = HttpContext.Current.User.Identity.Name;
                        string ws = "";
                        if (isRqnRegistered)
                        {

                            ws = "[RQN] = '" + rqn + "'";
                        }
                        else if (isPhoneRegistered)
                        {
                            var idByPhone = dicByPhone.First(x => x.Value == srByPhone).Key;
                            ws = "[Id] = " + idByPhone.ToString();
                        }
                        else if (isMobileRegistered)
                        {
                            var idByMobile = dicByMobile.First(x => x.Value == srByMobile).Key;
                            ws = "[Id] = " + idByMobile.ToString();

                        }
                        var valsql = string.Format(sql, engId, dic[sr.Status], productId, model, sn, !closingDate.HasValue ? "NULL" : "'" + closingDate.Value.ToString("dd/MMM/yyyy HH:mm:ss") + "'", description, userName, ws);
                        sb.AppendLine(valsql);

                    }
                    else
                    {
                        // New request

                        // Check Customer
                        string phoneKey = string.IsNullOrEmpty(sr.Customer_Phone_No) || string.IsNullOrWhiteSpace(sr.Customer_Phone_No) ? "X" : sr.Customer_Phone_No.Trim();
                        string mobKey = string.IsNullOrEmpty(sr.Cellular_No) || string.IsNullOrWhiteSpace(sr.Cellular_No) ? "X" : sr.Cellular_No.Trim();
                        string key1 = phoneKey + "_" + sr.Customer_Name.Trim();
                        string key2 = mobKey + "_" + sr.Customer_Name.Trim();
                        string customerId = "@cid";
                        if (dicCustomers.ContainsKey(key1))
                            customerId = dicCustomers[key1].ToString();
                        else if (dicCustomers.ContainsKey(key2))
                            customerId = dicCustomers[key2].ToString();
                        else
                        {
                            // Insert new customer

                            string csql = @"set @cid = IsNull((select Top 1 Id from Customers Where Ltrim(rtrim([Name])) = '{0}' AND  ([Phone] = {1} OR [Mobile] = {2}) ) , 0); IF(@cid = 0) BEGIN  INSERT INTO Customers (Name, Phone, Mobile, CityId, Address) VALUES (N'{0}', {1}, {2}, {3}, {4});  set @cid = @@identity;   END;";
                            var cPhone = (phoneKey == "X") ? "NULL" : string.Format("N'{0}'", phoneKey.Replace("'", "''"));
                            var cMobile = (mobKey == "X") ? "NULL" : string.Format("N'{0}'", mobKey.Replace("'", "''"));
                            if (cPhone == "NULL" && cMobile == "NULL")
                                cPhone = "011XXXX";
                            var cCity = sr.CityId.HasValue ? sr.CityId.Value.ToString() : "NULL";
                            var cAddress = string.IsNullOrEmpty(sr.Address) ? "NULL" : string.Format("N'{0}'", sr.Address.Replace("'", "''"));
                            string cValSql = string.Format(csql, sr.Customer_Name.Trim(), cPhone, cMobile, cCity, cAddress);
                            sb.AppendLine(cValSql);
                            //sb.AppendLine("set @cid = @@identity;");
                        }

                        string sql = @"INSERT INTO ServiceRequests ([CenterId], [CustomerId], [DepartmentId], [RequestDate], [EngineerId], [RQN], [StatusId], [StatusDate], [Model], [SN], [ClosingDate], [Description], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [IsDeleted], [ProductId]) VALUES (1,{0}, 1, '{1}', {2}, '{3}', {4}, '{1}', {5}, {6}, {7}, {8}, getdate(), '{9}', getdate(), '{9}', 0, {10} );";

                        DateTime requestDate = DateTime.Now;
                        if (!string.IsNullOrEmpty(sr.Request_Date))
                        {
                            DateTime drequest;
                            if (DateTime.TryParseExact(sr.Request_Date, dateFormat, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out drequest))
                                requestDate = drequest;
                        }
                        var engId = (sr.EngineerId.HasValue && sr.EngineerId.Value > 0) ? sr.EngineerId.ToString() : "NULL";
                        var model = string.IsNullOrEmpty(sr.Model) ? "NULL" : string.Format("N'{0}'", sr.Model.Replace("'", "''"));
                        var sn = string.IsNullOrEmpty(sr.Serial_No) ? "NULL" : string.Format("N'{0}'", sr.Serial_No.Replace("'", "''"));
                        var productId = (!string.IsNullOrEmpty(sr.ProductId)) ? "'" + sr.ProductId + "'" : "NULL";
                        DateTime? closingDate = null;
                        if (!string.IsNullOrEmpty(sr.Completion_Date))
                        {
                            DateTime dclosing;
                            if (DateTime.TryParseExact(sr.Completion_Date, dateFormat, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dclosing))
                                closingDate = dclosing;
                        }
                        var description = string.IsNullOrEmpty(sr.CIC_Remark) ? "NULL" : string.Format("N'{0}'", sr.CIC_Remark.Replace("'", "''"));
                        var userName = HttpContext.Current.User.Identity.Name;

                        string valsql = string.Format(sql, customerId, requestDate.ToString("dd/MMM/yyyy HH:mm:ss"), engId
                            , sr.Receipt_No, dic[sr.Status], model, sn, !closingDate.HasValue ? "NULL" : "'" + closingDate.Value.ToString("dd/MMM/yyyy HH:mm:ss") + "'", description, userName, productId);
                        sb.AppendLine(valsql);
                        sb.AppendLine("set @sid = @@identity;");
                        if (!string.IsNullOrEmpty(sr.Svc_Type) || !string.IsNullOrEmpty(sr.Warranty_Flag))
                        {
                            var t = rtags.FirstOrDefault(x => x.Name == sr.Svc_Type);
                            if (t != null)
                            {
                                sb.AppendFormat("INSERT INTO RequestTags (TagId, ServiceRequestId) VALUES ({0}, @sid);", t.Id);
                                sb.AppendLine();
                            }
                            var t1 = rtags.FirstOrDefault(x => x.Name == sr.Warranty_Flag);
                            if (t1 != null)
                            {
                                sb.AppendFormat("INSERT INTO RequestTags (TagId, ServiceRequestId) VALUES ({0}, @sid);", t1.Id);
                                sb.AppendLine();
                            }
                        }

                    }

                    UpdateProgress();

                }
                if (batchCounter > 0)
                {
                    //System.IO.File.AppendAllText("e:/sql.sql", sb.ToString());
                    DbCommand cmd = ctx.Database.Connection.CreateCommand();
                    cmd.CommandText = "declare @sid int;\r\n" + "declare @cid int;\r\n" + sb.ToString();
                    cmd.CommandTimeout = 100;
                    cmd.Transaction = ctx.Database.CurrentTransaction.UnderlyingTransaction;
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                
                throw;
            }

            //
            Progress = 100;
        }

        private void ImportCustomers(SCMContext ctx, ref IEnumerable<ServiceRequestRecord> data)
        {
            ResetProgress();

            var curList = ctx.Customers.Select(x => x.Name).ToList();
            var dic = new Dictionary<string, Customer>();
            var tags = ctx.Tags.Where(x => x.TagType == "C").ToDictionary(x=> x.Name, y => y);
            var objects = data.Select(x => new { Name = x.Customer_Name, Phone = x.Customer_Phone_No, Mobile = x.Cellular_No, City = x.CityId, Address = x.Address, CustomerType = x.Customer_Type }).Distinct().Where(x => !string.IsNullOrEmpty(x.Name)).ToList();

            var objectsToAdd = objects.Where(x => !curList.Contains(x.Name));
            total = objectsToAdd.Count();
            foreach (var item in objectsToAdd)
            {
                var rec = new Customer() { Name = item.Name, Phone = item.Phone, Mobile = item.Mobile, CityId = item.City, Address = item.Address  };
                if (string.IsNullOrEmpty(rec.Phone) && string.IsNullOrEmpty(rec.Mobile))
                    rec.Phone = "011XXXX";
                ctx.Customers.Add(rec);
                ctx.SaveChanges();
                if (!dic.ContainsKey(rec.Name.ToUpper()))
                {
                    dic.Add(rec.Name.ToUpper(), rec);
                }

                // Add Customer Tags
                string t1 = item.CustomerType;
                if(t1 == "General Enduser")
                        t1 = "General End User";
                Tag t = null;
                if(!tags.ContainsKey(t1))
                {
                    t = new Tag() { Name = t1, TagType = "C", Format = "label-default" };
                    ctx.Tags.Add(t);
                    ctx.SaveChanges();
                    tags.Add(t.Name, t);
                }
                else
                {
                    t = tags[t1];
                }
                if(t != null)
                {
                    rec.Tags.Add(t);
                    ctx.SaveChanges();
                }

                UpdateProgress();
            }
            foreach (var item in ctx.Customers)
            {
                if (!dic.ContainsKey(item.Name.ToUpper()))
                {
                    dic.Add(item.Name.ToUpper(), item);
                }
            }
            ResetProgress();
            total = data.Count();
            foreach (var item in data)
            {
                if (!string.IsNullOrEmpty(item.Customer_Name) && dic.ContainsKey(item.Customer_Name))
                {
                    item.CustomerId = dic[item.Customer_Name.ToUpper()].Id;
                }
                UpdateProgress();
            }

            Progress = 100;
        }

        private void ImportEngineers(SCMContext ctx, ref IEnumerable<ServiceRequestRecord> data)
        {
            ResetProgress();

            var curList = ctx.Engineers.Select(x => x.Name).ToList();
            var dic = new Dictionary<string, Engineer>();
            var objects = data.Select(x => new { Name = x.SVC_Engineer_Name }).Distinct().Where(x => !string.IsNullOrEmpty(x.Name)).ToList();
            var objectsToAdd = objects.Where(x => !curList.Contains(x.Name));
            total = objectsToAdd.Count();

            foreach (var item in objectsToAdd)
            {
                var rec = new Engineer() { Name = item.Name, DepartmentId = 1, IsActive = true };
                ctx.Engineers.Add(rec);
                ctx.SaveChanges();
                if (!dic.ContainsKey(rec.Name.ToUpper()))
                {
                    dic.Add(rec.Name.ToUpper(), rec);
                }

                UpdateProgress();
            }
            foreach (var item in ctx.Engineers)
            {
                if (!dic.ContainsKey(item.Name.ToUpper()))
                {
                    dic.Add(item.Name.ToUpper(), item);
                }
            }
            foreach (var item in data)
            {
                if (!string.IsNullOrEmpty(item.SVC_Engineer_Name))
                {
                    item.EngineerId = dic[item.SVC_Engineer_Name.ToUpper()].Id;
                }
                else
                {
                    item.EngineerId = null;
                }
            }

            Progress = 100;
        }

        private void ImportRequests(SCMContext ctx, ref IEnumerable<ServiceRequestRecord> data)
        {
            ResetProgress();

            var list = data.Where(x => !string.IsNullOrEmpty(x.Receipt_No)).ToDictionary(x => x.Receipt_No, y => y.SysID);
            var dic = new Dictionary<string,int>();
            dic.Add("Repair Accepted",10);
            dic.Add("Repair Pending",20);
            dic.Add("Repair Canceled",90);
            dic.Add("Repair Completed",100);
            var tags = ctx.Tags.Where(x => x.TagType == "R").ToList();

            var rqns = list.Keys.ToArray();
            var ids = new List<int>();

            var objectsToUpdate = ctx.ServiceRequests.Where(x => !string.IsNullOrEmpty(x.RQN) && rqns.Contains(x.RQN));
            total = objectsToUpdate.Count();
            StringBuilder sb1 = new StringBuilder();
            int counter = 0;
            foreach (var item in objectsToUpdate)
            {
                if(counter >= 9)
                {
                    DbCommand cmd1 = ctx.Database.Connection.CreateCommand();
                    cmd1.CommandText = sb1.ToString();
                    
                    cmd1.Transaction = ctx.Database.CurrentTransaction.UnderlyingTransaction;
                    cmd1.ExecuteNonQuery();
                    sb1 = new StringBuilder();
                    counter = 0;

                }
                else
                {
                    counter++;
                }
                var sr = data.First(x => x.SysID == list[item.RQN]);

                string sql = @"UPDATE ServiceRequests SET [EngineerId] = {0}, [StatusId] = {1}, [StatusDate] = (case when [RequestDate] > getdate() then [RequestDate] else getdate() end), [ProductId] = {2}, [Model] = {3}, [SN] = {4}, [ClosingDate] = {5}, [Description] = {6}, UpdatedOn = getdate(), UpdatedBy = '{7}' Where [RQN] = '{8}'; ";
                var engId = (sr.EngineerId.HasValue && sr.EngineerId.Value > 0) ? sr.EngineerId.ToString() : "NULL";
                var model = string.IsNullOrEmpty(sr.Model) ? "NULL" : string.Format("N'{0}'", sr.Model.Replace("'", "''"));
                var rqn = item.RQN;
                var sn = string.IsNullOrEmpty(sr.Serial_No) ? "NULL" : string.Format("N'{0}'", sr.Serial_No.Replace("'", "''"));
                DateTime? closingDate = null;
                if (!string.IsNullOrEmpty(sr.Completion_Date))
                {
                    closingDate = Convert.ToDateTime(sr.Completion_Date);
                }
                var description = string.IsNullOrEmpty(sr.CIC_Remark) ? "NULL" : string.Format("N'{0}'", sr.CIC_Remark.Replace("'", "''"));
                var userName = HttpContext.Current.User.Identity.Name;
                if (!string.IsNullOrEmpty(sr.ProductId))
                    item.ProductId = "'" + sr.ProductId + "'";
                else
                    item.ProductId = "NULL";

                var valsql = string.Format(sql, engId, dic[sr.Status], item.ProductId, model, sn, !closingDate.HasValue ? "NULL" : "'" + closingDate.Value.ToString("dd/MMM/yyyy HH:mm:ss") + "'", description, userName, rqn);
                sb1.AppendLine(valsql);

                ids.Add(sr.SysID);


                UpdateProgress();

            }
            if (counter > 0)
            {
                DbCommand cmd1 = ctx.Database.Connection.CreateCommand();
                cmd1.CommandText = sb1.ToString();
                cmd1.CommandTimeout = 200;
                cmd1.Transaction = ctx.Database.CurrentTransaction.UnderlyingTransaction;
                cmd1.ExecuteNonQuery();
                counter = 0;
            }

            ResetProgress();
            var objectsToAdd = data.Where(x => !ids.Contains(x.SysID));
            total = objectsToAdd.Count();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("declare @id int;");
            try
            {
                counter = 0;
                foreach (var sr in objectsToAdd)
                {
                    if(counter >= 9)
                    {
                        DbCommand cmd = ctx.Database.Connection.CreateCommand();
                        cmd.CommandText = sb.ToString();
                        cmd.Transaction = ctx.Database.CurrentTransaction.UnderlyingTransaction;
                        cmd.ExecuteNonQuery();
                        sb = new StringBuilder();
                        sb.AppendLine("declare @id int;");
                        counter = 0;
                    }
                    else
                    {
                        counter++;
                    }
                    string sql = @"INSERT INTO ServiceRequests ([CenterId], [CustomerId], [DepartmentId], [RequestDate], [EngineerId], [RQN], [StatusId], [StatusDate], [Model], [SN], [ClosingDate], [Description], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [IsDeleted]) VALUES (1,{0}, 1, '{1}', {2}, '{3}', {4}, '{1}', {5}, {6}, {7}, {8}, getdate(), '{9}', getdate(), '{9}', 0 );";

                    var requestDate = !string.IsNullOrEmpty(sr.Request_Date) ? Convert.ToDateTime(sr.Request_Date) : DateTime.Now;
                    var engId = (sr.EngineerId.HasValue && sr.EngineerId.Value > 0) ? sr.EngineerId.ToString() : "NULL";
                    var model = string.IsNullOrEmpty(sr.Model) ? "NULL" : string.Format("N'{0}'", sr.Model.Replace("'","''"));
                    var sn = string.IsNullOrEmpty(sr.Serial_No) ? "NULL" : string.Format("N'{0}'", sr.Serial_No.Replace("'","''"));
                    DateTime? closingDate = null;
                    if (!string.IsNullOrEmpty(sr.Completion_Date))
                    {
                        closingDate = Convert.ToDateTime(sr.Completion_Date);
                    }
                    var description = string.IsNullOrEmpty(sr.CIC_Remark) ? "NULL" : string.Format("N'{0}'", sr.CIC_Remark.Replace("'","''"));
                    var userName = HttpContext.Current.User.Identity.Name;

                    string valsql = string.Format(sql, sr.CustomerId, requestDate.ToString("dd/MMM/yyyy HH:mm:ss"), engId
                        , sr.Receipt_No, dic[sr.Status], model, sn, !closingDate.HasValue ? "NULL" : "'" + closingDate.Value.ToString("dd/MMM/yyyy HH:mm:ss") + "'", description, userName);
                    sb.AppendLine(valsql);
                    sb.AppendLine("set @id = @@identity;");
                    if (!string.IsNullOrEmpty(sr.Svc_Type) || !string.IsNullOrEmpty(sr.Warranty_Flag))
                    {
                        var t = tags.FirstOrDefault(x => x.Name == sr.Svc_Type);
                        if (t != null)
                        {
                            sb.AppendFormat("INSERT INTO RequestTags (TagId, ServiceRequestId) VALUES ({0}, @id);", t.Id);
                            sb.AppendLine();
                        }
                        var t1 = tags.FirstOrDefault(x => x.Name == sr.Warranty_Flag);
                        if (t1 != null)
                        {
                            sb.AppendFormat("INSERT INTO RequestTags (TagId, ServiceRequestId) VALUES ({0}, @id);", t1.Id);
                            sb.AppendLine();
                        }
                    }


                    System.Diagnostics.Debug.Print(sr.SysID.ToString());
                    UpdateProgress();
                }
                if(counter > 0)
                {
                    DbCommand cmd = ctx.Database.Connection.CreateCommand();
                    cmd.CommandText = sb.ToString();
                    cmd.Transaction = ctx.Database.CurrentTransaction.UnderlyingTransaction;
                    cmd.ExecuteNonQuery();
                }
            }
        catch (Exception ex)
        {
            //ex.Data["item"] = item;
            //ex.Data["sr"] = sr;
            throw ex;
        }
    //ctx.SaveChanges();

    Progress = 100;
        }
    }
}