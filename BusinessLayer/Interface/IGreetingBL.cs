﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ModelLayer.Model;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        public string GetGreetingMessage();
        public string GetGreetingMessageUser(string? firstName, string? lastName);
        void SaveGreetingMessage(string value);

        //GetGreetingMessage GetGreetingById(int id);

        //List<GetGreetingMessage> GetAllGreetings();

       // bool UpdateGreetingMessage(int id, string newMessage);
       // bool DeleteGreetingById(int id);

        Task<List<GetGreetingMessage>> GetAllGreetings();
        Task<GetGreetingMessage> GetGreetingById(int id);
        Task<bool> UpdateGreetingMessage(int id, string newMessage);
        Task<bool> DeleteGreetingById(int id);



    }
}
