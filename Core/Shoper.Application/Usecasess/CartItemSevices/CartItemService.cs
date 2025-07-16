using Shoper.Application.Dtos.CartItemDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Interfaces.ICartItemsRepository;
using Shoper.Domain.Entities;

namespace Shoper.Application.Usecasess.CartItemSevices;

public class CartItemService : ICartItemService
{
    private readonly IRepository<CartItem> _repository;
    private readonly IRepository<Cart> _cartRepository;
    private readonly ICartItemsRepository _cartItemsRepository;

    public CartItemService(
        IRepository<CartItem> repository,
        IRepository<Cart> cartRepository,
        ICartItemsRepository cartItemsRepository)
    {
        _repository = repository;
        _cartRepository = cartRepository;
        _cartItemsRepository = cartItemsRepository;
    }
    
    public async Task<bool> CheckCartItems(int cartId, int productId)
    {
        var value = await _cartItemsRepository.CheckCartItemAsync(cartId, productId);
        return value;
    }
    
    public async Task CreateCartItemAsync(CreateCartItemDto model)
    {
        var newCartItem = new CartItem
        {
            CartId = model.CartId,
            ProductId = model.ProductId,
            Quantity = model.Quantity,
            TotalPrice = model.TotalPrice
        };
        await _repository.CreateAsync(newCartItem);
    }
    
    public async Task DeleteCartItemAsync(int id)
    {
        var foundCartItem = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(foundCartItem);
    }

    public async Task<List<ResultCartItemDto>> GetAllCartItemAsync()
    {
        var cartItems = await _repository.GetAllAsync();
        return cartItems.Select(x => new ResultCartItemDto 
        {
            CartId = x.CartId,
            ProductId = x.ProductId,
            CartItemId = x.CartItemId,
            Quantity = x.Quantity,
            TotalPrice = x.TotalPrice,
        }).ToList();
    }

    public async Task<GetByIdCartItemDto> GetByIdCartItemAsync(int id)
    {
        var cartItem = await _repository.GetByIdAsync(id);
        return new GetByIdCartItemDto
        {
            CartId = cartItem.CartId,
            ProductId = cartItem.ProductId,
            CartItemId = cartItem.CartItemId,
            Quantity = cartItem.Quantity,
            TotalPrice = cartItem.TotalPrice,
        };
    }
    
    public async Task<int> GetCountCartItemsByCartId(string userId)
    {
        var cartId = await _cartRepository.FirstOrDefaultAsync(x => x.UserId == userId);
        if (cartId == null)
        {
            return 0;
        }
        else
        {
            var cartItems = await _repository.WhereAsync(x => x.CartId == cartId.CartId);
            if (cartItems == null)
            {
                return 0;
            }
            else
            {
                return cartItems.Count();
            }
        }
    }

    public async Task UpdateCartItemAsync(UpdateCartItemDto model)
    {
        var cartItem = await _repository.GetByIdAsync(model.CartItemId);
        cartItem.ProductId = model.ProductId;
        cartItem.TotalPrice = model.TotalPrice;
        cartItem.Quantity = model.Quantity;
        await _repository.UpdateAsync(cartItem);
    }
    
    public async Task UpdateQuantity(int cartId, int productId, int quantity)
    {
        await _cartItemsRepository.UpdateQuantity(cartId, productId, quantity);
    }
    
    public async Task UpdateQuantityOnCart(UpdateCartItemDto dto)
    {
        await _cartItemsRepository.UpdateQuantityOnCartAsync(dto);
    }

    public Task<List<ResultCartItemDto>> GetByCartIdCartItemAsync(int cartId)
    {
        throw new NotImplementedException();
    }
    
}