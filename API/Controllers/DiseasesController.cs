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
using API.Errors;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    
    public class DiseasesController : BaseApiController
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
        [HttpGet("getdiseases2")]
         public string GetDiseases2(){
        return "Hello World";
    }

    [HttpGet("Diseases")]
  
    public async Task<ActionResult<Pagination<Disease>>> GetDiseases([FromQuery] DiseaseSpecParams DiseaseParams)
    {
        var spec = new DiseasesWithSpecializationSpecification(DiseaseParams);
        var diseases = await _DiseaseRepo.ListAsync(spec);
        var data = _mapper
        .Map<IReadOnlyList<Disease>, IReadOnlyList<DiseaseToReturnDto>>(diseases);
        return Ok(data);
      
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DiseaseToReturnDto>> GetDisease(int id)
    {
        var spec = new DiseasesWithSpecializationSpecification(id);
       var disease= await _DiseaseRepo.GetEntityWithSpec(spec);
        
      if (disease == null) return NotFound(new ApiResponse(404));

       return _mapper.Map<Disease, DiseaseToReturnDto>(disease);
    }

    [HttpGet("specialization")]
    public async Task<ActionResult<IReadOnlyList<Specialization>>> GetDiseaseSpecialization()
    {
        return Ok(await _SpecializationRepo.ListAllAsync());
    }

}
}