using Core.Entities;

namespace Core.Specification
{
    public class PharmaciesWithBranchesSpecification : BaseSpecification<Pharmacy>
    {
        public PharmaciesWithBranchesSpecification(int value)
            : base(x => x.NumberOfBranches == value)
        {
            
        }
        public PharmaciesWithBranchesSpecification(PharmacySpecParams pharmacyParams)
            :base(x => 
                (string.IsNullOrEmpty(pharmacyParams.Search) || x.Name.ToLower().Contains(pharmacyParams.Search))
            )
        {
            
        }
    }
}