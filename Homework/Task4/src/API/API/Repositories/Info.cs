using System;
using System.ComponentModel.DataAnnotations;

namespace API.Repositories
{
    public class Info
    {
        [Key]
        public int Id { get; set; }
        public DateTime? Created { get; set; }
        public string Description { get; set; }
    }
}
