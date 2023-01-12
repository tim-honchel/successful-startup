using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using SuccessfulStartup.Data.Authentication; // assembly reference in order to access Identity database

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;



var builder = WebApplication.CreateBuilder(args); // initializes a builder for configuring a new web application

var connectionString = builder.Configuration.GetConnectionString("IdentityConnectionString"); // gets database connection string from appsettings.json

// configure service collection

builder.Services.AddRazorPages(); // allows Razor components, routing, model binding, caching, and view engines
builder.Services.AddServerSideBlazor(); // allows Blazor Server specific functions
builder.Services.AddDbContext<AuthenticationDbContext>(options => options.UseSqlServer(connectionString)); // connects to database with Identity accounts
builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AuthenticationDbContext>(); // adds default UI for Identity, eliminating need to create custom register and login pages, also requires account verification prior to first login
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<AppUser>>(); // periodically checks whether user credentials are still valid
builder.Services.AddTransient<IEmailSender, EmailSender>(); // enables email sends

var app = builder.Build(); // initializes web application from builder

// middleware pipleline sets the order for handing incoming requests and outgoing responses

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // shows greater detail for debugging
}
else
{
    app.UseExceptionHandler("/Error"); // shows user-friendly error page 
    app.UseHsts(); // HTTP strict transport security forces browsers to use HTTPS
}


app.UseHttpsRedirection(); // redirects all HTTP requests to HTTPS (both HSTS and HTTPS Redirection are recommended)
app.UseStaticFiles(); // enables use of HTML, CSS, JavaScript, image files in wwwroot directory
app.UseRouting(); // takes URL and searches for best matching endpoint 
app.UseAuthentication(); // identifies the current user
app.UseAuthorization(); // verifies that the current user is allowed to access the current endpoint
app.MapBlazorHub(); // establishes SignalR connection with endpoint for real-time Blazor functionality and communication
app.MapFallbackToPage("/Shared/_Host"); // if no endpoints are found, _Host will return the index page

app.Run();
