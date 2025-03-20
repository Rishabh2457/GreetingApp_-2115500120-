﻿using System;
using ModelLayer.DTO;
using ModelLayer.Model;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        User RegisterUser(RegisterDTO registerDTO);
        User LoginUser(LoginDTO loginDTO);
    }
}
