namespace SilkRoad.Core;

public interface IUnitOfWork
{
    public ICategoryRepository CategoryRepository { get; }
    public IProductRepository ProductRepository { get; }
    public IProductImageRepository ProductImageRepository { get; }
    public ICustomerBasketRepository CustomerBasketRepository { get; }
    public IAuth Auth { get; }
}
