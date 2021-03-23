using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specification
{
    public class DiseasesWithSpecializationSpecification : BaseSpecification<Disease>
    {
        public DiseasesWithSpecializationSpecification() 
        {
            AddInclude(x=>x.Specialization);

        }

        public DiseasesWithSpecializationSpecification(int id) 
            : base(x => x.Id == id)
        {
            AddInclude(x => x.Specialization);
          
        }
    }
}