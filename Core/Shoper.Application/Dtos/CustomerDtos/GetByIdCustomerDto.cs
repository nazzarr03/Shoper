namespace Shoper.Application.Dtos.CustomerDtos;

public class GetByIdCustomerDto
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserId { get; set; }
    public string PhoneNumber { get; set; }


    //public ICollection<Order> Orders { get; set; }
}