namespace SilkRoad.Core;

public interface IUnitOfWork
{
    public IProductRepository ProductRepository { get; }
    public IProductImageRepository ProductImageRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public ICustomerBasketRepository CustomerBasketRepository { get; }
    public IAuth Auth { get; }
    public ICompleteAccountRepository CompleteAccountRepository { get; }
}
