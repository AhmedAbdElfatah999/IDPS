using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specification
{
    public class DiseasesWithSpecializationSpecification : BaseSpecification<Disease>
    {

        public DiseasesWithSpecializationSpecification(DiseaseSpecParams diseaseParams)
            :base(x => 
                (!diseaseParams.specId.HasValue || x.SpecializationId == diseaseParams.specId)
            )
        {
            AddInclude(x => x.Specialization);
            AddOrderBy(x => x.Name);
            ApplyPaging(diseaseParams.PageSize * (diseaseParams.PageIndex -1),
             diseaseParams.PageSize);
            if(!string.IsNullOrEmpty(diseaseParams.Sort))
            {
                AddOrderBy(p => p.Name);
            }

        }


    }
}