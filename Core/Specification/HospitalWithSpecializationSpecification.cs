using Core.Entities;

namespace Core.Specification 
{
    public class HospitalWithSpecializationSpecification : BaseSpecification<Hospital>
    {
        public HospitalWithSpecializationSpecification(HospitalSpecParams hospitalParams)
            :base(x => 
                (string.IsNullOrEmpty(hospitalParams.Search) || x.Name.ToLower().Contains(hospitalParams.Search))
            )
        {
            AddOrderBy(x => x.Name);
            ApplyPaging(hospitalParams.PageSize * (hospitalParams.PageIndex -1),
             hospitalParams.PageSize);
            
            if(!string.IsNullOrEmpty(hospitalParams.Sort))
            {
                AddOrderBy(p => p.Name);
            }
        }
    }
}