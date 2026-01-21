using Core.Entites;

namespace Core.IRepositories;

public interface IProductRepository : IGenericIRepository<Product>
{
    Task<Product?> GetByNameAsync(string name);
}