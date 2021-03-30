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
using System.IO;


namespace API.Controllers
{
    public class diseasesController : BaseApiController
    {

        public readonly IGenericRepository<disease> _diseaseRepo;
        public readonly IGenericRepository<Specialization> _SpecializationRepo;  
        private readonly IMapper _mapper;
        public diseasesController(IGenericRepository<disease> diseaseRepo,
      

        IGenericRepository<Specialization> SpecializationRepo, IMapper mapper)
        {
            _mapper = mapper;
            _SpecializationRepo = SpecializationRepo;
            _diseaseRepo = diseaseRepo;

        } 


    [HttpGet("diseases")]
  
    public async Task<ActionResult<Pagination<disease>>> Getdiseases([FromQuery] diseaseSpecParams diseaseParams)
    {
        var spec = new diseasesWithSpecializationSpecification(diseaseParams);
        var diseases = await _diseaseRepo.ListAsync(spec);
        var data = _mapper
        .Map<IReadOnlyList<disease>, IReadOnlyList<diseaseToReturnDto>>(diseases);
        return Ok(data);
      
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<diseaseToReturnDto>> Getdisease(int id)
    {
        var spec = new diseasesWithSpecializationSpecification(id);
       var disease= await _diseaseRepo.GetEntityWithSpec(spec);
        
      if (disease == null) return NotFound(new ApiResponse(404));

       return _mapper.Map<disease, diseaseToReturnDto>(disease);
    }

    [HttpGet("specializations")]
    public async Task<ActionResult<IReadOnlyList<Specialization>>> GetdiseaseSpecialization()
    {
        return Ok(await _SpecializationRepo.ListAllAsync());
    }
    //------------------------------------------------------
   //Create New disease
    [HttpGet]
    public IActionResult Create()
        {
            return Ok();
        }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description,PictureUrl,SpecializationId")] disease disease)
        {
             var file = Request.Form.Files[0];
            if ((file != null && file.Length > 0))
                {
                  var fileName = Path.GetFileName(file.FileName);
                  var path = Path.Combine("wwwroot/images/diseases", fileName);
                  var fileStream = new FileStream(path, FileMode.Create);  
                  file.CopyTo(fileStream);  
                  disease.PictureUrl = fileName;

                }
            if (ModelState.IsValid)
            {
                await _diseaseRepo.Add(disease);  
            }else
            {
               return await BadRequest(new ApiResponse(400));
            }
            
        }
    //To delete disease    
    [HttpPost]
    public async Task<IActionResult> Delete(disease disease)
    {
       await _diseaseRepo.Delete(disease);
    }  

    //to update disease
    [HttpGet]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult<diseaseToReturnDto>> Edit(int? id)
    {
            if (id == null)
            {
                return await BadRequest(new ApiResponse(400));
            }

       var spec = new diseasesWithSpecializationSpecification(id);
       var disease= await _diseaseRepo.GetEntityWithSpec(spec);
            if (disease == null)
            {
                return await BadRequest(new ApiResponse(400));
            }
            return _mapper.Map<disease, diseaseToReturnDto>(disease);
    }
   [HttpPost]
   public async Task<IActionResult> Edit([Bind("Id,Name,Description,PictureUrl,SpecializationId")] disease disease)
   {
      if (Request.Form.Files.Any())
         {
            var file = Request.Form.Files[0];
            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine("wwwroot/images/diseases", fileName);
            var fileStream = new FileStream(path, FileMode.Create);          
            file.CopyTo(fileStream);  
            disease.PictureUrl = fileName;             
            _diseaseRepo.Update(disease);
            return await Ok();                    

        }else{
            _diseaseRepo.Update(disease);
            return await Ok();
            }
   }   

}
}