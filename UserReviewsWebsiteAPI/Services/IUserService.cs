﻿using UserReviewsWebsiteAPI.Database.Models;
using UserReviewsWebsiteAPI.Database.Models.Dtos;

namespace UserReviewsWebsiteAPI.Services
{
    public interface IUserService
    {
        void RegisterUser(User createUser);
        string GenerateJwt(LoginDto loginUser);
        void DeleteUser(int id);
        User GetUser(int id);
        IEnumerable<User> GetUsers();
        void UpdateUser(int id, User updateUser);
    }
}