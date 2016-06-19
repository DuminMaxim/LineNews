using Domain.Entity;
using System.Collections.Generic;

namespace WebUI.Models
{
    public class BlogListViewModel
    {
        public List<Blog> Blogs { get; set; }

        public PagingInfo pagingInfo { get; set; }
    }
}