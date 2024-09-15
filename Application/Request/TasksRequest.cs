using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class TasksRequest
    {
        public string? Name { get; set; }
        public DateTime DueDate { get; set; }
        public int User {  get; set; }
        public int Status { get; set; }
    }
}
