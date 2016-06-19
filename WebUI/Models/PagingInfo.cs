using System;

namespace WebUI.Models
{
    public class PagingInfo
    {
        public int CurrentPage { get; set; }
        public int BlogsPerPage { get; set; }
        public int TotalBlogs { get; set; }
        public int TotalPage
        {
            get
            {
                return (int)Math.Ceiling((decimal)TotalBlogs / BlogsPerPage);
            }
        }

    }
}