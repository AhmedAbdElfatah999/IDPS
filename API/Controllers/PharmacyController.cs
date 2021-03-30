using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
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

        [HttpGet("Pharmacies")]
        public async Task<ActionResult<IReadOnlyList<List<Pharmacy>>>> GetPharmacys()
        {
            
            var pharmacies = await _PharmacyRepo.ListAllAsync();
          
            return Ok(pharmacies);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pharmacy>> GetPharmacy(int id)
        {
           return await _PharmacyRepo.GetByIdAsync(id);
        }
   //------------------------------------------------------
   //Create New pharmacy
    [HttpGet]
    public IActionResult Create()
        {
            return Ok();
        }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Phone,PictureUrl,NumberOfBranches")] Pharmacy pharmacy)
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
                await _PharmacyRepo.Add(pharmacy);  
            }else
            {
               return await BadRequest(new ApiResponse(400));
            }
            
        }
    //To delete pharmacy    
    [HttpPost]
    public async Task<IActionResult> Delete(pharmacy pharmacy)
    {
       await _PharmacyRepo.Delete(pharmacy);
    }  

    //to update pharmacy
    [HttpGet]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult<Pharmacy>> Edit(int? id)
    {
            if (id == null)
            {
                return await BadRequest(new ApiResponse(400));
            }

            var pharmacy= await _PharmacyRepo.GetByIdAsync(id);
            if (pharmacy == null)
            {
                return await BadRequest(new ApiResponse(400));
            }
            return pharmacy;
    }
   [HttpPost]
   public async Task<IActionResult> Edit([Bind("Id,Name,Phone,PictureUrl,NumberOfBranches")] Pharmacy pharmacy)
   {
      if (Request.Form.Files.Any())
         {
            var file = Request.Form.Files[0];
            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine("wwwroot/images/pharmacies", fileName);
            var fileStream = new FileStream(path, FileMode.Create);          
            file.CopyTo(fileStream);  
            Pharmacy.PictureUrl = fileName;             
            _PharmacyRepo.Update(pharmacy);
            return await Ok();                    

        }else{
            _PharmacyRepo.Update(pharmacy);
            return await Ok();
            }
   }           
    }
}