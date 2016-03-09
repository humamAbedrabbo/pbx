﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SCM;
using PagedList;
using System.Linq.Dynamic;
using SCM.Utils;

namespace SCM.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private SCMContext db = new SCMContext();

        // GET: Requests
        public ActionResult Index(int? page)
        {
            var requests = DataManager.Requests();

            ViewBag.TotalActive = requests.Count(x => x.StatusId < 90);
            ViewBag.TotalPending = requests.Count(x => x.StatusId == 20);
            ViewBag.TotalDelayed = requests.Count(x => x.StatusId < 90 && x.RequestDate.AddDays(3) < DateTime.Today);
            ViewBag.Status = "active";
            ViewBag.RelatedTags = requests.SelectMany(x => x.Tags.Union(x.Customer.Tags)).OrderBy(y => y.TagType).ThenBy(y => y.Name).GroupBy(x => x.Name).ToDictionary(x => x.Key, y => y.Count());

            int pageSize = 30;
            int pageNumber = (page ?? 1);
            return View(requests.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Requests(int? page = null, string status = "active", string filterDuration = null, string customerName = null, string phone = null, string code = null, string receipt = null, string product = null, string model = null, string sn = null, string tags = null)
        {
            var requests = DataManager.Requests();

            switch (status)
            {
                case "pending":
                    requests = requests.Where(x => x.StatusId == 20);
                    break;
                case "active":
                    break;
                case "delayed":
                    requests = requests.Where(x => x.StatusId < 90 && x.RequestDate.AddDays(3) < DateTime.Today);
                    break;
                default:
                    break;
            }

            requests = (from x in requests
                        where (string.IsNullOrEmpty(customerName) || x.Customer.Name.Contains(customerName))
                        && (string.IsNullOrEmpty(phone) || x.Customer.Phone == phone || x.Customer.Mobile == phone)
                        && (string.IsNullOrEmpty(code) || x.RQN == code)
                        && (string.IsNullOrEmpty(receipt) || x.ReceiptNo == receipt)
                        && (string.IsNullOrEmpty(product) || x.Product != null || x.Product.Name.Contains(product))
                        && (string.IsNullOrEmpty(model) || x.Model == model)
                        && (string.IsNullOrEmpty(sn) || x.SN == sn)
                        select x);

            switch(filterDuration)
            {
                case "today":
                    requests = requests.Where(x => x.RequestDate.Year == DateTime.Today.Year && x.RequestDate.Month == DateTime.Today.Month && x.RequestDate.Day == DateTime.Today.Day);
                    break;
                case "week":
                    int span = 0;
                    switch(DateTime.Today.DayOfWeek)
                    {
                        case DayOfWeek.Sunday:
                            span = 0;
                            break;
                        case DayOfWeek.Monday:
                            span = 1;
                            break;
                        case DayOfWeek.Tuesday:
                            span = 2;
                            break;
                        case DayOfWeek.Wednesday:
                            span = 3;
                            break;
                        case DayOfWeek.Thursday:
                            span = 4;
                            break;
                        case DayOfWeek.Friday:
                            span = 5;
                            break;
                        case DayOfWeek.Saturday:
                            span = 6;
                            break;
                        default:
                            span = 0;
                            break;

                    }
                    requests = requests.Where(x => x.RequestDate >= DateTime.Today.AddDays(-span) && x.RequestDate >= DateTime.Today.AddDays(7- span));
                    break;
                case "month":
                    requests = requests.Where(x => x.RequestDate.Year == DateTime.Today.Year && x.RequestDate.Month == DateTime.Today.Month );
                    break;
                default:
                    break;
            }

            requests = requests.ToList();

            ViewBag.TotalActive = requests.Count(x => x.StatusId < 90);
            ViewBag.TotalPending = requests.Count(x => x.StatusId == 20);
            ViewBag.TotalDelayed = requests.Count(x => x.StatusId < 90 && x.RequestDate.AddDays(3) < DateTime.Today);
            ViewBag.Status = status;
            ViewBag.RelatedTags = requests.SelectMany(x => x.Tags.Union(x.Customer.Tags)).OrderBy(y => y.TagType).ThenBy(y => y.Name).GroupBy(x => x.Name).ToDictionary(x => x.Key, y => y.Count());


            int pageSize = 30;
            int pageNumber = (page ?? 1);
            return PartialView("_Requests", requests.ToPagedList(pageNumber, pageSize));
        }

        public JsonResult TakeCall()
        {
            var result = db.q_take_call("SYSTEM", "102").ToList().FirstOrDefault();
            if(result == null)
            {
                return null;
            }
            //var cust = db.Customers.Find(id);
            //var str = "[{" + string.Format("Id: {0}, Name: \"{1}\"", cust.Id, cust.Name) + "}]";
            var obj = new { Id = result.Id, CallerId = result.TNo};
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index_COPY(int? page = null)
        {
            var model = db.ServiceRequests.Include(x => x.Center).Include(x => x.Customer).Include(x => x.Department).Include(x => x.Product).Include(x => x.Tags).Include(x => x.Customer.Tags).Where(x => !x.IsDeleted && x.StatusId < 90).ToList();
            int pageNumber = page ?? 1;
            int pageSize = 30;
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult List(int? page = null, string name = null, string sortBy = "Customer.Name", string direction = "ASC")
        {
            var model = db.ServiceRequests.Include(x => x.Center).Include(x => x.Customer).Include(x => x.Department).Include(x => x.Product).Include(x => x.Tags).Include(x => x.Customer.Tags).Where(x => !x.IsDeleted && x.StatusId < 90 
                && (string.IsNullOrEmpty(name) || x.Customer.Name.Contains(name) || x.Customer.Phone == name || x.Customer.Mobile == name)).OrderBy(sortBy + " " + direction).ToList();
            int pageNumber = page ?? 1;
            int pageSize = 30;
            return PartialView(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.ServiceRequests.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: AA/Create
        public ActionResult Create(int? customerId = null, int? qin = null)
        {
            ViewBag.CenterId = new SelectList(DataManager.Centers(), "Id", "Name");
            ViewBag.DepartmentId = new SelectList(DataManager.Departments(), "Id", "Name");
            var model = db.ServiceRequests.Create();
            model.RequestDate = DateTime.Now ;
            
            if(customerId.HasValue)
            {
                var customer = db.Customers.Find(customerId);
                if(customer != null)
                {
                    model.Customer = customer;
                    model.CustomerId = customerId.Value;
                }
                
            }
            else if (qin.HasValue)
            {
                var q = db.QINs.Find(qin);
                if (q != null)
                {
                    var qcustomer = db.Customers.FirstOrDefault(x => x.Phone == q.TNo || x.Mobile == q.TNo);
                    if (qcustomer != null)
                    {
                        model.Customer = qcustomer;
                        model.CustomerId = qcustomer.Id;
                    }

                }
            }

            return View(model);
        }

        // POST: AA/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerId,RequestDate,StatusId,StatusDate,CenterId,RQN,ReceiptNo,DepartmentId,ProductId,Model,SN,EngineerId,Description,Remarks,ClosingDate,PendingReasonId,CancelReasonId,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,IsDeleted")] ServiceRequest model)
        {
            //Set default values
            model.CenterId = 1;
            model.StatusId = 10;
            model.StatusDate = DateTime.Now;
            if (string.IsNullOrEmpty(User.Identity.Name))
            {
                model.CreatedBy = "SYSTEM";
                model.UpdatedBy = "SYSTEM";
            }
            else
            {
                model.CreatedBy = User.Identity.Name;
                model.UpdatedBy = User.Identity.Name;
            }
            model.CreatedOn = DateTime.Now;
            model.UpdatedOn = DateTime.Now;
                       

            if (ModelState.IsValid)
            {
                db.ServiceRequests.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CenterId = new SelectList(DataManager.Centers(), "Id", "Name");
            ViewBag.DepartmentId = new SelectList(DataManager.Departments(), "Id", "Name");
            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.ServiceRequests.Find(id);
            
            ViewBag.CenterId = new SelectList(DataManager.Centers(), "Id", "Name", model.CenterId);
            ViewBag.DepartmentId = new SelectList(DataManager.Departments(), "Id", "Name", model.DepartmentId);
            ViewBag.ProductId = new SelectList(DataManager.Products().Where(x => model.DepartmentId == null || x.DepartmentId == model.DepartmentId), "Id", "Name", model.ProductId);
            ViewBag.EngineerId = new SelectList(DataManager.Engineers().Where(x => model.DepartmentId == null || x.DepartmentId == model.DepartmentId), "Id", "Name", model.EngineerId);
            ViewBag.PendingReasonId = new SelectList(db.PendingReasons, "Id", "Reason", model.PendingReasonId);
            ViewBag.CancelReasonId = new SelectList(db.CancelReasons, "Id", "Reason", model.CancelReasonId);
            ViewBag.StatusId = Utils.ListManager.GetStatus();

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerId,RequestDate,StatusId,StatusDate,CenterId,RQN,ReceiptNo,DepartmentId,ProductId,Model,SN,EngineerId,Description,Remarks,ClosingDate,PendingReasonId,CancelReasonId,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,IsDeleted")] ServiceRequest serviceRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CenterId = new SelectList(DataManager.Centers(), "Id", "Name", serviceRequest.CenterId);
            ViewBag.DepartmentId = new SelectList(DataManager.Departments(), "Id", "Name", serviceRequest.DepartmentId);
            ViewBag.ProductId = new SelectList(DataManager.Products().Where(x => serviceRequest.DepartmentId == null || x.DepartmentId == serviceRequest.DepartmentId), "Id", "Name", serviceRequest.ProductId);
            ViewBag.EngineerId = new SelectList(DataManager.Engineers().Where(x => serviceRequest.DepartmentId == null || x.DepartmentId == serviceRequest.DepartmentId), "Id", "Name", serviceRequest.EngineerId);
            ViewBag.PendingReasonId = new SelectList(db.PendingReasons, "Id", "Reason", serviceRequest.PendingReasonId);
            ViewBag.CancelReasonId = new SelectList(db.CancelReasons, "Id", "Reason", serviceRequest.CancelReasonId);
            ViewBag.StatusId = Utils.ListManager.GetStatus();

            return View(serviceRequest);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.ServiceRequests.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            var model = db.ServiceRequests.Find(id);
            db.ServiceRequests.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}