using System.Linq.Expressions;

namespace Shoper.Application.Interfaces;

public interface IRepository<T> where T : class
// Farklı türlerle çalışan bir interface tanımlar
// Bu türler primitive(ör; int) tipte olamaz 
{
    // metotlar asenkron olduğundan dolayı await ile kullanılır
    
    // Tüm verileri asenkron getirir ve T türünde bir liste döner
    Task<List<T>> GetAllAsync();
    
    // id ile bir tane T türünde veri döner
    Task<T> GetByIdAsync(int id);
    
    // T türünde bir nesne oluşturur
    Task CreateAsync(T entity);
    
    // T nesnesini günceller
    Task UpdateAsync(T entity);
    
    // T nesnesini siler
    Task DeleteAsync(T entity);
    
    Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter);
    
    Task<List<T>> GetTakeAsync(int sayi);
    
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
    
    Task<List<T>> WhereAsync(Expression<Func<T, bool>> filter);

}