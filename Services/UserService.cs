using DAL;
using DAL.UnitOfWork;
using DAL.UnitOfWork.impl;
using DAL.Utils;
using Services.Authentication;
using Services.Models;

namespace Services;

public class UserService(IUnitOfWork unitOfWork, IUserRepository repository, IAuthentication authentication) : Service<User>(unitOfWork, repository)
{
    private readonly IUserRepository _userRepository = repository;
    private readonly IAuthentication _authentication = authentication;

    public override Task<bool> Validate(User entry)
    {
        if (entry == null)
        {
            throw new ArgumentNullException(nameof(entry), "User entry cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(entry.Email))
        {
            ValidationDictionary.AddError(nameof(entry.Email), "Username cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(entry.HashedPassword))
        {
            ValidationDictionary.AddError(nameof(entry.Password), "Password cannot be empty.");
        }

        return Task.FromResult(ValidationDictionary.IsValid);    }

    public override async Task<ItemResponseModel<User>> Create(User entry)
    {
        var response = new ItemResponseModel<User>();

        if (!await Validate(entry))
        {
            response.Success = false;
            response.ErrorMessages = ValidationDictionary.Errors;
            return response;
        }

        var user = await _userRepository.InsertOneAsync(entry);
        if (user == null)
        {
            throw new InvalidOperationException("Failed to create the user.");
        }

        var authenticationInformation = await _authentication.Authenticate(user);
        if (authenticationInformation == null)
        {
            throw new InvalidOperationException("Failed to authenticate the user.");
        }

        response.Data = user;
        response.Success = true;
        return response;
    }

    public override async Task<ItemResponseModel<User>> Update(string id, User entry)
    {
        var response = new ItemResponseModel<User>();

        if (!await Validate(entry))
        {
            response.Success = false;
            response.ErrorMessages = ValidationDictionary.Errors;
            return response;
        }

        var existingUser = await _userRepository.FindByIdAsync(id);
        if (existingUser == null)
        {
            response.Success = false;
            response.ErrorMessages.Add("NotFound", "User not found.");
            return response;
        }
        
        
        existingUser.Email = entry.Email;
        existingUser.HashedPassword = entry.Password;
        

        var updatedUser = await _userRepository.UpdateOneAsync(existingUser);
        if (updatedUser == null)
        {
            throw new InvalidOperationException("Failed to update the user.");
        }

        response.Data = updatedUser;
        response.Success = true;
        return response;
    }

    public async Task<ItemResponseModel<UserResponse>> Login(LoginRequest request)
    {
        var response = new ItemResponseModel<UserResponse>();
        
        var user = await _userRepository.Login(request.Username, request.Password);

        if (user != null)
        {
            AuthenticationInformation authenticationInformation = await _authentication.Authenticate(user);
            if (_authentication == null) return response;
            var userResponse = new UserResponse
            {
                User = user,
                AuthenticationInformation = authenticationInformation
            };

            response.Data = userResponse;
        }
        else
        {
            response.ErrorMessages.Add("WrongPassword", "Username and password do not match.");
        }
        return response;
    }
}