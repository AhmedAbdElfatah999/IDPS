using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specification
{
    public class DiseasesWithSpecializationSpecification : BaseSpecification<Disease>
    {
        public DiseasesWithSpecializationSpecification(string sort) 
        {
            AddInclude(x => x.Specialization);
            AddOrderBy(x => x.Name);
            if(!string.IsNullOrEmpty(sort))
            {
                AddOrderBy(p => p.Name);
            }
        }

        public DiseasesWithSpecializationSpecification(int id) 
            : base(x => x.Id == id)
        {
            AddInclude(x => x.Specialization);
          
        }
    }
}