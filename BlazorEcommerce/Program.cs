using BlazorEcommerce.Components;
using BlazorEcommerce.Components.Account;
using BlazorEcommerce.Data;
using BlazorEcommerce.Repository;
using BlazorEcommerce.Repository.IRepository;
using BlazorEcommerce.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddRadzenComponents();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<SharedStateService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = "1160092848406864" ?? throw new InvalidOperationException("Facebook AppId not found in configuration.");
        facebookOptions.AppSecret = "" ?? throw new InvalidOperationException("Facebook AppSecret not found in configuration.");
    })
    .AddMicrosoftAccount(microsoftOptions =>
    {
        microsoftOptions.ClientId = "fcad5410-4c6e-4146-b904-b19c6f86de7d" ?? throw new InvalidOperationException("Microsoft ClientId not found in configuration.");
        microsoftOptions.ClientSecret = "" ?? throw new InvalidOperationException("Microsoft ClientSecret not found in configuration.");
    })
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = "394881385187-rd6t22ekpm3ufbm44ofr1135pu0ng9ek.apps.googleusercontent.com" ?? throw new InvalidOperationException("Google ClientId not found in configuration.");
        googleOptions.ClientSecret = "" ?? throw new InvalidOperationException("Google ClientSecret not found in configuration.");
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Transient);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("StripeApiKey").Value;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
