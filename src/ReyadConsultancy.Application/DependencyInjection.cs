using Microsoft.Extensions.DependencyInjection;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Application.Services;
using ReyadConsultancy.Application.Website;

namespace ReyadConsultancy.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped(typeof(ICrudService<>), typeof(CrudService<>));
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IMediaService, MediaService>();
        services.AddScoped<IWebsiteContentService, WebsiteContentService>();
        services.AddScoped<IWebsiteSubmissionService, WebsiteSubmissionService>();
        return services;
    }
}
