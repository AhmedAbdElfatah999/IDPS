using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class PersonController :BaseApiController
    {
        private readonly IGenericRepository<Person> _PersonRepo;

        private readonly IMapper _mapper;
        public PersonController(IGenericRepository<Person> PersonRepo, IMapper mapper)
        {
            _PersonRepo = PersonRepo;
            _mapper = mapper;

        }

        [HttpGet("Persons")]
        public async Task<ActionResult<IReadOnlyList<List<Person>>>> GetPersons()
        {
            
            var persons = await _PersonRepo.ListAllAsync();
          
            return Ok(persons);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
           return await _PersonRepo.GetByIdAsync(id);
        }
        
    }
}