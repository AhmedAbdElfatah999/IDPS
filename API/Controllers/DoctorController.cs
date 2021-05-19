using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Dtos;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Services;
using API.Errors;
using System;
using System.Linq;
using API.Helpers;
using Core.Specification;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{

    public class DoctorController : BaseApiController
    {
           private readonly UserManager<Person> _userManager;
        //_signInManager used to check the Password if exists or not
        private readonly SignInManager<Person> _signInManager;
        private readonly ITokenService _tokenService;
         private readonly RoleManager<IdentityRole> _roleManager;  
     
        private readonly IPersonGenericRepository<Doctor> _DoctorRepo;
        public readonly IPersonGenericRepository<Specialization> _SpecializationRepo; 
        private readonly IMapper _mapper;
        public DoctorController(IPersonGenericRepository<Doctor> DoctorRepo, IMapper mapper,UserManager<Person> userManager,
         SignInManager<Person> signInManager,ITokenService tokenService
         ,RoleManager<IdentityRole> roleManager)
        {
            _DoctorRepo = DoctorRepo;
            _mapper = mapper;
            _userManager=userManager;
            _roleManager=roleManager;
            _signInManager=signInManager;
            _tokenService=tokenService;

        }

        [HttpGet("Doctors")]
        public async Task<ActionResult<Pagination<DoctorDto>>> GetDoctors([FromQuery] DoctorSpecParams doctorParams)
        {
            var spec = new DoctorsWithSpecializationSpecification(doctorParams);

            var countSpec = new DoctorWithFiltersForCountSpecification(doctorParams);
            
            var totalItems = await _DoctorRepo.CountAsync(countSpec);
            
            var doctors = await _DoctorRepo.ListAsync(spec);

           // var data = _mapper.Map<IReadOnlyList<Doctor>, IReadOnlyList<DoctorDto>>(doctors);

            return Ok(new Pagination<Doctor>(doctorParams.PageIndex,
            doctorParams.PageSize, totalItems,  doctors));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctor(string id)
        {
           return await _DoctorRepo.GetByIdAsync(id);
        }
        [HttpGet("specializations")]
        public async Task<ActionResult<IReadOnlyList<Specialization>>> GetDoctorSpecialization()
        {
            return Ok(await _SpecializationRepo.ListAllAsync());
        }
        
        [Authorize(Roles=PersonRoles.Admin)] 
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return Ok(await _SpecializationRepo.ListAllAsync());
        } 

        [Authorize(Roles=PersonRoles.Admin)]  
        [HttpPost]
        public ActionResult Delete(List<Doctor> doctors)
        {
            foreach (var item in doctors)
            {
                _DoctorRepo.Delete(item);
            }
            
            return Ok();
        }
        [Authorize]
        [HttpGet("Account")]
        public async Task<ActionResult<DoctorDto>> GetCurrentUser()
        {
            var email = HttpContext.Session.GetString("email");
            var doctor=await _userManager.FindByEmailAsync(email);
            //Last Login Functionality
            TimeSpan LastLoginDate=DateTime.Now.Subtract((DateTime)doctor.LastLogin);
            doctor.LastLogin = DateTime.Now;
            var lastLoginResult = await _userManager.UpdateAsync(doctor);
            if (!lastLoginResult.Succeeded)
            {
                throw new InvalidOperationException("Unexpected error occurred setting the last login date");
            }
            return new DoctorDto
            {
                Email = doctor.Email,
                Token = _tokenService.CreateToken(doctor),
                DisplayName = doctor.Name,
                LastLogin=LastLoginDate
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
        public async Task<ActionResult<DoctorDto>> Login(LoginDto loginDto)
        {
            var doctor = await _userManager.FindByEmailAsync(loginDto.Email);

            if (doctor == null) return Unauthorized(new ApiResponse(401));;

            var result = _signInManager.UserManager.Users.Where(x=>x.Password==loginDto.Password);

           if (result ==null) return Unauthorized(new ApiResponse(401));

           if(loginDto.RememberMe == true){
                await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password,
                loginDto.RememberMe, false);
            }
            
            //Last Login Functionality
            TimeSpan LastLoginDate=DateTime.Now.Subtract((DateTime)doctor.LastLogin);
            doctor.LastLogin = DateTime.Now;
            var lastLoginResult = await _userManager.UpdateAsync(doctor);
            if (!lastLoginResult.Succeeded)
            {
                throw new InvalidOperationException("Unexpected error occurred setting the last login date");
            }
            //save email in a session
            HttpContext.Session.SetString("email",doctor.Email);             
            return new DoctorDto
            {
                Email = doctor.Email,
                Token =_tokenService.CreateToken(doctor),
                DisplayName = doctor.Name,
                LastLogin=LastLoginDate

            };
        }
        //The Logout Method
        public ActionResult Logout(){
              HttpContext.Session.Clear();
            return Ok();
           }  
        [HttpPost("request")]
        public async Task<ActionResult<DoctorDto>> RegistrationRequest(Doctor doctor)
        {
            //Check the Email 
            if (CheckEmailExistsAsync(doctor.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors = new []{"Email address is in use"}});
            }
            
            //Check the new doctor data
            var result = await _userManager.CreateAsync(doctor);
            if (!result.Succeeded)  
                return NotFound("Doctor creation failed! Please check user details and try again.");
             
                  
            //Add Role
            if (await _roleManager.RoleExistsAsync(PersonRoles.Doctor))  
            {  
                await _userManager.AddToRoleAsync(doctor, PersonRoles.Doctor);  
            } 
                
            //Last Login Functionality
            TimeSpan LastLoginDate=DateTime.Now.Subtract((DateTime)doctor.LastLogin);
            doctor.LastLogin = DateTime.Now;
            var lastLoginResult = await _userManager.UpdateAsync(doctor);
            if (!lastLoginResult.Succeeded)
            {
                throw new InvalidOperationException("Unexpected error occurred setting the last login date");
            }
            return Ok();
        }

         //Forget Password Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> forgetPassword(ForgetPasswordDto forgetPasswordDto)
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
    [Authorize(Roles=PersonRoles.Doctor)]   
    [HttpGet]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<Doctor>> EditProfile(string id)
    {
            if (id == null)
            {
                return BadRequest(new ApiResponse(400));
            }

       var doctor= await _DoctorRepo.GetByIdAsync(id);
            if (doctor == null)
            {
                return  BadRequest(new ApiResponse(400));
            }
        return Ok(doctor);
    }
        [Authorize(Roles=PersonRoles.Admin)]
        [Authorize(Roles=PersonRoles.Doctor)]
        [HttpPost]
        public IActionResult EditProfile(Doctor doctor)
        {
            if (Request.Form.Files.Any())
            {
                var file = Request.Form.Files[0];
                var fileName = Path.GetFileName(file.FileName);
                var extension= Path.GetExtension(file.FileName);
                var path = Path.Combine("./client/src/assets/user", fileName);
                var fileStream = new FileStream(path, FileMode.Create);
                file.CopyTo(fileStream);
               doctor.PictureUrl = fileName+extension;
                _DoctorRepo.Update(doctor);
                return Ok();

            }
            else
            {
               _DoctorRepo.Update(doctor);
                return Ok();
            }
        }//function Ends here
       //Delete Profile Functinality
        [Authorize(Roles=PersonRoles.Admin)]
         [Authorize(Roles=PersonRoles.Doctor)]   
        [HttpPost]
        public ActionResult DeleteProfile(Doctor doctor)
        {
            
                 _DoctorRepo.Delete(doctor);
                  return Ok();
        }        

    }
}