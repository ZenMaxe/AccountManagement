namespace AccountManagement.Domain.Interfaces;


/// <summary>
/// This interface is for Web Api Services to Check something from user.
/// </summary>
public interface IUserService
{
    Task<bool> IsUsernameFree(string username);

}