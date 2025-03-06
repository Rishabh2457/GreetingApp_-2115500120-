using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using ModelLayer.Model;
using NLog;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service
{
    public class GreetingBL : IGreetingBL
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IGreetingRL _greetingRL;

        public GreetingBL(IGreetingRL greetingRL)
        {
            _greetingRL = greetingRL;
        }

        public string GetGreetingMessage()
        {
            return "Hello World! from business layer";
        }

        public string GetGreetingMessageUser(string? firstName, string? lastName)
        {
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                return $"Hello from, {firstName} {lastName}!";
            }
            if (!string.IsNullOrEmpty(firstName))
            {
                return $"Hello from, {firstName}!";
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                return $"Hello from, {lastName}!";
            }
            return "Hello, World!";
        }

        public void SaveGreetingMessage(string message)
        {
            _greetingRL.SaveGreetingMessage(message);
        }

        public GetGreetingMessage GetGreetingById(int id)
        {
            return _greetingRL.GetGreetingById(id);
        }

        public List<GetGreetingMessage> GetAllGreetings()
        {
            return _greetingRL.GetAllGreetings();
        }

        

    }
}
