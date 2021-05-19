using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class MessageGenericRepository<T> : IMessageGenericRepository<T> where T : Message
    {

     private readonly AppIdentityDbContext _context;
        public MessageGenericRepository(AppIdentityDbContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Messages.Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }


        public async Task<Photo> GetMainPhotoForUser(string userId)
        {
            return await _context.Photos.Where(u => u.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);

            return photo;
        }



    

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
           var messages = _context.Messages
                .Include(u => u.Doctor).ThenInclude(p => p.Photos)
                .Include(u => u.Patient).ThenInclude(p => p.Photos)
                .AsQueryable();

            switch (messageParams.MessageContainer)
            {
                case "DoctorInbox":
                    messages = messages.Where(u => u.DoctorId == messageParams.UserId 
                        && u.PatientDeleted == false);
                    break;
                case "DoctorOutbox":
                    messages = messages.Where(u => u.PatientId == messageParams.UserId 
                        && u.DoctorDeleted == false);
                    break;
                case "PatientInbox":
                    messages = messages.Where(u => u.PatientId == messageParams.UserId 
                        && u.DoctorDeleted == false);
                    break;
                case "PatientOutbox":
                    messages = messages.Where(u => u.PatientId == messageParams.UserId 
                        && u.PatientDeleted == false);
                    break;                    
                default:
                    messages = messages.Where(u => u.DoctorId == messageParams.UserId 
                        && u.PatientDeleted == false && u.IsRead == false ||u.PatientId == messageParams.UserId 
                        && u.DoctorDeleted == false && u.IsRead == false );
                    break;
            }

            messages = messages.OrderByDescending(d => d.MessageSent);

            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(string userId, string recipientId)
        {
            var messages = await _context.Messages
                .Include(u => u.Doctor).ThenInclude(p => p.Photos)
                .Include(u => u.Patient).ThenInclude(p => p.Photos)
                .Where(m => m.DoctorId == userId && m.DoctorDeleted == false 
                    && m.PatientId == recipientId 
                    || m.DoctorId == recipientId && m.PatientId == userId 
                    && m.PatientDeleted == false)
                .OrderByDescending(m => m.MessageSent)
                .ToListAsync();

            return messages;
        }



    }



}