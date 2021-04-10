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

namespace API.Controllers
{

    public class DoctorController : BaseApiController
    {
           private readonly UserManager<Person> _userManager;
        //_signInManager used to check the Password if exists or not
        private readonly SignInManager<Person> _signInManager;
        private readonly ITokenService _tokenService;
         private readonly RoleManager<IdentityRole> _roleManager;  
         private readonly EmailSender _emailSender;
        private readonly IGenericRepository<Doctor> _DoctorRepo;
        public readonly IGenericRepository<Specialization> _SpecializationRepo; 
        private readonly IMapper _mapper;
        public DoctorController(IGenericRepository<Doctor> DoctorRepo, IMapper mapper,UserManager<Person> userManager,
         SignInManager<Person> signInManager,ITokenService tokenService
         ,RoleManager<IdentityRole> roleManager,EmailSender emailSender)
        {
            _DoctorRepo = DoctorRepo;
            _mapper = mapper;
            _mapper = mapper;
            _emailSender=emailSender;
            _userManager=userManager;
            _roleManager=roleManager;
            _signInManager=signInManager;
            _tokenService=tokenService;

        }

        [HttpGet("Doctors")]
        public async Task<ActionResult<IReadOnlyList<List<Doctor>>>> GetDoctors()
        {
            
            var doctors = await _DoctorRepo.ListAllAsync();
          
            return Ok(doctors);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctor(int id)
        {
           return await _DoctorRepo.GetByIdAsync(id);
        }
        [HttpGet("specializations")]
        public async Task<ActionResult<IReadOnlyList<Specialization>>> GetDoctorSpecialization()
        {
            return Ok(await _SpecializationRepo.ListAllAsync());
        } 

        [Authorize(Roles=PersonRoles.Admin)]  
        [HttpPost]
        public ActionResult Delete(Doctor doctor)
        {
            _DoctorRepo.Delete(doctor);
            return Ok();
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
                Token =_tokenService.CreateToken(doctor),
                DisplayName = doctor.Name,
                LastLogin=LastLoginDate

            };
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

    }
}