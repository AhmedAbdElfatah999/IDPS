using Core.Entities;

namespace Core.Specification
{
    public class DiseaseWithFiltersForCountSpecification : BaseSpecification<Disease>
    {
        public DiseaseWithFiltersForCountSpecification(DiseaseSpecParams diseaseParams)
            :base(x => 
                (string.IsNullOrEmpty(diseaseParams.Search) || x.Name.ToLower().Contains(diseaseParams.Search))
                &&
                (!diseaseParams.specId.HasValue || x.SpecializationId == diseaseParams.specId)
            )
        {
            
        }
    }
}