using System.Linq.Expressions;
using Shoper.Application.Dtos.CartDtos;
using Shoper.Application.Dtos.CartItemDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Interfaces.ICartsRepository;
using Shoper.Domain.Entities;

namespace Shoper.Application.Usecasess.CartServices;

public class CartService : ICartService
{
    private readonly IRepository<Cart> _repository;
    private readonly IRepository<CartItem> _itemRepository;
    private readonly IRepository<Customer> _customerRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly ICartsRepository _cartsRepository;

    public CartService(
        IRepository<Cart> repository,
        IRepository<CartItem> itemRepository,
        IRepository<Customer> customerRepository,
        IRepository<Product> productRepository,
        ICartsRepository cartsRepository)
    {
        _repository = repository;
        _itemRepository = itemRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
        _cartsRepository = cartsRepository;
    }
    
    // userId ait cart var mı diye check ediyoruz
    public async Task<bool> CheckCartAsync(string userId)
    {
        var cart = await _repository.FirstOrDefaultAsync(cart => cart.UserId == userId);
        return cart != null;
    }
    
    public async Task CreateCartAsync(CreateCartDto model)
    {
        var cart = new Cart
        {
            CreatedDate = DateTime.Now,
            CustomerId = model.CustomerId,
            UserId = model.UserId,
        };
        await _repository.CreateAsync(cart);
        var sum = 0;
        foreach (var cartItem in model.CartItems)
        {
            var newCartItem = new CartItem
            {
                CartId = cart.CartId,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                TotalPrice = cartItem.TotalPrice,
            };
            sum = sum + (cartItem.TotalPrice);
            await _itemRepository.CreateAsync(newCartItem);
        }
        
        cart.TotalAmount = sum;
        await _repository.UpdateAsync(cart);
    }
    
    public async Task DeleteCartAsync(int id)
    {
        var cart = await _repository.GetByIdAsync(id);
        var cartItems = await _itemRepository.GetAllAsync();
        foreach (var item in cartItems)
        {
            if (item.CartId == id)
            {
                var foundCartItem = await _itemRepository.GetByIdAsync(item.CartItemId);
                await _itemRepository.DeleteAsync(foundCartItem);
            }
        }
        await _repository.DeleteAsync(cart);
    }
    
    public async Task<List<AdminCartDto>> GetAllAdminCartAsync()
    {
        var carts = await _repository.GetAllAsync();
        var cartItems = await _itemRepository.GetAllAsync();
        var products = await _productRepository.GetAllAsync();
        var result = new List<AdminCartDto>();

        foreach (var item in carts)
        {
            var customerDto = await _customerRepository.GetByFilterAsync(cus => cus.UserId == item.UserId);
            var cartDto = new AdminCartDto
            {
                CartId = item.CartId,
                CreatedDate = item.CreatedDate,
                TotalAmount = item.TotalAmount,
                UserId = item.UserId,
                NameSurname = customerDto.FirstName + " " + customerDto.LastName,
                CartItems = new List<ResultCartItemDto>()
            };

            if (item.CartItems != null)
            {
                foreach (var item1 in item.CartItems)
                {
                    var productDto = await _productRepository.GetByFilterAsync(prd => prd.ProductId == item1.ProductId);
                    var cartItemDto = new ResultCartItemDto
                    {
                        CartId = item1.CartId,
                        CartItemId = item1.CartItemId,
                        ProductId = item1.ProductId,
                        Product = productDto,
                        Quantity = item1.Quantity,
                        TotalPrice = item1.TotalPrice,
                    };
                    
                    cartDto.CartItems.Add(cartItemDto);
                }
            }
            
            result.Add(cartDto);
        }
        
        return result;
    }

