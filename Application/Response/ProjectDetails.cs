using Application.UseCase;
using Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response
{
    public class ProjectDetails
    {
        public Project Data {  get; set; }
        public List<Interactions> Interactions { get; set; }
        public List<Tasks> Tasks { get; set; }
    }
}
