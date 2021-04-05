using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
     [Authorize(Roles=PersonRoles.Admin)]
    public class SpecializationController :BaseApiController
    {
        private readonly IGenericRepository<Specialization> _SpecializationRepo;

        private readonly IMapper _mapper;
        public SpecializationController(IGenericRepository<Specialization> SpecializationRepo, IMapper mapper)
        {
            _SpecializationRepo = SpecializationRepo;
            _mapper = mapper;

        }

        [HttpGet("AllSpecializations")]
        public async Task<ActionResult<IReadOnlyList<List<Specialization>>>> GetSpecializations()
        {
            
            var specializations = await _SpecializationRepo.ListAllAsync();
          
            return Ok(specializations);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Specialization>> GetSpecialization(int id)
        {
           return await _SpecializationRepo.GetByIdAsync(id);
        }
 //------------------------------------------------------
   //Create New Specialization
    [HttpGet]
    public IActionResult Create()
        {
            return Ok();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] Specialization specialization)
        {

            if (ModelState.IsValid)
            {
                _SpecializationRepo.Add(specialization);
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }
    //To delete Specialization    
    [HttpPost]
    public  IActionResult Delete(Specialization specialization)
    {
       _SpecializationRepo.Delete(specialization);
       return Ok();
    }  

    //to update Specialization
    [HttpGet]
    [ValidateAntiForgeryToken]
    public ActionResult<Specialization> Edit(int? id)
    {
            if (id == null)
            {
                return NotFound();
            }

            var specialization= _SpecializationRepo.GetByIdAsync((int)id);
            if (specialization == null)
            {
                return NotFound();
            }
            return Ok(specialization);
    }
        [HttpPost]
        public IActionResult Edit([Bind("Id,Name")] Specialization Specialization)
        {

            _SpecializationRepo.Update(Specialization);
            return Ok();
        }
    }
}