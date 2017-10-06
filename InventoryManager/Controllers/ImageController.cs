using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryManager.Models;
using InventoryManager.ViewModel;
using JsonResult = InventoryManager.ViewModel.JsonResult;

namespace InventoryManager.Controllers
{
    public class DocumentController : Controller
    {
        private InventoryContext db = new InventoryContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(DocumentJson viewModel, int InventoryId = 0)
        {
            if (InventoryId == 0) return new HttpNotFoundResult();

            if(db.Inventories.Find(InventoryId) == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var json = new DocumentJson();
            if (string.IsNullOrEmpty(viewModel.Name))
            {
                json.ResponseStatus = JsonResult.JsonResponse.Failed;
                json.Message = "Please Enter a Document Name";
                return Json(json);
            }

            if (viewModel.File != null)
            {
                if (viewModel.File.ContentLength > 20971520)
                {
                    json.ResponseStatus = JsonResult.JsonResponse.Failed;
                    json.Message = "File Cannot Be more than 20MB.";
                    return Json(json);
                }
                
                var allowedTypes = new string[] {"image/png", "image/jpeg", "img/jpg", "application/pdf" };

                if (allowedTypes.Contains(viewModel.File.ContentType))
                {
                    var rand = new Random();
                    var path = rand.Next(1111111, 99999999) + "_" + viewModel.File.FileName;
                    
                    try
                    {
                        viewModel.File.SaveAs(Server.MapPath(ConfigurationManager.AppSettings["DocumentLocation"] + path));

                        db.Documents.Add(new Document()
                        {
                            InventoryId = InventoryId,
                            Path = path,
                            FileType = viewModel.File.ContentType,
                            Name = viewModel.Name,
                            UploadedOn = DateTime.Now,
                        });

                        db.SaveChanges();

                        json.ResponseStatus = JsonResult.JsonResponse.Successful;
                        json.Message = "File Uploaded Succesfully";
                        return Json(json);
                    }
                    catch (Exception e)
                    {
                        json.ResponseStatus = JsonResult.JsonResponse.Failed;
                        json.Message = "Cannot Upload File. Please Contact Admin";
                        return Json(json);
                    }
                }
            }
            
            json.ResponseStatus = JsonResult.JsonResponse.Failed;
            json.Message = "Please submit a valid File";
            return Json(json);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadFiles(int InventoryId = 0)
        {
            var json = new DocumentListJson();

            if (InventoryId == 0) return new HttpNotFoundResult();

            if (db.Inventories.Find(InventoryId) == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var documents = db.Documents.Where(i => i.InventoryId == InventoryId).ToList();

            json.Documents = documents;

            json.ResponseStatus = JsonResult.JsonResponse.Successful;
            json.Message = "Files Successfully Loaded";

            return Json(json);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? ImageId)
        {
            if (ImageId == 0) return new HttpNotFoundResult();

            var document = db.Documents.SingleOrDefault(i => i.Id == ImageId);
            if (document == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            db.Documents.Remove(document);
            var filepath = Request.MapPath(ConfigurationManager.AppSettings["DocumentLocation"] + document.Path);

            var json = new DocumentJson();
            try
            {
                if (System.IO.File.Exists(filepath))
                    System.IO.File.Delete(filepath);

                db.SaveChanges();

                json.ResponseStatus = JsonResult.JsonResponse.Successful;
                json.Message = "Document Successfully Removed.";
            }
            catch (Exception e)
            {
                json.ResponseStatus = JsonResult.JsonResponse.Failed;
                json.Message = "Cannot Remove Document. Please Contact Admin.";
            }
            
            return Json(json);
        }

        public ActionResult Download(int? Id)
        {
            if (Id == 0) return new HttpNotFoundResult();

            var document = db.Documents.SingleOrDefault(i => i.Id == Id);
            if (document == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var file = Server.MapPath(ConfigurationManager.AppSettings["DocumentLocation"] + document.Path);
            Response.AddHeader("Content-Disposition", $"attachment; filename=\"{document.Path}\"");
            Response.ContentType = document.FileType;
            try
            {
                Response.WriteFile(file);
            }
            catch (Exception e)
            {
                return new HttpNotFoundResult();
            }
            
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}