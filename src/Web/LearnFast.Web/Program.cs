namespace LearnFast.Web
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using AutoMapper;

    using Braintree;
    using CloudinaryDotNet;
    using LearnFast.Data;
    using LearnFast.Data.Common;
    using LearnFast.Data.Common.Repositories;
    using LearnFast.Data.Models;
    using LearnFast.Data.Repositories;
    using LearnFast.Data.Seeding;
    using LearnFast.Services;
    using LearnFast.Services.Data;
    using LearnFast.Services.Data.CategoryService;
    using LearnFast.Services.Data.ContactService;
    using LearnFast.Services.Data.CountryService;
    using LearnFast.Services.Data.CourseService;
    using LearnFast.Services.Data.DifficultyService;
    using LearnFast.Services.Data.ImageService;
    using LearnFast.Services.Data.LanguageService;
    using LearnFast.Services.Data.ReviewService;
    using LearnFast.Services.Data.VideoService;
    using LearnFast.Services.Mapping;
    using LearnFast.Services.Messaging;
    using LearnFast.Web.Middlewares;
    using LearnFast.Web.ViewModels;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Services, builder.Configuration);
            var app = builder.Build();
            Configure(app);
            app.UseAuthentication();
            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Cloudinary api
            var cloudName = configuration.GetValue<string>("AccountSettings:CloudName");
            var apiKey = configuration.GetValue<string>("AccountSettings:ApiKey");
            var apiSecret = configuration.GetValue<string>("AccountSettings:ApiSecret");

            if (new[] { cloudName, apiKey, apiSecret }.Any(string.IsNullOrWhiteSpace))
            {
                throw new ArgumentException("Please specify Cloudinary account details!");
            }

            services.AddSingleton(new Cloudinary(new Account(cloudName, apiKey, apiSecret)));

            //var aes256EncryptionConfig = new Aes256EncryptionConfig();
            configuration.GetSection("Aes256Encryption").Bind(aes256EncryptionConfig);
            services.AddSingleton(aes256EncryptionConfig);

            services.AddDbContext<ApplicationDbContext>(
                options => options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                })
                .AddRazorRuntimeCompilation();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            services.AddSingleton(mapper);
            services.AddSingleton(configuration);
            services.AddMvc();

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();
            services.AddHttpContextAccessor();

            // Application services
            var newGateway = new BraintreeGateway()
            {
                MerchantId = configuration.GetValue<string>("BraintreeGateway:MerchantId"),
                PublicKey = configuration.GetValue<string>("BraintreeGateway:PublicKey"),
                PrivateKey = configuration.GetValue<string>("BraintreeGateway:PrivateKey"),
            };

            services.AddTransient<IBraintreeService>(x => new Services.BraintreeService(
                configuration.GetValue<string>("BraintreeGateway:MerchantId"),
                configuration.GetValue<string>("BraintreeGateway:PublicKey"),
                configuration.GetValue<string>("BraintreeGateway:PrivateKey")));

            services.AddTransient<IEmailSender>(x => new SendGridEmailSender(configuration["SendGrid:ApiKey"]));

            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<IFilterCourse, CourseService>();
            services.AddTransient<ILanguageService, LanguageService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IDifficultyService, DifficultyService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IVideoService, VideoService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<IUserService, UserService>();
<<<<<<< Updated upstream
=======
            services.AddTransient<IPaymentService, PaymentService>();
>>>>>>> Stashed changes
        }

        private static void Configure(WebApplication app)
        {
            // Seed data on application startup
            using (var serviceScope = app.Services.CreateScope())
            {
                ApplicationDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // app.UseMiddleware<RedirectMiddleware>();

            app.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute("users", "{username}", new { controller = "Profile", action = "Index", username = string.Empty });

            app.MapRazorPages();
        }
    }
}
