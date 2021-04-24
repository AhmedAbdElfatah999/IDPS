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
    
    public class PharmacyController : BaseApiController
    {
        private readonly IGenericRepository<Pharmacy> _PharmacyRepo;

        private readonly IMapper _mapper;
        public PharmacyController(IGenericRepository<Pharmacy> PharmacyRepo, IMapper mapper)
        {
            _PharmacyRepo = PharmacyRepo;
            _mapper = mapper;

        }

        [HttpGet("AllPharmacies")]
        public async Task<ActionResult<Pagination<PharmactDto>>> GetPharmacys(
            [FromQuery] PharmacySpecParams PharmacyParams)
        {
            var spec = new PharmaciesWithBranchesSpecification(PharmacyParams);

            var countSpec = new PharmacyWithFiltersForCountSpecification(PharmacyParams);
            
            var totalItems = await _PharmacyRepo.CountAsync(countSpec);
            
            var pharmacies = await _PharmacyRepo.ListAsync(spec);

            
            return Ok(new Pagination<Pharmacy>(PharmacyParams.PageIndex,
            PharmacyParams.PageSize, totalItems, pharmacies));

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pharmacy>> GetPharmacy(int id)
        {
           return await _PharmacyRepo.GetByIdAsync(id);
        }
   //------------------------------------------------------
   //Create New pharmacy
    [Authorize(Roles=PersonRoles.Admin)]   
    [HttpGet]
    public IActionResult Create()
        {
            return Ok();
        }
        [Authorize(Roles=PersonRoles.Admin)]   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Phone,PictureUrl,NumberOfBranches")] Pharmacy pharmacy)
        {
            var file = Request.Form.Files[0];
            if ((file != null && file.Length > 0))
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine("wwwroot/images/pharmacies", fileName);
                var fileStream = new FileStream(path, FileMode.Create);
                file.CopyTo(fileStream);
                pharmacy.PictureUrl = fileName;

            }
            if (ModelState.IsValid)
            {
                _PharmacyRepo.Add(pharmacy);
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }
        //To delete pharmacy 
         [Authorize(Roles=PersonRoles.Admin)]      
        [HttpPost]
        public IActionResult Delete(Pharmacy pharmacy)
        {
            _PharmacyRepo.Delete(pharmacy);
            return Ok();
        }

        //to update pharmacy
    [Authorize(Roles=PersonRoles.Admin)]   
    [HttpGet]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<Pharmacy>> Edit(int? id)
    {
            if (id == null)
            {
                return NotFound();
            }

            var pharmacy= await _PharmacyRepo.GetByIdAsync((int)id);
            if (pharmacy == null)
            {
                return NotFound();
            }
            return pharmacy;
    }
       [Authorize(Roles=PersonRoles.Admin)]   
        [HttpPost]
        public IActionResult Edit([Bind("Id,Name,Phone,PictureUrl,NumberOfBranches")] Pharmacy pharmacy)
        {
            if (Request.Form.Files.Any())
            {
                var file = Request.Form.Files[0];
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine("wwwroot/images/pharmacies", fileName);
                var fileStream = new FileStream(path, FileMode.Create);
                file.CopyTo(fileStream);
                pharmacy.PictureUrl = fileName;
                _PharmacyRepo.Update(pharmacy);
                return Ok();

            }
            else
            {
                _PharmacyRepo.Update(pharmacy);
                return Ok();
            }
        }
    }
}