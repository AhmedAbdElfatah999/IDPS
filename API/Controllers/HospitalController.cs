using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class HospitalController :BaseApiController
    {
        private readonly IGenericRepository<Hospital> _HospitalRepo;

        private readonly IMapper _mapper;
        public HospitalController(IGenericRepository<Hospital> HospitalRepo, IMapper mapper)
        {
            _HospitalRepo = HospitalRepo;
            _mapper = mapper;

        }

        [HttpGet("ALlHospitals")]
        public async Task<ActionResult<Pagination<HospitalDto>>> GetHospitals(
            [FromQuery] HospitalSpecParams hospitalParams)
        {
            var spec = new HospitalWithSpecializationSpecification(hospitalParams);
            
            var countSpec = new HospitalWithFilterCountSpecification(hospitalParams);
            
            var totalItems = await _HospitalRepo.CountAsync(countSpec);
            
            var hospitals = await _HospitalRepo.ListAsync(spec);

            return Ok(new Pagination<Hospital>(hospitalParams.PageIndex,
            hospitalParams.PageSize, totalItems,hospitals));

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Hospital>> GetHospital(int id)
        {
           return await _HospitalRepo.GetByIdAsync(id);
        }

        //------------------------------------------------------
        //Create New Hospital
         [Authorize(Roles=PersonRoles.Admin)]   
        [HttpGet]
        public IActionResult Create()
        {
            return Ok();
        }
        
        [Authorize(Roles=PersonRoles.Admin)]   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Address,Phone,PictureUrl,Details")] Hospital hospital)
        {
            var file = Request.Form.Files[0];
            if ((file != null && file.Length > 0))
            {
              var fileName = Path.GetFileName(file.FileName);
              var path = Path.Combine("wwwroot/images/hospitals", fileName);
              var fileStream = new FileStream(path, FileMode.Create);  
              file.CopyTo(fileStream);  
              hospital.PictureUrl = fileName;

            }
            if (ModelState.IsValid)
            {
                _HospitalRepo.Add(hospital);
                return Ok();
            }
            else
            {
               return NotFound();
            }
            
        }

        //To delete hospital

        [Authorize(Roles=PersonRoles.Admin)]   
        [HttpPost]  
        public IActionResult Delete(List<Hospital> hospitals)
        {
            foreach (var item in hospitals)
            {
                _HospitalRepo.Delete(item);
            }
            
            return Ok();
        }

        //to update hospital
         [Authorize(Roles=PersonRoles.Admin)]   
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Hospital>> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospital= await _HospitalRepo.GetByIdAsync((int)id);
            if (hospital == null)
            {
                return NotFound();
            }
            return hospital;
        }
        [Authorize(Roles=PersonRoles.Admin)]   
        [HttpPost]
        public IActionResult Edit([Bind("Id,Name,Address,Phone,PictureUrl,Details")] Hospital hospital)
        {
            if (Request.Form.Files.Any())
            {
                var file = Request.Form.Files[0];
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine("wwwroot/images/hospitals", fileName);
                var fileStream = new FileStream(path, FileMode.Create);          
                file.CopyTo(fileStream);  
                hospital.PictureUrl = fileName;             
                _HospitalRepo.Update(hospital);
                return Ok();                    

            }
            else{
                _HospitalRepo.Update(hospital);
                return Ok();
            }
        }
    }
}