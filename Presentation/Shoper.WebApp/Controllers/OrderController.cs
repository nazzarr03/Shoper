using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.CartDtos;
using Shoper.Application.Dtos.CartItemDtos;
using Shoper.Application.Dtos.OrderDtos;
using Shoper.Application.Dtos.OrderItemDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Usecasess.CartItemSevices;
using Shoper.Application.Usecasess.CartServices;
using Shoper.Application.Usecasess.CustomerServices;
using Shoper.Application.Usecasess.OrderServices;
using Shoper.Application.Usecasess.ProductServices;
using Shoper.Domain.Entities;

namespace Shoper.WebApp.Controllers;

public class OrderController : Controller
{
    private readonly IOrderService _orderServices;
    private readonly ICartService _cartService;
    private readonly IProductService _productService;
    private readonly ICartItemService _cartItemService;
    private readonly IUserIdentityRepository _userIdentityRepository;
    private readonly ICustomerService _customerServices;

    public OrderController(
        IOrderService orderServices,
        ICartService cartService,
        IProductService productService,
        ICartItemService cartItemService,
        IUserIdentityRepository userIdentityRepository,
        ICustomerService customerServices)
    {
        _orderServices = orderServices;
        _cartService = cartService;
        _productService = productService;
        _cartItemService = cartItemService;
        _userIdentityRepository = userIdentityRepository;
        _customerServices = customerServices;
    }
    
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Checkout(int cartId)
    {
        if (!User.Identity.IsAuthenticated)
        {
            string cookieName = "cart";
            CreateCartDto cartItems = new CreateCartDto();
            
            if (Request.Cookies.TryGetValue(cookieName, out string cartData1))
            {
                cartItems = JsonSerializer.Deserialize<CreateCartDto>(cartData1) ?? new CreateCartDto();
            }
            
            GetByIdCartDto detailedCartItems = new GetByIdCartDto();
            detailedCartItems.CartItems = new List<ResultCartItemDto>();
            var totalSum = 0;

            foreach (var item in cartItems.CartItems)
            {
                var detailedItem = await _productService.GetByIdProductAsync(item.ProductId);
                var newproduct = new Product();
                newproduct.ProductId = detailedItem.ProductId;
                newproduct.ProductName = detailedItem.ProductName;
                newproduct.Price = detailedItem.Price;
                newproduct.ImageUrl = detailedItem.ImageUrl;
                newproduct.Description = detailedItem.Description;
                newproduct.CategoryId = detailedItem.CategoryId;
                newproduct.Stock = detailedItem.Stock;
                totalSum += item.TotalPrice;
                
                var newccartitem = new ResultCartItemDto()
                {
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,
                    TotalPrice = item.TotalPrice,
                    Product = newproduct,
                };
                
                detailedCartItems.CartItems.Add(newccartitem);
            }
            
            detailedCartItems.TotalAmount = totalSum;
            return View(detailedCartItems);
        }
        else
        {
            var value = await _cartService.GetByIdCartAsync(cartId);
            var userId = await _userIdentityRepository.GetUserIdOnAuth(User);
            var customer = await _customerServices.GetCustomerByUserId(userId);

            value.Customer = new Customer { 
                CustomerId = customer.CustomerId,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                UserId = customer.UserId
            };
            
            if (value == null)
            {
                return View();
            }
            
            return View(value);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto, int cartId)
    {
        try
        {
            var cart = new GetByIdCartDto();

            if (!User.Identity.IsAuthenticated)
            {
                string cookieName = "cart";
                CreateCartDto cartItems = new CreateCartDto();

                if (Request.Cookies.TryGetValue(cookieName, out string cartData1))
                {
                    cartItems = JsonSerializer.Deserialize<CreateCartDto>(cartData1) ?? new CreateCartDto();
                }
                
                GetByIdCartDto detailedCartItems = new GetByIdCartDto();
                detailedCartItems.CartItems = new List<ResultCartItemDto>();
                var totalSum = 0;

                foreach (var item in cartItems.CartItems)
                {
                    var detailedItem = await _productService.GetByIdProductAsync(item.ProductId);
                    var newProduct = new Product();
                    newProduct.ProductId = detailedItem.ProductId;
                    newProduct.ProductName = detailedItem.ProductName;
                    newProduct.Price = detailedItem.Price;
                    newProduct.ImageUrl = detailedItem.ImageUrl;
                    newProduct.Description = detailedItem.Description;
                    newProduct.CategoryId = detailedItem.CategoryId;
                    newProduct.Stock = detailedItem.Stock;

                    totalSum += item.TotalPrice;

                    var newcCartitem = new ResultCartItemDto()
                    {
                        Quantity = item.Quantity,
                        ProductId = item.ProductId,
                        TotalPrice = item.TotalPrice,
                        Product = newProduct,
                    };

                    detailedCartItems.CartItems.Add(newcCartitem);
                }
                
                detailedCartItems.TotalAmount = totalSum;
                detailedCartItems.UserId = "1111111111111";
                cart = detailedCartItems;
            }
            else
            {
                cart = await _cartService.GetByIdCartAsync(cartId);
            }
            
            List<CreateOrderItemDto> result = new List<CreateOrderItemDto>();

            foreach (var item in cart.CartItems)
            {
                var newOrderItem = new CreateOrderItemDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    TotalPrice = item.TotalPrice,
                };
                result.Add(newOrderItem);
            }
            
            dto.UserId = await _userIdentityRepository.GetUserIdOnAuth(User);
            dto.CustomerId =1009;
            dto.OrderItems = result;
            dto.OrderStatus = "Your Order Has Been Received";
            await _orderServices.CreateOrderAsync(dto);
            
            if(cart.CartItems != null)
            {
                foreach (var item in cart.CartItems)
                {
                    if(item.CartItemId != 0)
                    {
                        await _cartItemService.DeleteCartItemAsync(item.CartItemId);
                    }
                }
                if(cartId != 0)
                {
                    await _cartService.DeleteCartAsync(cartId);
                }
            }
            
            string cookieName1 = "cart";

            Response.Cookies.Delete(cookieName1);

            var emptyCart = new CreateCartDto
            {
                CartItems = new List<CreateCartItemDto>() 
            };

            var cartData2 = JsonSerializer.Serialize(emptyCart);
            Response.Cookies.Append(cookieName1, cartData2, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                HttpOnly = true,
                Secure = true
            });

            return Json(new
            {
                success=true
            });
        }
        catch (Exception e)
        {
            return RedirectToAction("Error","Home",e.Message);
        }
    }
    
    public async Task<ActionResult> GetCity()
    {
        var values = await _orderServices.GetAllCity();
        return Json(new
        {
            success = true, data = values
        });
    }
    public async Task<ActionResult> GetTownByCityId(int cityId)
    {
        var values = await _orderServices.GetTownByCityId(cityId);
        return Json(new
        {
            success = true,data=values
        });
    }
}