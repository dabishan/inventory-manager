using System.Linq;
using System.Web.Mvc;
using InventoryManager.Models;

namespace InventoryManager.Controllers
{
    public class HomeController : Controller
    {

        private InventoryContext db = new InventoryContext();
        public ActionResult Index()
        {

            /*var inventory = db.Inventories.Where(i => i.OwnerId ==)*/
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}