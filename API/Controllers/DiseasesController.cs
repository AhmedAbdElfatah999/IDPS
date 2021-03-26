using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Infrastructure.Data;
using Core.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Core.Specification;
using AutoMapper;
using API.Dtos;
using API.Helpers;

namespace API.Controllers
{
    [ApiController]
    public class DiseasesController : ControllerBase
    {

        public readonly IGenericRepository<Disease> _DiseaseRepo;
        public readonly IGenericRepository<Specialization> _SpecializationRepo;  
        private readonly IMapper _mapper;
        public DiseasesController(IGenericRepository<Disease> DiseaseRepo,
      

        IGenericRepository<Specialization> SpecializationRepo, IMapper mapper)
        {
            _mapper = mapper;
            _SpecializationRepo = SpecializationRepo;
            _DiseaseRepo = DiseaseRepo;

        }

    [HttpGet("Disease")]
    public async Task<ActionResult<Pagination<List<Disease>>>> GetDiseases(
        [FromQuery]DiseaseSpecParams diseaseParams)
    {
        var spec = new DiseasesWithSpecializationSpecification(diseaseParams);
        
        var countSpec = new DiseaseWithFiltersForCountSpecification(diseaseParams);

        var totalItems = await _DiseaseRepo.CountAsync(spec);

        var diseases = await _DiseaseRepo.ListAsync(spec);
        
        var data = _mapper
        .Map<IReadOnlyList<Disease>, IReadOnlyList<DiseaseToReturnDto>>(diseases);
        
        return Ok(new Pagination<DiseaseToReturnDto>(diseaseParams.PageIndex, diseaseParams.PageSize, totalItems, data));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DiseaseToReturnDto>> GetDisease(int id)
    {
        var spec = new DiseasesWithSpecializationSpecification(id);
        var disease= await _DiseaseRepo.GetEntityWithSpec(spec);
        
      // if (disease == null) return NotFound(new ApiResponse(404));

       return _mapper.Map<Disease, DiseaseToReturnDto>(disease);
    }

    [HttpGet("specialization")]
    public async Task<ActionResult<IReadOnlyList<Specialization>>> GetDiseaseSpecialization()
    {
        return Ok(await _SpecializationRepo.ListAllAsync());
    }

}
}