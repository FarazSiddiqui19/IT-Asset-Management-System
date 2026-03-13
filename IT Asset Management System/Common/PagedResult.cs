using System.Collections.Generic;

namespace IT_Asset_Management_System.Common
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
    }
}
