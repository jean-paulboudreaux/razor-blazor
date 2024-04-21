// Jean-Paul Boudreaux, Andrew Kieu 
// C00416940, C00014562
// CMPS 358 .NET/C# Programming
// project Chinook Razor/Blazor Website Project
using BlazorChinookWebsite.Components;
using ChinookLibrary;
using SmartComponents.Inference.OpenAI;
using SmartComponents.LocalEmbeddings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSmartComponents()
    .WithInferenceBackend<OpenAIInferenceBackend>();
builder.Services.AddSingleton<LocalEmbedder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

// Ensure DbUtility is accessible here
var embedder = app.Services.GetRequiredService<LocalEmbedder>();
var artistList = DbUtility.ListArtists();
var listedArtists = embedder.EmbedRange(artistList.ToArray());

app.MapSmartComboBox("/api/suggestions/artists", 
    request => embedder.FindClosest(request.Query, listedArtists));

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();