    public async Task<List<ResultCartDto>> GelAllCartAsync()
    {
        var carts = await _repository.GetAllAsync();
        var cartItems = await _itemRepository.GetAllAsync();
        var products = await _productRepository.GetAllAsync();
        var result = new List<ResultCartDto>();

        foreach (var item in carts)
        {
            var customerDto = await _customerRepository.GetByFilterAsync(cus => cus.CustomerId == item.CustomerId);
            var cartDto = new ResultCartDto
            {
                CartId = item.CartId,
                CreatedDate = item.CreatedDate,
                CustomerId = item.CustomerId,
                Customer = customerDto,
                TotalAmount = item.TotalAmount,
                UserId = item.UserId,
                CartItems = new List<ResultCartItemDto>()
            };

            foreach (var item1 in item.CartItems)
            {
                var productDto = await _productRepository.GetByFilterAsync(prd => prd.ProductId == item1.ProductId);
                var cartItemDto = new ResultCartItemDto
                {
                    CartId = item1.CartId,
                    CartItemId = item1.CartItemId,
                    ProductId = item1.ProductId,
                    Product = productDto,
                    Quantity = item1.Quantity,
                    TotalPrice = item1.TotalPrice,
                };
                
                cartDto.CartItems.Add(cartItemDto);
            }
            
            result.Add(cartDto);
        }
        
        return result;
    }

    public async Task<GetByIdCartDto> GetByIdCartAsync(int id)
    {
        var cart = await _repository.GetByIdAsync(id);
        if (cart != null)
        {
            var cartItems = await _itemRepository.GetAllAsync();
            var customer = await _customerRepository.GetByIdAsync(cart.CustomerId);

            var result = new GetByIdCartDto
            {
                CartId = cart.CartId,
                CartItems = new List<ResultCartItemDto>(),
                CreatedDate = cart.CreatedDate,
                CustomerId = cart.CustomerId,
                Customer = customer,
                TotalAmount = cart.TotalAmount,
                UserId = cart.UserId,
            };
            if (cart.CartItems != null)
            {
                foreach (var item1 in cart.CartItems)
                {
                    var productDto = await _productRepository.GetByFilterAsync(prd => prd.ProductId == item1.ProductId);
                    var cartItemDto = new ResultCartItemDto
                    {
                        CartId = item1.CartId,
                        CartItemId = item1.CartItemId,
                        ProductId = item1.ProductId,
                        Product = productDto,
                        Quantity = item1.Quantity,
                        TotalPrice = item1.TotalPrice,
                    };
                    
                    result.CartItems.Add(cartItemDto);
                }
            }
            
            return result;
        }

        else
        {
            return new GetByIdCartDto { };
        }
    }
    
    public async Task<GetByIdCartDto> GetByUserIdCartAsync(string userId)
    {
        var cart = await _repository.FirstOrDefaultAsync(cart => cart.UserId == userId);
        if (cart != null)
        {
            var cartItems = await _itemRepository.GetAllAsync();
            var customer = await _customerRepository.GetByIdAsync(cart.CustomerId);
            var result = new GetByIdCartDto
            {
                CartId = cart.CartId,
                CartItems = new List<ResultCartItemDto>(),
                CreatedDate = cart.CreatedDate,
                CustomerId = cart.CustomerId,
                Customer = customer,
                TotalAmount = cart.TotalAmount,
                UserId = cart.UserId,
            };

            if (cart.CartItems != null)
            {
                foreach (var item1 in cart.CartItems)
                {
                    var productDto = await _productRepository.GetByFilterAsync(prd => prd.ProductId == item1.ProductId);
                    var cartItemDto = new ResultCartItemDto
                    {
                        CartId = item1.CartId,
                        CartItemId = item1.CartItemId,
                        ProductId = item1.ProductId,
                        Product = productDto,
                        Quantity = item1.Quantity,
                        TotalPrice = item1.TotalPrice,
                    };
                    
                    result.CartItems.Add(cartItemDto);
                }
            }
            
            return result;
        }

        else
        {
            return new GetByIdCartDto { };
        }
    }

    public async Task UpdateCartAsync(UpdateCartDto model)
    {
        var cart = await _repository.GetByIdAsync(model.CartId);
        var cartItems = await _itemRepository.GetAllAsync();

        var sum = 0;
        foreach (var item1 in model.CartItems)
        {
            foreach (var item in cart.CartItems)
            {
                var cartItem = await _itemRepository.GetByIdAsync(item.CartItemId);
                if (item.CartItemId == item1.CartItemId)
                {
                    cartItem.Quantity = item1.Quantity;
                    cartItem.TotalPrice = item1.TotalPrice;
                }
                
                sum = sum + item1.TotalPrice;
            }
        }
        
        cart.TotalAmount = sum;
        await _repository.UpdateAsync(cart);
    }

    public async Task UpdateTotalAmount(int cartId, decimal totalAmount)
    {
        await _cartsRepository.UpdateTotalAmountAsync(cartId, totalAmount);
    }
}