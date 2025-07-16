using Shoper.Application.Dtos.CategoryDtos;
using Shoper.Application.Dtos.CityDtos;
using Shoper.Application.Dtos.DashboardDtos;
using Shoper.Application.Dtos.OrderDtos;
using Shoper.Application.Dtos.OrderItemDtos;
using Shoper.Application.Dtos.ProductDtos;
using Shoper.Application.Dtos.TownDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Interfaces.IOrderRepository;
using Shoper.Domain.Entities;

namespace Shoper.Application.Usecasess.OrderServices;

// Bazı fonksiyonlarda çok fazla database sorgusu attık bu sorunu daha sonra çözücem

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _repository;
    private readonly IOrdersRepository _ordersRepository;
    private readonly IRepository<OrderItem> _repositoryOrderItem;
    private readonly IRepository<Customer> _repositoryCustomer;
    private readonly IRepository<Product> _repositoryProduct;
    private readonly IRepository<Category> _repositoryCategory;
    private readonly IRepository<CartItem> _repositoryCartItem;

    public OrderService
    (
        IRepository<Order> repository,
        IOrdersRepository ordersRepository,
        IRepository<OrderItem> repositoryOrderItem,
        IRepository<Customer> repositoryCustomer,
        IRepository<Product> repositoryProduct,
        IRepository<Category> repositoryCategory,
        IRepository<CartItem> repositoryCartItem
        )
    {
        _repository = repository;
        _ordersRepository = ordersRepository;
        _repositoryOrderItem = repositoryOrderItem;
        _repositoryCustomer = repositoryCustomer;
        _repositoryProduct = repositoryProduct;
        _repositoryCategory = repositoryCategory;
        _repositoryCartItem = repositoryCartItem;
    }


    public async Task CreateOrderAsync(CreateOrderDto model)
    {
        decimal sum = 0;
        var newOrder = new Order
        {
            OrderDate = DateTime.Now,
            // burada sum 0 çünkü daha item içindeki ürünleri eklemedik
            TotalAmount = sum,
            OrderStatus = model.OrderStatus,
            ShippingAdress = model.ShippingAdress,
            ShippingCityId = model.ShippingCityId,
            ShippingTownId = model.ShippingTownId,
            CustomerId = model.CustomerId,
            CustomerName = model.CustomerName,
            CustomerSurname = model.CustomerSurname,
            CustomerEmail = model.CustomerEmail,
            CustomerPhone = model.CustomerPhone,
            UserId = model.UserId,
        };
        // burada hemen create yapma sebebimiz oluşan orderId'yi itemler ile ilişkilendirmek
        await _repository.CreateAsync(newOrder);
        
        // order item tek tek alınıp order item tablosuna kaydediliyor
        foreach (var item in model.OrderItems)
        {
            await _repositoryOrderItem.CreateAsync(new OrderItem
            {
                OrderId = newOrder.OrderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                TotalPrice = item.TotalPrice,
            });
            
            sum += sum + item.TotalPrice;
        }
        
        newOrder.TotalAmount = sum;
        await _repository.UpdateAsync(newOrder);
    }
    
    public async Task DeleteOrderAsync(int id)
    {
        var foundOrder = await _repository.GetByIdAsync(id);
        
        foreach (var item in foundOrder.OrderItems)
        {
            var orderItem = await _repositoryOrderItem.GetByIdAsync(item.OrderItemId);
            await _repositoryOrderItem.DeleteAsync(orderItem);
        }
        
        await _repository.DeleteAsync(foundOrder);
    }
    
    public async Task<List<ResultCityDto>> GetAllCity()
    {
        var cities = await _ordersRepository.GetCity();
        return cities.Select(c => new ResultCityDto
        {
            Id = c.Id,
            Cityname = c.CityName,
            CityId = c.CityId
        }).ToList();
    }
    
     public async Task<List<ResultOrderDto>> GetAllOrderAsync()
        {
            var orders = await _repository.GetAllAsync();
            var orderitem = await _repositoryOrderItem.GetAllAsync();
            var result = new List<ResultOrderDto>();
            foreach (var item in orders)
            {
                 var ordercustomer = await _repositoryCustomer.GetByIdAsync(item.CustomerId);
                var orderdto = new ResultOrderDto
                {
                    OrderId = item.OrderId,
                    OrderDate = item.OrderDate,
                    TotalAmount = item.TotalAmount,
                    OrderStatus = item.OrderStatus,
                    //BillingAdress = item.BillingAdress,
                    ShippingAdress = item.ShippingAdress,
                    ShippingCityId = item.ShippingCityId,
                    ShippingTownId = item.ShippingTownId,
                    //PaymentMethod = item.PaymentMethod,
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    CustomerSurname = item.CustomerSurname,
                    CustomerEmail = item.CustomerEmail,
                    CustomerPhone = item.CustomerPhone,
                    OrderItems = new List<ResultOrderItemDto>(),
                    UserId = item.UserId,
				};
                foreach (var item1 in item.OrderItems)
                {
                    var orderItemProduct = await _repositoryProduct.GetByIdAsync(item1.ProductId);
                    var orderItemDto = new ResultOrderItemDto
                    {
						OrderId = item1.OrderId,
						ProductId = item1.ProductId,
						Quantity = item1.Quantity,
						TotalPrice = item1.TotalPrice,
						OrderItemId = item1.OrderItemId,
                        Product = orderItemProduct,
                       
					};
                    orderdto.OrderItems.Add(orderItemDto);
                }
                result.Add(orderdto);
            }
            return result;
        }

        public async Task<GetByIdOrderDto> GetByIdOrderAsync(int id)
        {
            var orders = await _repository.GetByIdAsync(id);
            var ordercustomer = await _repositoryCustomer.GetByIdAsync(orders.CustomerId);
            var orderitemsrepo = await _repositoryOrderItem.WhereAsync(x => x.OrderId == id);
            var result = new GetByIdOrderDto 
            {
                OrderId = orders.OrderId,
                OrderDate = orders.OrderDate,
                TotalAmount = orders.TotalAmount,
                OrderStatus = orders.OrderStatus,
                //BillingAdress = orders.BillingAdress,
                ShippingAdress = orders.ShippingAdress,
                ShippingCityId = orders.ShippingCityId,
                ShippingTownId = orders.ShippingTownId,
                //PaymentMethod = orders.PaymentMethod,
                CustomerId = orders.CustomerId,
                CustomerName = orders.CustomerName,
                CustomerSurname = orders.CustomerSurname,
                CustomerEmail = orders.CustomerEmail,
                CustomerPhone = orders.CustomerPhone,
                OrderItems = new List<ResultOrderItemDto>(),
                UserId = orders.UserId,
            };
            foreach (var item in orderitemsrepo)
            {
                var orderItemProduct = await _repositoryProduct.GetByIdAsync(item.ProductId);
                var orderItemDto = new ResultOrderItemDto
                {
					OrderId = item.OrderId,
					ProductId = item.ProductId,
					Quantity = item.Quantity,
					TotalPrice = item.TotalPrice,
					OrderItemId = item.OrderItemId,
					Product = orderItemProduct,
				};
                result.OrderItems.Add(orderItemDto);
            }
            return result;
        }

        public async Task<DashboardCardsDto> GetDashboardCards()
        {
            var result = new DashboardCardsDto();

            var totalorder = await _repository.GetAllAsync();
            result.TotalOrders = totalorder.Count();

            var totalcustomer = await _repositoryCustomer.GetAllAsync();
            result.TotalCustomers = totalcustomer.Count();

            var totalcart = await _repositoryCartItem.GetAllAsync();
            result.TotalCartItemsProducts = totalcart.GroupBy(y => y.ProductId).Select(x => x.Key).Count();

            var crisiticstock = await _repositoryProduct.GetAllAsync();
            result.CriticStockProducts = crisiticstock.Where(x => x.Stock < 10).Count();

            return result;
        }

        public async Task<List<SalesWithCategory>> GetOrderByCategory()
        {
            var orders = await _repository.GetAllAsync();
            var orderitem = await _repositoryOrderItem.GetAllAsync();
            var result = new List<DashboardOrderDto>();
            foreach (var item in orders)
            {
                var ordercustomer = await _repositoryCustomer.GetByIdAsync(item.CustomerId);
                var orderdto = new DashboardOrderDto
                {
                    OrderId = item.OrderId,
                    OrderDate = item.OrderDate,
                    TotalAmount = item.TotalAmount,
                    OrderStatus = item.OrderStatus,
                    //BillingAdress = item.BillingAdress,
                    ShippingAdress = item.ShippingAdress,
                    ShippingCityId = item.ShippingCityId,
                    ShippingTownId = item.ShippingTownId,
                    //PaymentMethod = item.PaymentMethod,
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    CustomerSurname = item.CustomerSurname,
                    CustomerEmail = item.CustomerEmail,
                    CustomerPhone = item.CustomerPhone,
                    OrderItems = new List<DashboardOrderItemDto>(),
                    UserId = item.UserId,
                };
                foreach (var item1 in item.OrderItems)
                {
                    var orderItemProduct = await _repositoryProduct.GetByIdAsync(item1.ProductId);
                    var category = await _repositoryCategory.GetByIdAsync(orderItemProduct.CategoryId);
                    var newcategory = new GetByIdCategoryDto
                    {
                        CategoryId = category.CategoryId,
                        CategoryName = category.CategoryName,
                    };
                    var dashboardproduct = new DashboardProductDto
                    {
                        ProductId = orderItemProduct.ProductId,
                        ProductName = orderItemProduct.ProductName,
                        Description = orderItemProduct.Description,
                        Price = orderItemProduct.Price,
                        Stock = orderItemProduct.Stock,
                        ImageUrl = orderItemProduct.ImageUrl,
                        CategoryId = orderItemProduct.CategoryId,
                        Category = newcategory,
                    };
                    var orderItemDto = new DashboardOrderItemDto
                    {
                        OrderId = item1.OrderId,
                        ProductId = item1.ProductId,
                        Quantity = item1.Quantity,
                        TotalPrice = item1.TotalPrice,
                        OrderItemId = item1.OrderItemId,
                        Product = dashboardproduct,

                    };
                    orderdto.OrderItems.Add(orderItemDto);
                }
                result.Add(orderdto);
            }
            var result1 = result
                .SelectMany(x => x.OrderItems)
                .GroupBy(y => y.Product.Category.CategoryName)
                .Select(z => new SalesWithCategory
                {
                    Categoryname = z.Key,
                    Totalsales = z.Sum(y => y.TotalPrice)
                })
                .OrderByDescending( a => a.Totalsales).ToList();

            return result1;
        }

        public async Task<List<ResultOrderDto>> GetOrderByUserId(string userId)
        {
            var orders = await _repository.WhereAsync(x => x.UserId == userId);
            var orderitem = await _repositoryOrderItem.GetAllAsync();
            var result = new List<ResultOrderDto>();
            foreach (var item in orders)
            {
                var ordercustomer = await _repositoryCustomer.GetByIdAsync(item.CustomerId);
                var orderdto = new ResultOrderDto
                {
                    OrderId = item.OrderId,
                    OrderDate = item.OrderDate,
                    TotalAmount = item.TotalAmount,
                    OrderStatus = item.OrderStatus,
                    //BillingAdress = item.BillingAdress,
                    ShippingAdress = item.ShippingAdress,
                    ShippingCityId = item.ShippingCityId,
                    ShippingTownId = item.ShippingTownId,
                    //PaymentMethod = item.PaymentMethod,
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    CustomerSurname = item.CustomerSurname,
                    CustomerEmail = item.CustomerEmail,
                    CustomerPhone = item.CustomerPhone,
                    OrderItems = new List<ResultOrderItemDto>(),
                    UserId = item.UserId,
                };
                foreach (var item1 in item.OrderItems)
                {
                    var orderItemProduct = await _repositoryProduct.GetByIdAsync(item1.ProductId);
                    var orderItemDto = new ResultOrderItemDto
                    {
                        OrderId = item1.OrderId,
                        ProductId = item1.ProductId,
                        Quantity = item1.Quantity,
                        TotalPrice = item1.TotalPrice,
                        OrderItemId = item1.OrderItemId,
                        Product = orderItemProduct,

                    };
                    orderdto.OrderItems.Add(orderItemDto);
                }
                result.Add(orderdto);
            }
            return result;
        }

        public async Task<List<DashboardOrderStatusDto>> GetOrderStatusGraphs()
        {
            var orders = await _repository.GetAllAsync();
            var orderitem = await _repositoryOrderItem.GetAllAsync();
            var result = new List<DashboardOrderDto>();
            foreach (var item in orders)
            {
                var ordercustomer = await _repositoryCustomer.GetByIdAsync(item.CustomerId);
                var orderdto = new DashboardOrderDto
                {
                    OrderId = item.OrderId,
                    OrderDate = item.OrderDate,
                    TotalAmount = item.TotalAmount,
                    OrderStatus = item.OrderStatus,
                    //BillingAdress = item.BillingAdress,
                    ShippingAdress = item.ShippingAdress,
                    ShippingCityId = item.ShippingCityId,
                    ShippingTownId = item.ShippingTownId,
                    //PaymentMethod = item.PaymentMethod,
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    CustomerSurname = item.CustomerSurname,
                    CustomerEmail = item.CustomerEmail,
                    CustomerPhone = item.CustomerPhone,
                    OrderItems = new List<DashboardOrderItemDto>(),
                    UserId = item.UserId,
                };
                foreach (var item1 in item.OrderItems)
                {
                    var orderItemProduct = await _repositoryProduct.GetByIdAsync(item1.ProductId);
                    var category = await _repositoryCategory.GetByIdAsync(orderItemProduct.CategoryId);
                    var newcategory = new GetByIdCategoryDto
                    {
                        CategoryId = category.CategoryId,
                        CategoryName = category.CategoryName,
                    };
                    var dashboardproduct = new DashboardProductDto
                    {
                        ProductId = orderItemProduct.ProductId,
                        ProductName = orderItemProduct.ProductName,
                        Description = orderItemProduct.Description,
                        Price = orderItemProduct.Price,
                        Stock = orderItemProduct.Stock,
                        ImageUrl = orderItemProduct.ImageUrl,
                        CategoryId = orderItemProduct.CategoryId,
                        Category = newcategory,
                    };
                    var orderItemDto = new DashboardOrderItemDto
                    {
                        OrderId = item1.OrderId,
                        ProductId = item1.ProductId,
                        Quantity = item1.Quantity,
                        TotalPrice = item1.TotalPrice,
                        OrderItemId = item1.OrderItemId,
                        Product = dashboardproduct,

                    };
                    orderdto.OrderItems.Add(orderItemDto);
                }
                result.Add(orderdto);
            }
            var result1 = result
                .GroupBy(y => y.OrderStatus)
                .Select(z => new DashboardOrderStatusDto
                {
                    Status = z.Key,
                    Count = z.Key.Count()
                }).ToList();

            return result1;
        }

        public async Task<List<SalesTrendDto>> GetSalesTrends()
        {
            var orders = await _repository.GetAllAsync();
            var orderitem = await _repositoryOrderItem.GetAllAsync();
            var result = new List<DashboardOrderDto>();
            foreach (var item in orders)
            {
                var ordercustomer = await _repositoryCustomer.GetByIdAsync(item.CustomerId);
                var orderdto = new DashboardOrderDto
                {
                    OrderId = item.OrderId,
                    OrderDate = item.OrderDate,
                    TotalAmount = item.TotalAmount,
                    OrderStatus = item.OrderStatus,
                    //BillingAdress = item.BillingAdress,
                    ShippingAdress = item.ShippingAdress,
                    ShippingCityId = item.ShippingCityId,
                    ShippingTownId = item.ShippingTownId,
                    //PaymentMethod = item.PaymentMethod,
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    CustomerSurname = item.CustomerSurname,
                    CustomerEmail = item.CustomerEmail,
                    CustomerPhone = item.CustomerPhone,
                    OrderItems = new List<DashboardOrderItemDto>(),
                    UserId = item.UserId,
                };
                foreach (var item1 in item.OrderItems)
                {
                    var orderItemProduct = await _repositoryProduct.GetByIdAsync(item1.ProductId);
                    var category = await _repositoryCategory.GetByIdAsync(orderItemProduct.CategoryId);
                    var newcategory = new GetByIdCategoryDto
                    {
                        CategoryId = category.CategoryId,
                        CategoryName = category.CategoryName,
                    };
                    var dashboardproduct = new DashboardProductDto
                    {
                        ProductId = orderItemProduct.ProductId,
                        ProductName = orderItemProduct.ProductName,
                        Description = orderItemProduct.Description,
                        Price = orderItemProduct.Price,
                        Stock = orderItemProduct.Stock,
                        ImageUrl = orderItemProduct.ImageUrl,
                        CategoryId = orderItemProduct.CategoryId,
                        Category = newcategory,
                    };
                    var orderItemDto = new DashboardOrderItemDto
                    {
                        OrderId = item1.OrderId,
                        ProductId = item1.ProductId,
                        Quantity = item1.Quantity,
                        TotalPrice = item1.TotalPrice,
                        OrderItemId = item1.OrderItemId,
                        Product = dashboardproduct,

                    };
                    orderdto.OrderItems.Add(orderItemDto);
                }
                result.Add(orderdto);
            }
            var result1 = result
                .GroupBy(y => y.OrderDate.ToString("yyyy-MM"))
                .Select(z => new SalesTrendDto
                {
                    Months = z.Key.Replace(".",""), 
                    SalesCount = z.Sum(y => y.OrderItems.Sum(x => x.Quantity)).ToString(),
                    TotalAmount = z.Sum(y => y.TotalAmount).ToString().Replace(",","."),
                })
                .OrderBy(a => a.Months).ToList();

            return result1;
        }

        public async Task<List<DashboardSoledProductDto>> GetSoledProducts()
        {
            var orders = await _repository.GetAllAsync();
            var orderitem = await _repositoryOrderItem.GetAllAsync();
            var result = new List<DashboardOrderDto>();
            foreach (var item in orders)
            {
                var ordercustomer = await _repositoryCustomer.GetByIdAsync(item.CustomerId);
                var orderdto = new DashboardOrderDto
                {
                    OrderId = item.OrderId,
                    OrderDate = item.OrderDate,
                    TotalAmount = item.TotalAmount,
                    OrderStatus = item.OrderStatus,
                    //BillingAdress = item.BillingAdress,
                    ShippingAdress = item.ShippingAdress,
                    ShippingCityId = item.ShippingCityId,
                    ShippingTownId = item.ShippingTownId,
                    //PaymentMethod = item.PaymentMethod,
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    CustomerSurname = item.CustomerSurname,
                    CustomerEmail = item.CustomerEmail,
                    CustomerPhone = item.CustomerPhone,
                    OrderItems = new List<DashboardOrderItemDto>(),
                    UserId = item.UserId,
                };
                foreach (var item1 in item.OrderItems)
                {
                    var orderItemProduct = await _repositoryProduct.GetByIdAsync(item1.ProductId);
                    var category = await _repositoryCategory.GetByIdAsync(orderItemProduct.CategoryId);
                    var newcategory = new GetByIdCategoryDto
                    {
                        CategoryId = category.CategoryId,
                        CategoryName = category.CategoryName,
                    };
                    var dashboardproduct = new DashboardProductDto
                    {
                        ProductId = orderItemProduct.ProductId,
                        ProductName = orderItemProduct.ProductName,
                        Description = orderItemProduct.Description,
                        Price = orderItemProduct.Price,
                        Stock = orderItemProduct.Stock,
                        ImageUrl = orderItemProduct.ImageUrl,
                        CategoryId = orderItemProduct.CategoryId,
                        Category = newcategory,
                    };
                    var orderItemDto = new DashboardOrderItemDto
                    {
                        OrderId = item1.OrderId,
                        ProductId = item1.ProductId,
                        Quantity = item1.Quantity,
                        TotalPrice = item1.TotalPrice,
                        OrderItemId = item1.OrderItemId,
                        Product = dashboardproduct,

                    };
                    orderdto.OrderItems.Add(orderItemDto);
                }
                result.Add(orderdto);
            }
            var result1 = result
                .SelectMany(x => x.OrderItems)
                .GroupBy(y => y.Product.ProductName)
                .Select(z => new DashboardSoledProductDto
                {
                    ProductName = z.Key,
                    TotalSoled = z.Sum(y => y.Quantity)
                })
                .OrderByDescending(a => a.TotalSoled).Take(5).ToList();

            return result1;
        }

        public async Task<List<ResultTownDto>> GetTownByCityId(int cityId)
        {
            var values = await _ordersRepository.GetTownByCityId(cityId);
            return values.Select(x => new ResultTownDto
            {
                Id = x.Id,
                CityId = x.CityId,
                TownId = x.TownId,
                TownName = x.TownName,
            }).ToList();
        }

        public async Task UpdateOrderAsync(UpdateOrderDto model)
        {
            var order = await _repository.GetByIdAsync(model.OrderId);
            var orderitems = await _repositoryOrderItem.GetAllAsync();
            order.OrderStatus = model.OrderStatus;
            decimal sum = 0;
            foreach (var item in model.OrderItems)
            {

                foreach (var item1 in order.OrderItems)
                {
                    var orderItemDto = await _repositoryOrderItem.GetByIdAsync(item1.OrderItemId);
                    if(item.OrderItemId == item1.OrderItemId)
                    {
                        orderItemDto.Quantity = item.Quantity;
                        orderItemDto.TotalPrice = item.TotalPrice;
                    }
                    sum = sum + item1.TotalPrice;
                }

            }
            order.TotalAmount = sum;

            await _repository.UpdateAsync(order);
        }

        public async Task UpdateOrderStatus(int orderId, string orderstatus)
        {
            var order = await _repository.GetByIdAsync(orderId);
            order.OrderStatus = orderstatus;
            await _repository.UpdateAsync(order);
        }
    
}