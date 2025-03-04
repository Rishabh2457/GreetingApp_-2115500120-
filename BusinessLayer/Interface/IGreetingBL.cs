using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        public string GetGreetingMessage();
        public string GetGreetingMessageUser(string? firstName, string? lastName);
    }
}
