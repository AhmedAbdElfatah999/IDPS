using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    [ApiController]
    public class AdminController : BaseApiController
    {
        private readonly IPersonGenericRepository<Admin> _AdminRepo;

        private readonly IMapper _mapper;
        //adding Identity Methods
        //_userManager used to Check for the admin in Database
        private readonly UserManager<Person> _userManager;
        //_signInManager used to check the Password if exists or not
        private readonly SignInManager<Person> _signInManager;
        private readonly ITokenService _tokenService;
         private readonly RoleManager<IdentityRole> _roleManager;  
       //Initialize them in the controller
        public AdminController(IPersonGenericRepository<Admin> AdminRepo, IMapper mapper,UserManager<Person> userManager,
         SignInManager<Person> signInManager,ITokenService tokenService,RoleManager<IdentityRole> roleManager)
        {
            _AdminRepo = AdminRepo;
            _mapper = mapper;
            _signInManager=signInManager;
            _userManager=userManager;
            _tokenService=tokenService;
            _roleManager=roleManager;
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

      //------------------------------The Identity Methods------------------------------
      
       [Authorize]
        [HttpGet]
        public async Task<ActionResult<AdminDto>> GetCurrentUser()
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x=>x.Type==ClaimTypes.Email)?.Value;
            var admin=await _userManager.FindByEmailAsync(email);

            return new AdminDto
            {
                Email = admin.Email,
                Token = "Message",
                DisplayName = admin.Name
            };
        }
         //check for Email If it is already exists
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        //Login Method
        [HttpPost("login")]
        public async Task<ActionResult<AdminDto>> Login(LoginDto loginDto)
        {
            var admin = await _userManager.FindByEmailAsync(loginDto.Email);

            if (admin == null) return Unauthorized(new ApiResponse(401));;

            var result = _signInManager.UserManager.Users.Where(x=>x.Password==loginDto.Password);

           if (result ==null) return Unauthorized(new ApiResponse(401));
            //Last Login Functionality
            TimeSpan LastLoginDate=DateTime.Now.Subtract((DateTime)admin.LastLogin);
            admin.LastLogin = DateTime.Now;
            var lastLoginResult = await _userManager.UpdateAsync(admin);
            if (!lastLoginResult.Succeeded)
            {
                throw new InvalidOperationException("Unexpected error occurred setting the last login date");
            }
            return new AdminDto
            {
                Email = admin.Email,
                Token =_tokenService.CreateToken(admin),
                DisplayName = admin.Name,
                LastLogin=LastLoginDate

            };

        }

        [HttpPost("register")]
        public async Task<ActionResult<AdminDto>> Register(Admin admin)
        {
            //Check the Email 
            if (CheckEmailExistsAsync(admin.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors = new []{"Email address is in use"}});
            }
            //Check the new admin data
            var result = await _userManager.CreateAsync(admin);
            if (!result.Succeeded)  
                return NotFound("Admin creation failed! Please check user details and try again.");
            //Add Role 
                  
               
            if (await _roleManager.RoleExistsAsync(PersonRoles.Admin))  
            {  
                await _userManager.AddToRoleAsync(admin, PersonRoles.Admin);  
            }     
            //Last Login Functionality
            TimeSpan LastLoginDate=DateTime.Now.Subtract((DateTime)admin.LastLogin);
            admin.LastLogin = DateTime.Now;
            var lastLoginResult = await _userManager.UpdateAsync(admin);
            if (!lastLoginResult.Succeeded)
            {
                throw new InvalidOperationException("Unexpected error occurred setting the last login date");
            }
            return new  AdminDto
            {
                DisplayName = admin.Name,
                Token = _tokenService.CreateToken(admin),
                Email = admin.Email,
                LastLogin=LastLoginDate
            };
        }

    }
}