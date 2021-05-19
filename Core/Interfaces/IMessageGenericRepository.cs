using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IMessageGenericRepository<T>
    {
         void Add(T entity) ;
         void Delete(T entity) ;
         Task<bool> SaveAll();
         Task<Photo> GetPhoto(int id);
         Task<Photo> GetMainPhotoForUser(string userId);
         Task<Message> GetMessage(int id);
         Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
         Task<IEnumerable<Message>> GetMessageThread(string userId, string recipientId);
    }
}