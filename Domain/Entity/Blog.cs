using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Blog
    {
        public int BlogId { get; set; }

        [Required (ErrorMessage = "Введите название статьи")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }                 

        [Required(ErrorMessage = "Введите описание статьи")]
        [Display(Name = "Текст")]
        public string Description { get; set; }           

        [DataType(DataType.DateTime)]
        public DateTime? Date { get; set; }                 

        public byte[] ImageData { get; set; }               

        public string ImageExtention { get; set; }          

        public virtual List<Comment> Comments { get; set; }  
    }
}
