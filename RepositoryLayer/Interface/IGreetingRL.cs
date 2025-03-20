using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        public void SaveGreetingMessage(string message);

        //GetGreetingMessage GetGreetingById(int id);

        //List<GetGreetingMessage> GetAllGreetings();

        //bool UpdateGreetingMessage(int id, string newMessage);

        //bool DeleteGreetingById(int id);

        Task<List<GetGreetingMessage>> GetAllGreetings();
        Task<GetGreetingMessage> GetGreetingById(int id);
        Task<bool> UpdateGreetingMessage(int id, string newMessage);
        Task<bool> DeleteGreetingById(int id);
    }
}
