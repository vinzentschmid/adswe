using DAL;
using DAL.UnitOfWork;
using DAL.UnitOfWork.impl;
using Services.Authentication;
using Services.Models;

namespace Services;

public class UserService(IUnitOfWork unitOfWork, IUserRepository repository, IAuthentication authentication) : Service<User>(unitOfWork, repository)
{
    private IUserRepository _userRepository = repository;
    private IAuthentication _authentication = authentication;

    public override Task<bool> Validate(User entry)
    {
        throw new NotImplementedException();
    }

    public override Task<ItemResponseModel<User>> Create(User entry)
    {
        throw new NotImplementedException();
    }

    public override Task<ItemResponseModel<User>> Update(string id, User entry)
    {
        throw new NotImplementedException();
    }

    public async Task<ItemResponseModel<UserResponse>> Login(LoginRequest request)
    {
        ItemResponseModel<UserResponse> response = new ItemResponseModel<UserResponse>();
        
        User user = await _userRepository.Login(request.Username, request.Password);

        if (user != null)
        {
            AuthenticationInformation authenticationInformation = await _authentication.Authenticate(user);
            if (authentication != null)
            {
                UserResponse userResponse = new UserResponse();
                userResponse.User = user;
                userResponse.AuthenticationInformation = authenticationInformation;

                response.Data = userResponse;
            }
        }
        else
        {
            response.ErrorMessages.Add("WrongPassword", "Username and password do not match.");
        }
        return response;
    }
}