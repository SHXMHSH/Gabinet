using System;
namespace Gabinet.Models
{
    public class PagingModel
    {
        public int ItemOnPage { get; set; }
        public int CurrentPage { get; set; }
        public int CountItem { get; set; }

        public int Page => (int)Math.Ceiling((decimal)CountItem / ItemOnPage);
    }
}
