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

        GetGreetingMessage GetGreetingById(int id);

        List<GetGreetingMessage> GetAllGreetings();

      


    }
}
