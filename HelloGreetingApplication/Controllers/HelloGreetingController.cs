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
        private readonly RedisCacheService _cacheService;


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

        ///// <summary>
        ///// Save the greeting method in the database
        ///// </summary>
        ///// <param name="requestModel"></param>
        ///// <returns></returns>

        //[HttpPost("SaveGreeting")]
        //public IActionResult SaveGreetingMessage([FromBody] GetGreetingMessage getGreetingMessage)
        //{
        //    _greetingBL.SaveGreetingMessage(getGreetingMessage.Message);

        //    return Ok(new ResponseModel<string>
        //    {
        //        Success = true,
        //        Message = "Greeting message saved successfully",
        //        Data = getGreetingMessage.Message
        //    });
        //}


        ///// <summary>
        ///// Get a greeting message by ID
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet("GetGreetingById/{id}")]
        //public IActionResult GetGreetingById(int id)
        //{
        //    var greeting = _greetingBL.GetGreetingById(id);

        //    if (greeting == null)
        //    {
        //        return NotFound(new ResponseModel<string>
        //        {
        //            Success = false,
        //            Message = "Greeting message not found"
        //        });
        //    }

        //    return Ok(new ResponseModel<GetGreetingMessage>
        //    {
        //        Success = true,
        //        Message = "Greeting message retrieved successfully",
        //        Data = greeting
        //    });
        //}
        ///// <summary>
        ///// Showing All Greeting message
        ///// </summary>
        ///// <returns></returns>

        //[HttpGet("GetAllGreetings")]
        //public IActionResult GetAllGreetings()
        //{
        //    var result = _greetingBL.GetAllGreetings();

        //    if (result == null || result.Count == 0)
        //    {
        //        return NotFound(new ResponseModel<string>
        //        {
        //            Success = false,
        //            Message = "No greeting messages found"
        //        });
        //    }

        //    return Ok(new ResponseModel<List<GetGreetingMessage>>
        //    {
        //        Success = true,
        //        Message = "Greeting messages retrieved successfully",
        //        Data = result
        //    });
        //}

        ///// <summary>
        ///// Update by id 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="request"></param>
        ///// <returns></returns>


        //[HttpPut("UpdateGreeting/{id}")]
        //public IActionResult UpdateGreeting(int id, [FromBody] GetGreetingMessage request)
        //{
        //    bool isUpdated = _greetingBL.UpdateGreetingMessage(id, request.Message);

        //    if (!isUpdated)
        //    {
        //        return NotFound(new ResponseModel<string>
        //        {
        //            Success = false,
        //            Message = "Greeting message not found"
        //        });
        //    }

        //    return Ok(new ResponseModel<string>
        //    {
        //        Success = true,
        //        Message = "Greeting message updated successfully",
        //        Data = request.Message
        //    });
        //}

        ///// <summary>
        ///// Delete greeting msg by id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>

        //[HttpDelete("DeleteGreeting/{id}")]
        //public IActionResult DeleteGreeting(int id)
        //{
        //    bool isDeleted = _greetingBL.DeleteGreetingById(id);

        //    if (!isDeleted)
        //    {
        //        return NotFound(new ResponseModel<string>
        //        {
        //            Success = false,
        //            Message = "Greeting message not found"
        //        });
        //    }

        //    return Ok(new ResponseModel<string>
        //    {
        //        Success = true,
        //        Message = "Greeting message deleted successfully"
        //    });
        //}
        /// <summary>
        /// Get all greeting messages (Uses caching)
        /// </summary>
        [HttpGet("GetAllGreetings")]
        public async Task<IActionResult> GetAllGreetings()
        {
            string cacheKey = "all_greetings";
            var cachedData = await _cacheService.GetAsync<List<GetGreetingMessage>>(cacheKey);

            if (cachedData != null)
            {
                return Ok(new ResponseModel<List<GetGreetingMessage>>
                {
                    Success = true,
                    Message = "Retrieved from cache",
                    Data = cachedData
                });
            }

            var result = await _greetingBL.GetAllGreetings().ConfigureAwait(false);
            if (result == null || result.Count == 0)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "No greeting messages found"
                });
            }

            await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));

            return Ok(new ResponseModel<List<GetGreetingMessage>>
            {
                Success = true,
                Message = "Greeting messages retrieved successfully",
                Data = result
            });
        }


        /// <summary>
        /// Get a greeting message by ID (Uses caching)
        /// </summary>
        [HttpGet("GetGreetingById/{id}")]
        public async Task<IActionResult> GetGreetingById(int id)
        {
            string cacheKey = $"greeting_{id}";
            var cachedData = await _cacheService.GetAsync<GetGreetingMessage>(cacheKey);

            if (cachedData != null)
            {
                return Ok(new ResponseModel<GetGreetingMessage>
                {
                    Success = true,
                    Message = "Greeting retrieved from cache",
                    Data = cachedData
                });
            }

            var result = await _greetingBL.GetGreetingById(id).ConfigureAwait(false);
            if (result == null)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Greeting message not found"
                });
            }

            await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));

            return Ok(new ResponseModel<GetGreetingMessage>
            {
                Success = true,
                Message = "Greeting retrieved successfully",
                Data = result
            });
        }

        /// <summary>
        /// Save a new greeting message (Clears cache after saving)
        /// </summary>
        [HttpPost("SaveGreeting")]
        public async Task<IActionResult> SaveGreetingMessage([FromBody] GetGreetingMessage request)
        {
            _greetingBL.SaveGreetingMessage(request.Message);
            await _cacheService.RemoveAsync("all_greetings");

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Greeting message saved successfully",
                Data = request.Message
            });
        }

        /// <summary>
        /// Update an existing greeting message by ID (Clears cache after update)
        /// </summary>
        [HttpPut("UpdateGreeting/{id}")]
        public async Task<IActionResult> UpdateGreeting(int id, [FromBody] string newMessage)
        {
            bool isUpdated = await _greetingBL.UpdateGreetingMessage(id, newMessage);

            if (!isUpdated)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Greeting not found or update failed"
                });
            }

            await _cacheService.RemoveAsync($"greeting_{id}");
            await _cacheService.RemoveAsync("all_greetings");

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Greeting updated successfully"
            });
        }

        /// <summary>
        /// Delete a greeting message by ID (Clears cache after deletion)
        /// </summary>
        [HttpDelete("DeleteGreeting/{id}")]
        public async Task<IActionResult> DeleteGreeting(int id)
        {
            bool isDeleted = await _greetingBL.DeleteGreetingById(id).ConfigureAwait(false);

            if (!isDeleted)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Greeting message not found"
                });
            }

            await _cacheService.RemoveAsync($"greeting_{id}");
            await _cacheService.RemoveAsync("all_greetings");

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Greeting message deleted successfully"
            });
        }



    }
}
