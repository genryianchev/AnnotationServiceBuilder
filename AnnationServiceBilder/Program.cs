using AnnationServiceBilder.Annotations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

////////////////////////////////////////////////// AnnationServiceBilder ////////////////////////////////////
// Base URLs
var defaultBaseUrl = "https://jsonplaceholder.typicode.com";

var assembly = Assembly.GetExecutingAssembly();
builder.Services.AddRefitClientsFromAttributes(assembly, defaultBaseUrl);
builder.Services.AddAnnotatedSingletonServices(assembly);
builder.Services.AddAnnotatedScopedServices(assembly);


////////////////////////////////////////////////// AnnationServiceBilder ////////////////////////////////////


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
