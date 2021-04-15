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
using System.Linq;
using Microsoft.AspNetCore.Authorization;

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


    [HttpGet("AllDiseases")]
  
    public async Task<ActionResult<Pagination<DiseaseToReturnDto>>> GetDiseases([FromQuery] DiseaseSpecParams DiseaseParams)
    {
        var spec = new DiseasesWithSpecializationSpecification(DiseaseParams);
        
        var countSpec = new DiseaseWithFiltersForCountSpecification(DiseaseParams);
        
        var totalItems = await _DiseaseRepo.CountAsync(countSpec);
        
        var Diseases = await _DiseaseRepo.ListAsync(spec);
        
        var data = _mapper
        .Map<IReadOnlyList<Disease>, IReadOnlyList<DiseaseToReturnDto>>(Diseases);
        
        return Ok(new Pagination<DiseaseToReturnDto>(DiseaseParams.PageIndex,
        DiseaseParams.PageSize, totalItems, data));
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

    [HttpGet("specializations")]
    public async Task<ActionResult<IReadOnlyList<Specialization>>> GetDiseaseSpecialization()
    {
        return Ok(await _SpecializationRepo.ListAllAsync());
    }
    //------------------------------------------------------
   //Create New Disease
    [Authorize(Roles=PersonRoles.Admin)] 
    [HttpGet]
    public IActionResult Create()
        {
            return Ok();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Description,PictureUrl,SpecializationId")] Disease Disease)
        {
            var file = Request.Form.Files[0];
            if ((file != null && file.Length > 0))
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine("wwwroot/images/Diseases", fileName);
                var fileStream = new FileStream(path, FileMode.Create);
                file.CopyTo(fileStream);
                Disease.PictureUrl = fileName;
              
            }
            if (ModelState.IsValid)
            {
                _DiseaseRepo.Add(Disease);
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }
        //To delete Disease  
        [Authorize(Roles=PersonRoles.Admin)]  
        [HttpPost]
        public ActionResult Delete(Disease Disease)
        {
            _DiseaseRepo.Delete(Disease);
            return Ok();
        }

        //to update Disease
     [Authorize(Roles=PersonRoles.Admin)]   
    [HttpGet]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<DiseaseToReturnDto>> Edit(int? id)
    {
            if (id == null)
            {
                return BadRequest(new ApiResponse(400));
            }

       var spec = new DiseasesWithSpecializationSpecification((int)id);
       var Disease= await _DiseaseRepo.GetEntityWithSpec(spec);
            if (Disease == null)
            {
                return  BadRequest(new ApiResponse(400));
            }
            return _mapper.Map<Disease, DiseaseToReturnDto>(Disease);
    }
        [Authorize(Roles=PersonRoles.Admin)]
        [HttpPost]
        public IActionResult Edit([Bind("Id,Name,Description,PictureUrl,SpecializationId")] Disease Disease)
        {
            if (Request.Form.Files.Any())
            {
                var file = Request.Form.Files[0];
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine("wwwroot/images/Diseases", fileName);
                var fileStream = new FileStream(path, FileMode.Create);
                file.CopyTo(fileStream);
                Disease.PictureUrl = fileName;
                _DiseaseRepo.Update(Disease);
                return Ok();

            }
            else
            {
                _DiseaseRepo.Update(Disease);
                return Ok();
            }
        }

    }
}