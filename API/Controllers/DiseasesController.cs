using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Infrastructure.Data;
using Core.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace API.Controllers
{
    [ApiController]
    public class DiseasesController : ControllerBase
    {
        private readonly IDiseaseRepository _repo;
        public DiseasesController(IDiseaseRepository repo)
        {
            _repo=repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Disease>>> GetDiseases()
        {
            var Diseases =await _repo.GetDiseasesAsync();

            return Ok(Diseases);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Disease>> GetDisease(int id)
        {
          return await _repo.GetDiseaseByIdAsync(id);
        }

    }
}