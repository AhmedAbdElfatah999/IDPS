using Core.Entities;

namespace Core.Specification
{
    public class PharmaciesWithBranchesSpecification : BaseSpecification<Pharmacy>
    {
        public PharmaciesWithBranchesSpecification(PharmacySpecParams pharmacyParams)
            :base(x => 
                (string.IsNullOrEmpty(pharmacyParams.Search) || x.Name.ToLower().Contains(pharmacyParams.Search))
            )
        {
            AddInclude(x => x.NumberOfBranches);
            AddOrderBy(x => x.Name);
            ApplyPaging(pharmacyParams.PageSize * (pharmacyParams.PageIndex -1),
             pharmacyParams.PageSize);
            
            if(!string.IsNullOrEmpty(pharmacyParams.Sort))
            {
                AddOrderBy(p => p.Name);
            }
        }
    }
}