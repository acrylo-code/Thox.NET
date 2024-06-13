using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Thox.Data;
using Thox.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Diagnostics;
using Thox.Hubs;
using Thox.Models.DataModels;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("ThoxConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<GetReviewsFromExternalSites>();
builder.Services.AddSignalR();

var app = builder.Build();

// Middleware to set security headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//if page not found
app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}"
    );
    endpoints.MapControllerRoute(
      name: "default",
      pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
   // endpoints.MapHub<SignalHub>("/signalHub");
});

app.Run();


class Settings
{
    public static string GetApiKey(string key)
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        IConfigurationRoot configuration = builder.Build();

        return configuration["AppSettings:" + key];
    }

    static async Task TestAPI()
    {
        // Your API endpoint URL
        string apiUrl = "https://localhost:7212/api/RoomPrices/";

        // Your API key
        string apiKey = "thox.WWrN3939UZYyVzKILrxBYwOpbOT3mYCR6fUQfRQQXWnJCsXYHGCpC1dMjCQbIvbl";

        // Create an HttpClient instance
        using (HttpClient client = new HttpClient())
        {
            // Add the API key to the request headers
            client.DefaultRequestHeaders.Add("Api-Key", apiKey);

            try
            {
                // Send a GET request to the API endpoint
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("Response from API:");
                    Debug.WriteLine(responseBody);
                }
                else
                {
                    Debug.WriteLine($"Failed to call API. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
