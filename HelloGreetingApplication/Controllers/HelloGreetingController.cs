using System.Reflection.Metadata.Ecma335;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;

namespace HelloGreetingApplication.Controllers
{

    /// <summary>
    /// Class providing API for HelloGreeting
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {

        private readonly IGreetingBL _greetingBL;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        public HelloGreetingController(IGreetingBL greetingBL)
        {
            _greetingBL = greetingBL;

        }
        



        private static Dictionary<string, string> keyValueStore = new Dictionary<string, string>();

        /// <summary>
        /// Get method to get the Greeting message
        /// </summary>
        /// <returns>"Hello, World!"</returns>
        [HttpGet]
        public IActionResult Get() 
        {

            ResponseModel<string> responseModel = new ResponseModel<string>();

            responseModel.Message = "Hello to Greeting App API endpoint Hit";
            responseModel.Success = true;
            responseModel.Data = "Hello World!";

            return Ok(responseModel);

        }
        /// <summary>
        /// Add method to add the new key value 
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>response model</returns>

        [HttpPost]
        public IActionResult Post(RequestModel requestModel)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();

            // Store the key-value pair in keyValueStore
            keyValueStore[requestModel.key] = requestModel.value;

            responseModel.Success = true;
            responseModel.Message = "Request received successfully and stored";
            responseModel.Data = $"Key: {requestModel.key}, Value: {requestModel.value}";

            return Ok(responseModel);
        }


        /// <summary>
        /// Update an existing Key value pair
        /// </summary>
        /// <param name="requestModel"></param>
        [HttpPut]
        public IActionResult Put(RequestModel requestModel)
        {
            if (keyValueStore.ContainsKey(requestModel.key))
            {
                keyValueStore[requestModel.key] = requestModel.value;

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Updated Successfully",
                    Data = $"Key: {requestModel.key}, New Value: {requestModel.value}"
                });
            }

            return NotFound(new ResponseModel<string>
            {
                Success = false,
                Message = "Key not found"
            });
        }

        /// <summary>
        /// Partially update an existing key-value pair
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>reponse model</returns>

        [HttpPatch]
        public IActionResult Patch(RequestModel requestModel)
        {
            if (keyValueStore.ContainsKey(requestModel.key))
            {
                keyValueStore[requestModel.key] = requestModel.value;

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Updated Successfully",
                    Data = $"Key: {requestModel.key}, New Value: {requestModel.value}"
                });
            }

            return NotFound(new ResponseModel<string>
            {
                Success = false,
                Message = "Key not found"
            });
        }
        /// <summary>
        /// Delete the value by id 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        [HttpDelete("{key}")]
        public IActionResult Delete(string key) 
        {
            if (keyValueStore.ContainsKey(key))

            { keyValueStore.Remove(key);
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Deleted successfully"
                });

            }
            return NotFound(new ResponseModel<string>
            {
                Success = false,
                Message = "Key not found"
            });
        }

        /// <summary>
        /// Get method for greeting from service
        /// </summary>
        /// <returns></returns>
        [HttpGet("GreetingMessage")]
        public IActionResult GetGreetingMessage()
        {
            string result = _greetingBL.GetGreetingMessage();
            var response = new ResponseModel<string>
            {
                Success = true,
                Message = $"GOt greeting message form business layer{result}",
                Data= result
            };
            return Ok(response);
        }

        /// <summary>
        /// Get method for greeting from service with optional user details
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>

        [HttpGet("GreetingMessageUser")]
        
        public IActionResult GetGreetingMessageUser(string? firstName = null,string? lastName =null)
        {
            string result = _greetingBL.GetGreetingMessageUser(firstName,lastName);
            var response = new ResponseModel<string>
            {
                Success = true,
                Message = $"Got greetings msg generated successfully",
                Data = result
            };
            return Ok(response);

        }
        
    }
}
