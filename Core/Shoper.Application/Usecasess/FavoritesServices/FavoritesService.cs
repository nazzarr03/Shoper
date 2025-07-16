using Shoper.Application.Dtos.FavoritesDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Interfaces.IFavoritesRepository;
using Shoper.Domain.Entities;

namespace Shoper.Application.Usecasess.FavoritesServices;

public class FavoritesService : IFavoritesService
{
    private readonly IRepository<Favorites> _repository;
    private readonly IRepository<Product> _productRepository;
    private readonly IFavoritesRepository _favoritesRepository;

    public FavoritesService(
        IRepository<Favorites> repository,
        IRepository<Product> productRepository,
        IFavoritesRepository favoritesRepository)
    {
        _repository = repository;
        _productRepository = productRepository;
        _favoritesRepository = favoritesRepository;
    }
    
    public async Task<bool> CheckFavoritesByUseridAndProductId(string userId, int productId)
    {
        var favorite = await _repository.WhereAsync(x => x.UserId == userId && x.ProductId == productId);
        if (favorite.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public async Task CreateFavoritesAsync(CreateFavoritesDto createFavoritesDto)
    {
        await _repository.CreateAsync(new Favorites
        {
            UserId = createFavoritesDto.UserId,
            ProductId = createFavoritesDto.ProductId,
            CustomerId = createFavoritesDto.CustomerId,
            CreatedDate = createFavoritesDto.CreatedDate,
        });
    }
    
    public async Task DeleteFavoritesAsync(int id)
    {
        var favorite = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(favorite);
    }
    
    public async Task<List<AdminFavoritesDto>> GetAdminFavoritesList()
    {
        var favorites = await _favoritesRepository.GetFavoritesGroupUserId();
        return favorites;
    }

    public async Task<List<ResultFavoritesDto>> GetAllFavoritesAsync()
    {
        var favorites = await _repository.GetAllAsync();
        var result = new List<ResultFavoritesDto>();
        foreach (var favori in favorites)
        {
            var favoritesDto = new ResultFavoritesDto
            {
                CreatedDate = favori.CreatedDate,
                UserId = favori.UserId,
                CustomerId = favori.CustomerId,
                ProductId = favori.ProductId,
                Id = favori.Id,
                Product = new Product(),
            };
            
            var product = await _productRepository.GetByIdAsync(favori.ProductId);
            favoritesDto.Product = product;
            result.Add(favoritesDto);
        }
        
        return result;
    }

    public async Task<GetByIdFavoritesDto> GetByIdFavoritesAsync(int id)
    {
        var favorite = await _repository.GetByIdAsync(id);
        var product = await _productRepository.GetByIdAsync(favorite.ProductId);
        var newFavorite = new GetByIdFavoritesDto
        {
            Id = favorite.Id,
            ProductId = favorite.ProductId,
            CustomerId = favorite.CustomerId,
            Product = product,
            CreatedDate = favorite.CreatedDate,
            UserId = favorite.UserId,
        };
        
        return newFavorite;
    }
    
    public async Task<int> GetCountByUserId(string userid)
    {
        var favorites = await _repository.WhereAsync(x => x.UserId == userid);
        return favorites.Count;
    }
    
    public async Task<List<ResultFavoritesDto>> GetFavoritesByUserId(string userid)
    {
        var favorites = await _repository.WhereAsync(x => x.UserId == userid);
        var result = new List<ResultFavoritesDto>();
        foreach (var favorite in favorites)
        {
            var favoritesDto = new ResultFavoritesDto
            {
                CreatedDate = favorite.CreatedDate,
                UserId = favorite.UserId,
                CustomerId = favorite.CustomerId,
                ProductId = favorite.ProductId,
                Id = favorite.Id,
                Product = new Product(),
            };
            var product = await _productRepository.GetByIdAsync(favorite.ProductId);
            favoritesDto.Product = product;
            result.Add(favoritesDto);
        }
        
        return result;
    }

    public async Task UpdateFavoritesAsync(UpdateFavoritesDto updateFavoritesDto)
    {
        var favorite = await _repository.GetByIdAsync(updateFavoritesDto.Id);
        favorite.ProductId = updateFavoritesDto.ProductId;
        await _repository.UpdateAsync(favorite);
    }
}