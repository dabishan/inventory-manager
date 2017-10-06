using System.ComponentModel;

namespace InventoryManager.ViewModel
{
    public class ViewModel
    {
        public string ReferrerUrl { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public ViewModel()
        {
            Page = 1;
            PageSize = 2;
        }
    }

    public class JsonResult : ViewModel
    {
        public enum JsonResponse { Failed = 0, Successful = 1}
        public JsonResponse ResponseStatus { get; set; }

        public string Message { get; set; }
    }
}