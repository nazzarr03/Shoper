using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoper.Application.Interfaces;
using Shoper.Application.Interfaces.ICartItemsRepository;
using Shoper.Application.Interfaces.ICartsRepository;
using Shoper.Application.Interfaces.IOrderRepository;
using Shoper.Application.Interfaces.IProductsRepository;
using Shoper.Application.Usecasess.AccountServices;
using Shoper.Application.Usecasess.CartItemSevices;
using Shoper.Application.Usecasess.CartServices;
using Shoper.Application.Usecasess.CategoryServices;
using Shoper.Application.Usecasess.ContactServices;
using Shoper.Application.Usecasess.CustomerServices;
using Shoper.Application.Usecasess.OrderItemServices;
using Shoper.Application.Usecasess.OrderServices;
using Shoper.Application.Usecasess.ProductServices;
using Shoper.Persistence.Context;
using Shoper.Persistence.Context.Identity;
using Shoper.Persistence.Repositories;
using Shoper.Persistence.Repositories.CartItemsRepository;
using Shoper.Persistence.Repositories.CartsRepository;
using Shoper.Persistence.Repositories.OrdersRepository;
using Shoper.Persistence.Repositories.ProductsRepository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<ICartsRepository, CartsRepository>();
builder.Services.AddScoped<ICartItemsRepository, CartItemsRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryServices>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ICustomerService, CustomerServices>();

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
    });

builder.Services.AddIdentity<AppIdentityUser, AppIdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{

    //options.Password.RequireDigit = true; //Þifre Sayýsal karakteri desteklesin mi?
    options.Password.RequiredLength = 6;  //Þifre minumum karakter sayýsý
    options.Password.RequireLowercase = true; //Þifre küçük harf olabilir
    options.Password.RequireLowercase = true; //Þifre büyük harf olabilir
    options.Password.RequireNonAlphanumeric = false; //Sembol bulunabilir

    options.Lockout.MaxFailedAccessAttempts = 5; //Kullanýcý kaç baþarýsýz giriþten sonra sisteme giriþ yapamasýn
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //Baþarýsýz giriþ iþlemlerinden sonra ne kadar süre sonra sisteme giriþ hakký tanýnsýn
    options.Lockout.AllowedForNewUsers = true; //Yeni üyeler için kilit sistemi geçerli olsun mu

    options.User.RequireUniqueEmail = true; //Kullanýcý benzersiz e-mail adresine sahip olsun

    options.SignIn.RequireConfirmedEmail = false; //Kayýt iþlemleri için email onaylamasý zorunlu olsun mu?
    options.SignIn.RequireConfirmedPhoneNumber = false; //Telefon onayý olsun mu?
});    

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();