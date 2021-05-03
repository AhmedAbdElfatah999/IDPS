using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class PatientController :BaseApiController
    {
        private readonly IGenericRepository<Patient> _PatientRepo;
       private readonly UserManager<Person> _userManager;
        //_signInManager used to check the Password if exists or not
        private readonly SignInManager<Person> _signInManager;
        private readonly ITokenService _tokenService;
         private readonly RoleManager<IdentityRole> _roleManager;  

        private readonly IMapper _mapper;
        public PatientController(IGenericRepository<Patient> PatientRepo
        , IMapper mapper,UserManager<Person> userManager,
         SignInManager<Person> signInManager,ITokenService tokenService
         ,RoleManager<IdentityRole> roleManager)
        {
            _PatientRepo = PatientRepo;
            _mapper = mapper;
            _userManager=userManager;
            _roleManager=roleManager;
            _signInManager=signInManager;
            _tokenService=tokenService;

        }

        [HttpGet("Patients")]
        public async Task<ActionResult<IReadOnlyList<List<Patient>>>> GetPatients()
        {
            
            var patients = await _PatientRepo.ListAllAsync();
          
            return Ok(patients);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
           return await _PatientRepo.GetByIdAsync(id);
        } 
        //check for Email If it is already exists
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        //Login Method
        [HttpPost("login")]
        public async Task<ActionResult<PatientDto>> Login(LoginDto loginDto)
        {
            var patient = await _userManager.FindByEmailAsync(loginDto.Email);

            if (patient == null) return Unauthorized(new ApiResponse(401));;

            var result = _signInManager.UserManager.Users.Where(x=>x.Password==loginDto.Password);

           if (result ==null) return Unauthorized(new ApiResponse(401));
            //Last Login Functionality
            TimeSpan LastLoginDate=DateTime.Now.Subtract((DateTime)patient.LastLogin);
            patient.LastLogin = DateTime.Now;
            var lastLoginResult = await _userManager.UpdateAsync(patient);
            if (!lastLoginResult.Succeeded)
            {
                throw new InvalidOperationException("Unexpected error occurred setting the last login date");
            }
            return new PatientDto
            {
                Email = patient.Email,
                Token =_tokenService.CreateToken(patient),
                DisplayName = patient.Name,
                LastLogin=LastLoginDate

            };

        }
        //The Logout Method
        public ActionResult Logout(){
              HttpContext.Session.Clear();
            return Ok();
           }  

        [HttpPost("register")]
        public async Task<ActionResult<PatientDto>> Register(Patient patient)
        {
            //Check the Email 
            if (CheckEmailExistsAsync(patient.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors = new []{"Email address is in use"}});
            }
            //Check the new patient data
            var result = await _userManager.CreateAsync(patient);
            if (!result.Succeeded)  
                return NotFound("patient creation failed! Please check user details and try again.");
            //Add Role 
                  
               
            if (await _roleManager.RoleExistsAsync(PersonRoles.Patient))  
            {  
                await _userManager.AddToRoleAsync(patient, PersonRoles.Patient);  
            }     
            //Last Login Functionality
            TimeSpan LastLoginDate=DateTime.Now.Subtract((DateTime)patient.LastLogin);
            patient.LastLogin = DateTime.Now;
            var lastLoginResult = await _userManager.UpdateAsync(patient);
            if (!lastLoginResult.Succeeded)
            {
                throw new InvalidOperationException("Unexpected error occurred setting the last login date");
            }
            return new  PatientDto
            {
                DisplayName = patient.Name,
                Token = _tokenService.CreateToken(patient),
                Email = patient.Email,
                LastLogin=LastLoginDate
            };
        }

        //Forget Password Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
        {
            //Check the Email 
            var patient = await _userManager.FindByEmailAsync(forgetPasswordDto.Email);
            if (patient == null)
                 return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors = new []{"Email address is not exists"}});
             //Generate Url Token
            var token = await _userManager.GeneratePasswordResetTokenAsync(patient);
            var callback = Url.Action(nameof(ResetPassword), "patient", new { token, email = patient.Email }, Request.Scheme);
            string subject="Reset password token";
           EmailSender _emailSender=null;
            await _emailSender.SendEmailAsync(patient.Email,subject,callback);

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
    [Authorize(Roles=PersonRoles.Patient)]   
    [HttpGet]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<Patient>> EditProfile(int? id)
    {
            if (id == null)
            {
                return BadRequest(new ApiResponse(400));
            }

       var patient= await _PatientRepo.GetByIdAsync((int)id);
            if ( patient == null)
            {
                return  BadRequest(new ApiResponse(400));
            }
        return Ok(patient);
    }
        [Authorize(Roles=PersonRoles.Patient)]
        [HttpPost]
        public IActionResult EditProfile(Patient  patient)
        {
            if (Request.Form.Files.Any())
            {
                var file = Request.Form.Files[0];
                var fileName = Path.GetFileName(file.FileName);
                var extension= Path.GetExtension(file.FileName);
                var path = Path.Combine("./client/src/assets/user", fileName);
                var fileStream = new FileStream(path, FileMode.Create);
                file.CopyTo(fileStream);
                patient.PictureUrl = fileName+extension;
                _PatientRepo.Update(patient);
                return Ok();

            }
            else
            {
               _PatientRepo.Update(patient);
                return Ok();
            }
        }//function Ends here
       //Delete Profile Functinality
         [Authorize(Roles=PersonRoles.Patient)]   
        [HttpPost]
        public ActionResult DeleteProfile(Patient  patient)
        {
            
                 _PatientRepo.Delete(patient);
                  return Ok();
        }           
    }
}