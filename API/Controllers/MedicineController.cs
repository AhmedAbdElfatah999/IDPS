using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace API.Controllers
{
    
    public class MedicineController :BaseApiController
    {
        private readonly IGenericRepository<Medicine> _MedicineRepo;

        private readonly IMapper _mapper;
        public MedicineController(IGenericRepository<Medicine> MedicineRepo, IMapper mapper)
        {
            _MedicineRepo = MedicineRepo;
            _mapper = mapper;

        }

        [HttpGet("AllMedicines")]
        public async Task<ActionResult<IReadOnlyList<List<Medicine>>>> GetMedicines()
        {
            
            var medicines = await _MedicineRepo.ListAllAsync();
          
            return Ok(medicines);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Medicine>> GetMedicine(int id)
        {
           return await _MedicineRepo.GetByIdAsync(id);
        }
         //------------------------------------------------------
   //Create New medicine
   //Only Admin Can Access This Method
   [Authorize(Roles=PersonRoles.Admin)]
    [HttpGet]
    public IActionResult Create()
        {
            return Ok();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Manufacturer,Price,HowToTake,PictureUrl,Description")] Medicine medicine)
        {
            var file = Request.Form.Files[0];
            if ((file != null && file.Length > 0))
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine("wwwroot/images/medicines", fileName);
                var fileStream = new FileStream(path, FileMode.Create);
                file.CopyTo(fileStream);
                medicine.PictureUrl = fileName;

            }
            if (ModelState.IsValid)
            {
                _MedicineRepo.Add(medicine);
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }
        //To delete medicine  
        [Authorize(Roles=PersonRoles.Admin)]  
        [HttpPost]
        public IActionResult Delete(List<Medicine> medicines)
        {
            foreach (var item in medicines)
            {
                _MedicineRepo.Delete(item);
            }
            
            return Ok();
        }

        //to update medicine
        [Authorize(Roles=PersonRoles.Admin)]
        [HttpGet]
        [ValidateAntiForgeryToken]
    public async Task<ActionResult<Medicine>> Edit(int? id)
    {
            if (id == null)
            {
                return NotFound();
            }

            var medicine= await _MedicineRepo.GetByIdAsync((int)id);
            if (medicine == null)
            {
                return NotFound();
            }
            return medicine;
    }
        [Authorize(Roles=PersonRoles.Admin)]
        [HttpPost]
        public IActionResult Edit([Bind("Id,Name,Phone,PictureUrl,NumberOfBranches")] Medicine medicine)
        {
            if (Request.Form.Files.Any())
            {
                var file = Request.Form.Files[0];
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine("wwwroot/images/medicines", fileName);
                var fileStream = new FileStream(path, FileMode.Create);
                file.CopyTo(fileStream);
                medicine.PictureUrl = fileName;
                _MedicineRepo.Update(medicine);
                return Ok();

            }
            else
            {
                _MedicineRepo.Update(medicine);
                return Ok();
            }
        } 
    }
}