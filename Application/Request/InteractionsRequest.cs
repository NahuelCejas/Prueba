using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request
{
    public class InteractionsRequest
    {
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public int InteractionType { get; set; }

    }
}
