using Core.Entities;

namespace Core.Specification
{
    public class PharmacyWithFiltersForCountSpecification : BaseSpecification<Pharmacy>
    {
        public PharmacyWithFiltersForCountSpecification(
            PharmacySpecParams pharmacyParams)
            :base(x => 
                (string.IsNullOrEmpty(pharmacyParams.Search) || x.Name.ToLower().Contains(pharmacyParams.Search)))
        {

        }
    }
}