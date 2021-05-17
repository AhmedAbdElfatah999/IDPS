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
using Infrastructure.Services;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;

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
         [Authorize(Roles=PersonRoles.Admin)]
        [HttpGet("Admins")]
        public async Task<ActionResult<IReadOnlyList<List<Admin>>>> GetAdmins()
        {
            
            var admin = await _AdminRepo.ListAllAsync();
          
            return Ok(admin);

        }
        [Authorize(Roles=PersonRoles.Admin)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdmin(int id)
        {
           return await _AdminRepo.GetByIdAsync(id);
        }

      //------------------------------The Identity Methods------------------------------
      
        //[Authorize]
        [HttpGet("Account")]
        public async Task<ActionResult<AdminDto>> GetCurrentUser()
        {
            var email = HttpContext.Session.GetString("email");
            var admin=await _userManager.FindByEmailAsync(email);
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
                Token = _tokenService.CreateToken(admin),
                DisplayName = admin.Name,
                LastLogin=LastLoginDate

            };
        }
         //check for Email If it is already exists
          [Authorize(Roles=PersonRoles.Admin)]
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
            //save email in a session
            HttpContext.Session.SetString("email",admin.Email);

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
        //The Logout Method
        public ActionResult Logout(){
              HttpContext.Session.Clear();
            return Ok();
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
          [Authorize(Roles=PersonRoles.Admin)]
        //Forget Password Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
        {
            //Check the Email 
            var admin = await _userManager.FindByEmailAsync(forgetPasswordDto.Email);
            if (admin == null)
                 return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors = new []{"Email address is not exists"}});
             //Generate Url Token
            var token = await _userManager.GeneratePasswordResetTokenAsync(admin);
            var callback = Url.Action(nameof(ResetPassword), "admin", new { token, email = admin.Email }, Request.Scheme);
            string subject="Reset password token";
             EmailSender _emailSender=null;
            await _emailSender.SendEmailAsync(admin.Email,subject,callback);

            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordDto { Token = token, Email = email };
            return Ok(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
                    return Ok(resetPasswordDto);
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
                if (user == null)
                    RedirectToAction(nameof(ResetPasswordConfirmation));
            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
                if(!resetPassResult.Succeeded)
                {
                    foreach (var error in resetPassResult.Errors)
                    {
                        ModelState.TryAddModelError(error.Code, error.Description);
                    }
                    return Ok();
                }
                return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return Ok();
        }
        //Edit Profile Functionality
    [Authorize(Roles=PersonRoles.Admin)]   
    [HttpGet]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<Admin>> EditProfile(int? id)
    {
            if (id == null)
            {
                return BadRequest(new ApiResponse(400));
            }

       var admin= await _AdminRepo.GetByIdAsync((int)id);
            if (admin == null)
            {
                return  BadRequest(new ApiResponse(400));
            }
        return Ok(admin);
    }
        [Authorize(Roles=PersonRoles.Admin)]
        [HttpPost]
        public IActionResult EditProfile(Admin admin)
        {
            if (Request.Form.Files.Any())
            {
                var file = Request.Form.Files[0];
                var fileName = Path.GetFileName(file.FileName);
                var extension= Path.GetExtension(file.FileName);
                var path = Path.Combine("./client/src/assets/user", fileName);
                var fileStream = new FileStream(path, FileMode.Create);
                file.CopyTo(fileStream);
                admin.PictureUrl = fileName+extension;
                _AdminRepo.Update(admin);
                return Ok();

            }
            else
            {
               _AdminRepo.Update(admin);
                return Ok();
            }
        }//function Ends here
       //Delete Profile Functinality
        [Authorize(Roles=PersonRoles.Admin)]  
        [HttpPost]
        public ActionResult DeleteProfile(Admin admin)
        {
            
                 _AdminRepo.Delete(admin);
                  return Ok();
        }


    }
}