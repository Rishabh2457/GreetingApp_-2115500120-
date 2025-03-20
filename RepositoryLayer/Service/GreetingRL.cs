using RepositoryLayer.Interface;
using ModelLayer.Model;
public class GreetingRL : IGreetingRL
{
    private readonly HelloGreetingDbContext _context;
    private readonly RedisCacheService _cacheService;

    public GreetingRL(HelloGreetingDbContext context, RedisCacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }

    public void SaveGreetingMessage(string message)
    {
        var greeting = new GetGreetingMessage { Message = message };
        _context.GetGreetingMessages.Add(greeting);
        _context.SaveChanges();
    }
    //public GetGreetingMessage GetGreetingById(int id)
    //{
    //    return _context.GetGreetingMessages.FirstOrDefault(g => g.Id == id);
    //}

    //public List<GetGreetingMessage> GetAllGreetings()
    //{
    //    return _context.GetGreetingMessages.ToList();
    //}

    //public bool UpdateGreetingMessage(int id, string newMessage)
    //{
    //    var greeting = _context.GetGreetingMessages.FirstOrDefault(g => g.Id == id);

    //    if (greeting != null)
    //    {
    //        greeting.Message = newMessage;
    //        _context.SaveChanges();
    //        return true;
    //    }

    //    return false;
    //}

    //public bool DeleteGreetingById(int id)
    //{
    //    var greeting = _context.GetGreetingMessages.FirstOrDefault(g => g.Id == id);

    //    if (greeting != null)
    //    {
    //        _context.GetGreetingMessages.Remove(greeting);
    //        _context.SaveChanges();
    //        return true;
    //    }

    //    return false;
    //}

    public async Task<List<GetGreetingMessage>> GetAllGreetings()
    {
        string cacheKey = "all_greetings";
        var cachedData = await _cacheService.GetAsync<List<GetGreetingMessage>>(cacheKey);

        if (cachedData != null)
        {
            return cachedData; // Return from cache
        }

        var greetings = _context.GetGreetingMessages.ToList();
        await _cacheService.SetAsync(cacheKey, greetings, TimeSpan.FromMinutes(10)); // Cache for 10 mins

        return greetings;
    }

    public async Task<GetGreetingMessage> GetGreetingById(int id)
    {
        string cacheKey = $"greeting_{id}";
        var cachedGreeting = await _cacheService.GetAsync<GetGreetingMessage>(cacheKey);

        if (cachedGreeting != null)
        {
            return cachedGreeting;
        }

        var greeting = _context.GetGreetingMessages.FirstOrDefault(g => g.Id == id);
        if (greeting != null)
        {
            await _cacheService.SetAsync(cacheKey, greeting, TimeSpan.FromMinutes(10));
        }

        return greeting;
    }

    public async Task<bool> DeleteGreetingById(int id)
    {
        var greeting = _context.GetGreetingMessages.FirstOrDefault(g => g.Id == id);

        if (greeting != null)
        {
            _context.GetGreetingMessages.Remove(greeting);
            _context.SaveChanges();

            await _cacheService.RemoveAsync($"greeting_{id}");
            await _cacheService.RemoveAsync("all_greetings");

            return true;
        }
        return false;
    }

    public async Task<bool> UpdateGreetingMessage(int id, string newMessage)
    {
        var greeting = _context.GetGreetingMessages.FirstOrDefault(g => g.Id == id);

        if (greeting != null)
        {
            greeting.Message = newMessage;
            _context.SaveChanges();

            await _cacheService.RemoveAsync($"greeting_{id}");
            await _cacheService.RemoveAsync("all_greetings");

            return true;
        }
        return false;
    }
}





