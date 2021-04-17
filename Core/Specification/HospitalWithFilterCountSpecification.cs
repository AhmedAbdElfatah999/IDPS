using Core.Entities;

namespace Core.Specification
{
    public class HospitalWithFilterCountSpecification : BaseSpecification<Hospital>
    {
        public HospitalWithFilterCountSpecification(
            HospitalSpecParams hospitalParams)
            :base(x => 
                (string.IsNullOrEmpty(hospitalParams.Search) || x.Name.ToLower().Contains(hospitalParams.Search)))
        {

        }
    }
}