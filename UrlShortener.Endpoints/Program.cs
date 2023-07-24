using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Memory;
using UrlShortener.Endpoints.Configuration;
using UrlShortener.Endpoints.Models;
using UrlShortener.Endpoints.Services;

namespace UrlShortener.Endpoints;

public class Program
{
    private const string ApiTag = "ShortenerApi";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddMemoryCache();
        builder.Services.Configure<UrlShortenerDatabaseSettings>(builder.Configuration.GetSection("UrlShortenerDatabase"));
        builder.Services.Configure<UrlShortenerCacheSettings>(builder.Configuration.GetSection("UrlShortenerCache")); 
        builder.Services.AddScoped<IUrlService, UrlService>(); 
        builder.Services.AddSingleton<IUrlCacheService, UrlCacheService>();
        builder.Services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.AddFixedWindowLimiter("fixed", options =>
            {
                options.PermitLimit = 3;
                options.Window = TimeSpan.FromSeconds(10);
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = 5;
            });
        });

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseRateLimiter();
        app.UseHttpsRedirection();

        app.MapGet("/{tinyUrl}", GetUrl)
           .WithTags(ApiTag)
           .WithName("GetFullUrl")
           .RequireRateLimiting("fixed");

        app.MapPost("/create-shorter-url", CreateShorterUrl)
           .Accepts<CreateShortenerUrlRequest>("application/json")
           .Produces(201)
           .ProducesProblem(400)
           .WithTags(ApiTag)
           .WithName("CreateShortenerUrl");

        app.Run();
    }

    private static async Task<IResult> CreateShorterUrl(IMemoryCache memoryCache, IUrlService urlService, CreateShortenerUrlRequest request)
    {
        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
        {
            return TypedResults.BadRequest($"{request.Url} is not a valid URL.");
        }

        var url = await urlService.CreateAsync(request.Url, request.Alias);
        return TypedResults.Ok(url.TinyUrl);
    }

    private static async Task<IResult> GetUrl(IMemoryCache memoryCache, IUrlService urlService, string tinyUrl)
    {
        var url = await urlService.GetByTinyUrlAsync(tinyUrl);
        if (url is null)
        {
            return TypedResults.NotFound($"No URL found for the tiny url {tinyUrl}.");
        }

        await urlService.UpdateAsync(url);
        return TypedResults.Ok(new GetUrlResponse(url.FullUrl, url.TinyUrl, url.RequestedCount));
    }
}

