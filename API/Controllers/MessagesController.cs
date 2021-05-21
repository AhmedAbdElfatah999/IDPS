using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using API.Data;
using API.Dtos;
using API.Helpers;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using API.Extensions;

namespace API.Controllers
{
    //[ServiceFilter(typeof(LogUserActivity))]
    //[Authorize]
    [Route("api/user/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IPersonGenericRepository<Person> _repo;
        private readonly IMessageGenericRepository<Message> _messageRepo;
        private readonly IMapper _mapper;
        public MessagesController(IPersonGenericRepository<Person> repo, IMapper mapper, IMessageGenericRepository<Message> messageRepo)
        {
            _mapper = mapper;
            _repo = repo;
            _messageRepo=messageRepo;
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(string userId, int id)
        {


            var messageFromRepo = await _messageRepo.GetMessage(id);

            if (messageFromRepo == null)
                return NotFound();

            return Ok(messageFromRepo);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(string userId, 
            [FromQuery]MessageParams messageParams)
        {
             var user= await _repo.GetByIdAsync(userId);
            if (user ==null)
            {
                return Unauthorized();
            }
            messageParams.UserId = userId;

            var messagesFromRepo = await _messageRepo.GetMessagesForUser(messageParams);

            var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);
         
     Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize, 
                messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);
            
            return Ok(messages);
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(string userId, string recipientId)
        {
 

            var messagesFromRepo = await _messageRepo.GetMessageThread(userId, recipientId);

            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            return Ok(messageThread);
        }

        [HttpPost("SendToPatient")]
        public async Task<IActionResult> SendToPatient(string userId, MessageForCreationDto messageForCreationDto)
        {
            var sender = await _repo.GetByIdAsync(userId);

            messageForCreationDto.DoctorId = userId;
            messageForCreationDto.SenderId=userId;  
            var recipient = await _repo.GetByIdAsync(messageForCreationDto.PatientId);
            messageForCreationDto.ReceieverId=recipient.Id;
            if (recipient == null)
                return BadRequest("Could not find user");
            
            var message = _mapper.Map<Message>(messageForCreationDto);

            _messageRepo.Add(message);

            if (await _messageRepo.SaveAll())
            {
                var messageToReturn = _mapper.Map<MessageToReturnDto>(message);

                return Ok(messageToReturn);
            }

            throw new Exception("Creating the message failed on save");
        }
        [HttpPost("SendToDoctor")]
        public async Task<IActionResult> SendToDoctor(string userId, MessageForCreationDto messageForCreationDto)
        {
            var sender = await _repo.GetByIdAsync(userId);

            messageForCreationDto.PatientId = userId;
            messageForCreationDto.ReceieverId=messageForCreationDto.DoctorId;
            messageForCreationDto.SenderId=userId;                
            var recipient = await _repo.GetByIdAsync(messageForCreationDto.DoctorId);

            if (recipient == null)
                return BadRequest("Could not find user");
            
            var message = _mapper.Map<Message>(messageForCreationDto);
           
            _messageRepo.Add(message);

            if (await _messageRepo.SaveAll())
            {
                var messageToReturn = _mapper.Map<MessageToReturnDto>(message);
                return Ok(messageToReturn);
            }

            throw new Exception("Creating the message failed on save");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, string userId)
        {
  

            var messageFromRepo = await _messageRepo.GetMessage(id);

            if (messageFromRepo.DoctorId == userId)
                messageFromRepo.DoctorDeleted = true;

            if (messageFromRepo.PatientId == userId)
                messageFromRepo.PatientDeleted = true;

            if (messageFromRepo.DoctorDeleted && messageFromRepo.PatientDeleted)
                _messageRepo.Delete(messageFromRepo);
            
            if (await _messageRepo.SaveAll())
                return NoContent();

            throw new Exception("Error deleting the message");
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(string userId, int id)
        {
 

            var message = await _messageRepo.GetMessage(id);

            if (message.DoctorId != userId)
                return Unauthorized();
             if (message.PatientId != userId)
                return Unauthorized();
            message.IsRead = true;
            message.DateRead = DateTime.Now;

            await _messageRepo.SaveAll();

            return NoContent();
        }
    }
}