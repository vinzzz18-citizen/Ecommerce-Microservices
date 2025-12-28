using eCommerce.Core.DTO;

namespace eCommerce.Core.ServiceContracts;

/// <summary>
/// Contract for users service that contains use cases for users
/// </summary>
public interface IUsersService
{
  /// <summary>
  /// Method to handle user login use case and returns an AuthenticationResponse object that contains status of login
  /// </summary>
  /// <param name="loginRequest"></param>
  /// <returns></returns>
  Task<AuthenticationResponse?> Login(LoginRequest loginRequest);


  /// <summary>
  /// Method to handle user registration use case and returns an object of AuthenticationResponse type that represents status of user registration
  /// </summary>
  /// <param name="registerRequest"></param>
  /// <returns></returns>
  Task<AuthenticationResponse?> Register(RegisterRequest registerRequest);


  /// <summary>
  /// Returns UserDTO object based on the given UserID
  /// </summary>
  /// <param name="userID">UserID to search</param>
  /// <returns>UserDTO object based on the matching UserID</returns>
  Task<UserDTO> GetUserByUserID(Guid userID);
}
