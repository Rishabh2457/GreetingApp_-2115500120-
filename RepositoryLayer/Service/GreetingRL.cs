﻿using RepositoryLayer.Interface;
using ModelLayer.Model;
public class GreetingRL : IGreetingRL
{
    private readonly HelloGreetingDbContext _context;

    public GreetingRL(HelloGreetingDbContext context)
    {
        _context = context;
    }

    public void SaveGreetingMessage(string message)
    {
        var greeting = new GetGreetingMessage { Message = message };
        _context.GetGreetingMessages.Add(greeting);
        _context.SaveChanges();
    }
    public GetGreetingMessage GetGreetingById(int id)
    {
        return _context.GetGreetingMessages.FirstOrDefault(g => g.Id == id);
    }

    public List<GetGreetingMessage> GetAllGreetings()
    {
        return _context.GetGreetingMessages.ToList();
    }




}
