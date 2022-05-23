using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Sample_localization;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

#region multi language

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(
    options =>
    {
        var supportedCultures = new List<CultureInfo>
            {
                            new CultureInfo("en-US"),
                            new CultureInfo("ar-AE"),
                            new CultureInfo("fr-FR")
            };

        options.DefaultRequestCulture = new RequestCulture(culture: "fr-FR", uiCulture: "fr-FR");
        options.SupportedUICultures = supportedCultures;
        options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
        {
            var languages = context.Request.Headers["Accept-Language"].ToString();
            var currentLanguage = languages.Split(',').FirstOrDefault();
            var defaultLanguage = string.IsNullOrEmpty(currentLanguage) ? "en-US" : currentLanguage;

            //if (/*defaultLanguage != "fr-FR" && */defaultLanguage != "fr-FR")
            //{
             defaultLanguage = "fr-FR";
            //}

            return await Task.FromResult(new ProviderCultureResult(defaultLanguage, defaultLanguage));
        }));

    });

#endregion
builder.Services.AddControllers();

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

var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions != null ? locOptions.Value : new RequestLocalizationOptions());

app.UseAuthorization();

app.MapControllers();

app.Run();
