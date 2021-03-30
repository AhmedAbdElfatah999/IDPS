using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class SpecializationController :BaseApiController
    {
        private readonly IGenericRepository<Specialization> _SpecializationRepo;

        private readonly IMapper _mapper;
        public SpecializationController(IGenericRepository<Specialization> SpecializationRepo, IMapper mapper)
        {
            _SpecializationRepo = SpecializationRepo;
            _mapper = mapper;

        }

        [HttpGet("Specializations")]
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
    public async Task<IActionResult> Create([Bind("Id,Name")] Specialization specialization)
        {

            if (ModelState.IsValid)
            {
                await _SpecializationRepo.Add(specialization);  
            }else
            {
               return await BadRequest(new ApiResponse(400));
            }
            
        }
    //To delete Specialization    
    [HttpPost]
    public async Task<IActionResult> Delete(Specialization specialization)
    {
       await _SpecializationRepo.Delete(specialization);
    }  

    //to update Specialization
    [HttpGet]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult<Specialization>> Edit(int? id)
    {
            if (id == null)
            {
                return await BadRequest(new ApiResponse(400));
            }

            var specialization= await _SpecializationRepo.GetByIdAsync(id);
            if (specialization == null)
            {
                return await BadRequest(new ApiResponse(400));
            }
            return specialization;
    }
   [HttpPost]
   public async Task<IActionResult> Edit([Bind("Id,Name")] Specialization Specialization)
   {
                   
         _SpecializationRepo.Update(Specialization);
            return await Ok();                    
   }                   
    }
}