using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infra.Models
{
    public class UserTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }
        public string AdminId { get; set; }
   

        public Category Category { get; set; }
        public ApplicationUser User { get; set; }
        public ApplicationUser Admin { get; set; }

    }
}
