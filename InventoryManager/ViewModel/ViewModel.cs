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
}