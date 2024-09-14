using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request

{
    public class ProjectRequest
    {
        public string Name {  get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Client {  get; set; }
        public int CampaignType {  get; set; } 
    }
}
