using ReyadConsultancy.Application.Website.Models;
using ReyadConsultancy.Domain.Entities;

namespace ReyadConsultancy.Application.Website;

/// <summary>Read side of the public website. Every item comes from the database and is managed in the admin panel.</summary>
public interface IWebsiteContentService
{
    Task<LayoutModel> GetLayoutAsync();
    Task<HomePageModel> GetHomeAsync();
    Task<AboutPageModel> GetAboutAsync();
    Task<ServicesPageModel> GetServicesAsync();
    Task<List<Service>> GetServiceListAsync();
    Task<ServiceDetailModel?> GetServiceAsync(string slug);
    Task<BlogListModel> GetBlogListAsync(int page, int pageSize, string? categorySlug, string? search, string? tag);
    Task<BlogDetailModel?> GetBlogPostAsync(string slug);
    Task<PageContent?> GetPageAsync(string key);
}

/// <summary>Write side: visitor submissions (contact, appointment, comments, newsletter).</summary>
public interface IWebsiteSubmissionService
{
    Task SubmitContactAsync(ContactMessage message);
    Task SubmitAppointmentAsync(Appointment appointment);
    Task<bool> AddCommentAsync(string postSlug, string name, string email, string? phone, string message);
    /// <summary>Returns true when a new subscriber was added, false when it already existed.</summary>
    Task<bool> SubscribeAsync(string email);
}
