namespace Shoper.Application.Dtos.CustomerDtos;

public class CustomerDetailDto
{
    public int CustomerId { get; set; } 
    public GetByIdCustomerDto Customer { get; set; }
    //public GetByIdCartDto Cart { get; set; }
    //public List<ResultContactDto> Contacts { get; set; }
    //public List<ResultFavoritesDto> Favorites { get; set; }
    //public List<ResultHelpDto> Helps { get; set; }
    //public List<ResultOrderDto> Orders { get; set; }
    //public List<ResultSubscriberDto> Subscribe { get; set; }//
}