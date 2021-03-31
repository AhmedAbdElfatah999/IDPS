using Core.Entities;

namespace Core.Specification
{
    public class DoctorsWithSpecializationSpecification: BaseSpecification<Doctor>
    {
    public  DoctorsWithSpecializationSpecification(int id) 
            : base(x => x.SpecializationId == id)
        {
            AddInclude(x => x.Specialization);

        }  

    public DoctorsWithSpecializationSpecification(DoctorSpecParams DoctorParams)
            :base(x => 
                (string.IsNullOrEmpty(DoctorParams.Search) || x.Name.ToLower().Contains(DoctorParams.Search))
                && 
                (!DoctorParams.specId.HasValue || x.SpecializationId == DoctorParams.specId)
            )
        {
            AddInclude(x => x.Specialization);
            AddOrderBy(x => x.Name);
            ApplyPaging(DoctorParams.PageSize * (DoctorParams.PageIndex -1),
             DoctorParams.PageSize);
            if(!string.IsNullOrEmpty(DoctorParams.Sort))
            {
                AddOrderBy(p => p.Name);
            }

        }      
    }
}