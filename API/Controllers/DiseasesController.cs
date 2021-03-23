using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Infrastructure.Data;
using Core.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Core.Specification;

namespace API.Controllers
{
    [ApiController]
    public class DiseasesController : ControllerBase
    {
    
        public readonly IGenericRepository<Disease> _DiseaseRepo ;
        public readonly IGenericRepository<Specialization> _SpecializationRepo ;
        public DiseasesController(IGenericRepository<Disease> DiseaseRepo,
   
        IGenericRepository<Specialization> SpecializationRepo)
        {
            _SpecializationRepo = SpecializationRepo;
            _DiseaseRepo = DiseaseRepo;

        }

        [HttpGet]
        public async Task<ActionResult<List<Disease>>> GetDiseases()
        {
            var spec=new DiseasesWithSpecializationSpecification();
            var Diseases = await _DiseaseRepo.ListAsync(spec);

            return Ok(Diseases);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Disease>> GetDisease(int id)
        {
            var spec=new DiseasesWithSpecializationSpecification(id);
            return await _DiseaseRepo.GetEntityWithSpec(spec);
        }
        
        [HttpGet("specialization")]
        public async Task<ActionResult<IReadOnlyList<Specialization>>> GetDiseaseSpecialization()
        {
            return Ok(await _SpecializationRepo.ListAllAsync());
        }

    }
}