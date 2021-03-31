using Core.Entities;

namespace Core.Specification
{
    public class DoctorWithFiltersForCountSpecification : BaseSpecification<Doctor>
    {
        public DoctorWithFiltersForCountSpecification(DoctorSpecParams DoctorParams)
            :base(x => 
                (string.IsNullOrEmpty(DoctorParams.Search) || x.Name.ToLower().Contains(DoctorParams.Search))
                &&
                (!DoctorParams.specId.HasValue || x.SpecializationId == DoctorParams.specId)
            )
        {
            
        }
    }
}
