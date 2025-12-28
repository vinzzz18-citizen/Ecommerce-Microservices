namespace eCommerce.Core.Entities;

/// <summary>
/// Define the ApplicationUser class which acts as entity model class to store user details in data store
/// </summary>
public class ApplicationUser
{
  public Guid UserID { get; set; }
  public string? Email { get; set; }
  public string? Password { get; set; }
  public string? PersonName { get; set; }
  public string? Gender { get; set; }
}
