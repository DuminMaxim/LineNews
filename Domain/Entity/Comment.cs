using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Comment
    {
        public int CommentId { get; set; }  

        [Required]
        public string Text { get; set; }        // Текст комментария

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }      // Дата создания  

        [Required]
        public virtual Blog Blog { get; set; }  // FK: ссылка на новость

        public virtual User User { get; set; }  // FK: ссылка на пользователя
    }
}
