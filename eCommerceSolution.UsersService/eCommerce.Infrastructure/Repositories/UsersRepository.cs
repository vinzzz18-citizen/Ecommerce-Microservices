using Dapper;
using eCommerce.Core.DTO;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;
using Microsoft.AspNetCore.Builder;

namespace eCommerce.Infrastructure.Repositories;

internal class UsersRepository : IUsersRepository
{
  private readonly DapperDbContext _dbContext;

  public UsersRepository(DapperDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<ApplicationUser?> AddUser(ApplicationUser user)
  {
    //Generate a new unique user ID for the user
    user.UserID = Guid.NewGuid();

    // SQL Query to insert user data into the "Users" table.
    string query = "INSERT INTO public.\"Users\"(\"UserID\", \"Email\", \"PersonName\", \"Gender\", \"Password\") VALUES(@UserID, @Email, @PersonName, @Gender, @Password)";
    int rowCountAffected = await _dbContext.DbConnection.ExecuteAsync(query, user);
    
    if (rowCountAffected > 0 )
    {
      return user;
    }
    else
    {
      return null;
    }
  }

  public async Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password)
  {
    //SQL query to select a user by Email and Password
    string query = "SELECT * FROM public.\"Users\" WHERE \"Email\"=@Email AND \"Password\"=@Password";
    var parameters = new { Email = email, Password = password };

    ApplicationUser? user = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<ApplicationUser>(query, parameters);

    return user;
  }


  public async Task<ApplicationUser?> GetUserByUserID(Guid? userID)
  {
    var query = "SELECT * FROM public.\"Users\" WHERE \"UserID\" = @UserID";
    var parameters = new { UserID = userID };

    using var connection = _dbContext.DbConnection;
    return await connection.QueryFirstOrDefaultAsync<ApplicationUser>(query, parameters);
  }
}

