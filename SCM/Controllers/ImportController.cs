using CsvHelper;
using CsvHelper.Excel;
using SCM.Models;
using SCM.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCM.Controllers
{ 
    [Authorize(Roles = "Admin")]
    public class ImportController : Controller
    {
        private static ImportManager _manager = null;

        public static ImportManager manager
        {
            get
            {
                if (_manager == null)
                    _manager = new ImportManager();

                return _manager;
            }
        }

        static ImportController()
        {            
        }

        // GET: Import
        public ActionResult Index()
        {
            return View();
        }

        // GET: Import
        public ActionResult GetProgress()
        {
            var result = new string[2];
            result[0] = manager.Progress.ToString();
            result[1] = manager.Phase;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadRequests()
        {
            string savedFileName = "";
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0)
                    continue;
                savedFileName = Path.Combine(
                   Server.MapPath("~/Content/Files"),
                   Path.GetFileName(hpf.FileName));
                hpf.SaveAs(savedFileName);
            }

            List<ServiceRequestRecord> requests = null;
            using (var reader = new CsvReader(new ExcelParser(savedFileName)))
            {
                reader.Configuration.RegisterClassMap<ServiceRequestRecordMap>();
                requests = reader.GetRecords<ServiceRequestRecord>().ToList();
            }
            //manager = new Utils.ImportManager();
            manager.Import(requests);

            Utils.DataManager.ResetCities();
            Utils.DataManager.ResetProducts();
            Utils.DataManager.ResetEngineers();
            Utils.DataManager.ResetTags();
            Utils.DataManager.ResetCustomers();
            Utils.DataManager.ResetRequests();
            return RedirectToAction("Index", "ServiceRequests");
        }
    }
}