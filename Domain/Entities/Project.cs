using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Project
    {
        public Guid ProjectID { get; set; }
        public string? ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public int ClientID { get; set; }

        public int CampaignType {  get; set; }

        public Client? Clients { get; set; }

        public CampaignType? CampaignTypes { get; set; }

        public ICollection<Task>? ListTasks { get; set; }
        public ICollection<Interaction>? ListInteractions { get; set; }
    }
}
