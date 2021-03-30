using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class AdminController : BaseApiController
    {
        private readonly IGenericRepository<Admin> _AdminRepo;

        private readonly IMapper _mapper;
        public AdminController(IGenericRepository<Admin> AdminRepo, IMapper mapper)
        {
            _AdminRepo = AdminRepo;
            _mapper = mapper;

        }

        [HttpGet("Admins")]
        public async Task<ActionResult<IReadOnlyList<List<Admin>>>> GetAdmins()
        {
            
            var admin = await _AdminRepo.ListAllAsync();
          
            return Ok(admin);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdmin(int id)
        {
           return await _AdminRepo.GetByIdAsync(id);
        }

    

    }
}