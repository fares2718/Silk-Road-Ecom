using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SilkRoad.Core;
using SilkRoad.Core.Services;
using StackExchange.Redis;

namespace SilkRoad.Infrastructure;

internal class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IImageManagementService _imageManagementService;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;
    private readonly IConnectionMultiplexer _redis;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IGenerateToken _generateToken;
    public ICategoryRepository CategoryRepository { get; }

    public IProductRepository ProductRepository { get; }

    public IProductImageRepository ProductImageRepository { get; }

    public ICustomerBasketRepository CustomerBasketRepository { get; }

    public IAuth Auth { get; }

    public UnitOfWork(AppDbContext context, IImageManagementService imageManagementService,
         IMapper mapper, IConnectionMultiplexer redis,
          UserManager<AppUser> userManager, IEmailService emailService
          , SignInManager<AppUser> signInManager, IGenerateToken generateToken)
    {
        _context = context;
        _imageManagementService = imageManagementService;
        _emailService = emailService;
        _mapper = mapper;
        _redis = redis;
        _userManager = userManager;
        _generateToken = generateToken;
        _signInManager = signInManager;
        CategoryRepository = new CategoryRepository(_context);
        ProductRepository = new ProductRepository(_context, _mapper, _imageManagementService);
        ProductImageRepository = new ProductImageRepository(_context);
        CustomerBasketRepository = new CustomerBasketRepository(_redis);
        Auth = new AuthRepository(_userManager, _signInManager, _emailService,_generateToken);

    }

}
