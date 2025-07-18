using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.CartDtos;
using Shoper.Application.Dtos.CartItemDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Usecasess.CartItemSevices;
using Shoper.Application.Usecasess.CartServices;
using Shoper.Application.Usecasess.ProductServices;
using Shoper.Domain.Entities;

namespace Shoper.WebApp.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartservices;
    private readonly ICartItemService _cartitemservice;
    private readonly IProductService _productservice;
    private readonly IUserIdentityRepository _useridentityrepository;

    public CartController(
        ICartService cartservices,
        ICartItemService cartitemservice,
        IProductService productservice,
        IUserIdentityRepository useridentityrepository)
    {
        _cartservices = cartservices;
        _cartitemservice = cartitemservice;
        _productservice = productservice;
        _useridentityrepository = useridentityrepository;
    }

    public async Task<IActionResult> Index()
    {
        if (!User.Identity.IsAuthenticated)
            // Kullanıcı girişi yoksa sepeti cookie üzzerinden çekiyoruz
        {
            if (Request.Cookies.TryGetValue("cart", out string cartData))
                // Cookie'de olan Json stringini dto'ya deserialize yapıyoruz
                // deserialize; json verilerini c# nesnelerine dönüştürür
            {
                string cookieName = "cart";
                CreateCartDto cartItems = new CreateCartDto();
                
                if (Request.Cookies.TryGetValue(cookieName, out string cartData1))
                {
                    cartItems = JsonSerializer.Deserialize<CreateCartDto>(cartData1) ?? new CreateCartDto();
                }
                
                GetByIdCartDto detailedCartItems = new GetByIdCartDto();
                detailedCartItems.CartItems = new List<ResultCartItemDto>();
                var totalsum = 0;

                foreach (var item in cartItems.CartItems)
                {
                    var detailedItem = await _productservice.GetByIdProductAsync(item.ProductId);
                    var newProduct = new Product();
                    newProduct.ProductId = detailedItem.ProductId;
                    newProduct.ProductName = detailedItem.ProductName;
                    newProduct.Price = detailedItem.Price;
                    newProduct.Description = detailedItem.Description;
                    newProduct.ImageUrl = detailedItem.ImageUrl;
                    newProduct.CategoryId = detailedItem.CategoryId;
                    newProduct.Stock = detailedItem.Stock;
                    
                    totalsum += item.TotalPrice;

                    var newCartItem = new ResultCartItemDto
                    {
                        Product = newProduct,
                        Quantity = item.Quantity,
                        ProductId = item.ProductId,
                        TotalPrice = item.TotalPrice,
                    };
                    
                    detailedCartItems.CartItems.Add(newCartItem);
                }
                
                detailedCartItems.TotalAmount = totalsum;
                return View(detailedCartItems);
            }
            
            return View();
        }
        else
        {
            var userId = await _useridentityrepository.GetUserIdOnAuth(User);
            var cart = await _cartservices.GetByUserIdCartAsync(userId);
            return View(cart);
        }
    }

    [HttpPost]
    public async Task<JsonResult> AddToCartItem([FromBody] CreateCartItemDto dto)
    {
        try
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = await _useridentityrepository.GetUserIdOnAuth(User);
                var checkCart = await _cartservices.CheckCartAsync(userId);
                if (checkCart)
                {
                    var cart = await _cartservices.GetByUserIdCartAsync(userId);
                    var check = await _cartitemservice.CheckCartItems(cart.CartId, dto.ProductId);
                    if (check)
                    {
                        await _cartitemservice.UpdateQuantity(cart.CartId, dto.ProductId, dto.Quantity);
                    }
                    else
                    {
                        dto.CartId = cart.CartId;
                        await _cartitemservice.CreateCartItemAsync(dto);
                    }
                    
                    var sumPrice = cart.TotalAmount + dto.TotalPrice;
                    await _cartservices.UpdateTotalAmount(cart.CartId, sumPrice);
                }
                else
                {
                    var newCart = new CreateCartDto
                    {
                        CreatedDate = DateTime.Now,
                        UserId = userId,
                        CartItems = new List<CreateCartItemDto>(),
                    };
                    await _cartservices.CreateCartAsync(newCart);
                    var cart = await _cartservices.GetByUserIdCartAsync(userId);
                    var check = await _cartitemservice.CheckCartItems(cart.CartId, dto.ProductId);
                    if (check)
                    {
                        await _cartitemservice.UpdateQuantity(cart.CartId, dto.ProductId, dto.Quantity);
                    }
                    else
                    {
                        dto.CartId = cart.CartId;
                        await _cartitemservice.CreateCartItemAsync(dto);
                    }
                    
                    var sumPrice = cart.TotalAmount + dto.TotalPrice;
                    await _cartservices.UpdateTotalAmount(cart.CartId, sumPrice);
                }
            }
            else
            {
                string cookieName = "cart";
                CreateCartDto cartItems;
                if (Request.Cookies.TryGetValue(cookieName, out string cartData))
                {
                    cartItems = JsonSerializer.Deserialize<CreateCartDto>(cartData) ?? new CreateCartDto();
                }
                else
                {
                    cartItems = new CreateCartDto
                    {
                        CartItems = new List<CreateCartItemDto>(),
                    };
                }
                
                var existingItem = cartItems.CartItems.FirstOrDefault(x => x.ProductId == dto.ProductId);
                if (existingItem != null)
                {
                    // Ürün zaten varsa quantity değeri artıyor
                    existingItem.Quantity = dto.Quantity;
                    existingItem.TotalPrice = dto.TotalPrice;
                }
                else 
                {
                    // Ürün yoksa yeni ürünü listeye ekle
                    cartItems.CartItems.Add(dto);
                }
                
                var updatedCartData = JsonSerializer.Serialize(cartItems);
                Response.Cookies.Append(cookieName, updatedCartData, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
            }
            
            return Json(new
            {
                success = true
            });
        }
        catch (Exception e)
        {
            return Json(new
            {
                error = e
            });
        }
    }

    [HttpGet]
    public async Task<JsonResult> DeleteCartItem(int id, int productId)
    {
        try
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == 0)
                {
                    return Json(new
                    {
                        error = "Product does not exist"
                    });
                }

                var cartItem = await _cartitemservice.GetByIdCartItemAsync(id);
                if (cartItem == null)
                {
                    return Json(new
                    {
                        error = "Product does not exist"
                    });
                }
                
                await _cartitemservice.DeleteCartItemAsync(cartItem.CartId);
                var cart = await _cartservices.GetByIdCartAsync(cartItem.CartId);
                var tempCartTotal = cart.TotalAmount + cartItem.TotalPrice;
                await _cartservices.UpdateTotalAmount(cart.CartId, tempCartTotal);
            }
            else
            {
                string cookieName = "cart";
                CreateCartDto cartItems;

                if (Request.Cookies.TryGetValue(cookieName, out string cartData))
                {
                    cartItems = JsonSerializer.Deserialize<CreateCartDto>(cartData) ?? new CreateCartDto();
                }
                else
                {
                    cartItems = new CreateCartDto
                    {
                        CartItems = new List<CreateCartItemDto>(),
                    };
                }
                
                var existingItem = cartItems.CartItems.FirstOrDefault(x => x.ProductId == id);
                if (existingItem != null)
                {
                    cartItems.CartItems.Remove(existingItem);
                }
                else
                {
                    cartItems.CartItems.Add(new CreateCartItemDto());
                }
                
                var updatedCartData = JsonSerializer.Serialize(cartItems);
                Response.Cookies.Append(cookieName, updatedCartData, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
            }
            
            return Json(new
            {
                success = true
            });
        }
        catch (Exception e)
        {
            return Json(new
            {
                error = e
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantityOnCart(UpdateCartItemDto dto)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var cart = await _cartservices.GetByIdCartAsync(dto.CartId);
                    await _cartitemservice.UpdateQuantity(dto.CartId, dto.ProductId, dto.Quantity);
                    var cartItem = await _cartitemservice.GetByIdCartItemAsync(dto.CartItemId);
                    var product = await _productservice.GetByIdProductAsync(dto.ProductId);
                    decimal sumPrice = cart.TotalAmount;
                    
                    if (cartItem.Quantity == 0)
                    {
                        await _cartitemservice.DeleteCartItemAsync(cartItem.CartItemId);
                    }
                    if (dto.Quantity > 0)
                    {
                        sumPrice = cart.TotalAmount + product.Price;

                    }
                    else
                    {
                        sumPrice = cart.TotalAmount - product.Price;
                    }
                    
                    await _cartservices.UpdateTotalAmount(cart.CartId, sumPrice);
                }
                else
                {
                    string cookieName = "cart";
                    CreateCartDto cartItems;

                    if (Request.Cookies.TryGetValue(cookieName, out string cartData))
                    {
                        cartItems = JsonSerializer.Deserialize<CreateCartDto>(cartData) ?? new CreateCartDto();
                    }
                    else
                    {
                        cartItems = new CreateCartDto
                        {
                            CartItems = new List<CreateCartItemDto>()
                        };
                    }

                    var existingItem = cartItems.CartItems.FirstOrDefault(item => item.ProductId == dto.ProductId);
                    var cookieProduct = await _productservice.GetByIdProductAsync(dto.ProductId);
                    if (existingItem != null)
                    {
                        if (dto.Quantity > 0)
                        {
                            existingItem.Quantity += dto.Quantity;
                            existingItem.TotalPrice = Convert.ToInt32(existingItem.Quantity*cookieProduct.Price);
                        }
                        else
                        {
                            existingItem.Quantity += dto.Quantity;
                            existingItem.TotalPrice = Convert.ToInt32(existingItem.Quantity * cookieProduct.Price);
                            if (existingItem.TotalPrice == 0)
                            {
                                cartItems.CartItems.Remove(existingItem);
                            }
                        }
                    }
                    else
                    {
                        cartItems.CartItems.Add(new CreateCartItemDto());
                    }


                    var updatedCartData = JsonSerializer.Serialize(cartItems);
                    Response.Cookies.Append(cookieName, updatedCartData, new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(7),
                        HttpOnly = true,
                        Secure = true
                    });
                }
                   
                return Json(new
                {
                    success = true
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    error = e
                });
            }
        }
}