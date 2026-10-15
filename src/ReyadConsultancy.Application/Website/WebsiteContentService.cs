using Microsoft.EntityFrameworkCore;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Application.Website.Models;
using ReyadConsultancy.Domain.Entities;

namespace ReyadConsultancy.Application.Website;

public class WebsiteContentService : IWebsiteContentService
{
    private readonly IApplicationDbContext _db;
    private Task<LayoutModel>? _layout;   // memoised per request (service is scoped)

    public WebsiteContentService(IApplicationDbContext db) => _db = db;

    private IQueryable<BlogPost> PublishedPosts()
    {
        var now = DateTime.UtcNow;
        return _db.Set<BlogPost>().AsNoTracking().Where(p => p.IsPublished && p.PublishedAt <= now);
    }

    public Task<LayoutModel> GetLayoutAsync() => _layout ??= LoadLayoutAsync();

    private async Task<LayoutModel> LoadLayoutAsync()
    {
        var settings = await _db.Set<SiteSetting>().AsNoTracking().OrderBy(s => s.Id).FirstOrDefaultAsync() ?? new SiteSetting();
        var services = await _db.Set<Service>().AsNoTracking().Where(s => s.IsActive)
            .OrderBy(s => s.SortOrder).ThenBy(s => s.Id).ToListAsync();
        var recent = await PublishedPosts().OrderByDescending(p => p.PublishedAt).Take(3).ToListAsync();
        return new LayoutModel { Settings = settings, MenuServices = services, RecentPosts = recent };
    }

    public Task<PageContent?> GetPageAsync(string key) =>
        _db.Set<PageContent>().AsNoTracking().FirstOrDefaultAsync(p => p.Key == key);

    public async Task<HomePageModel> GetHomeAsync()
    {
        var m = new HomePageModel();
        m.Slides = await _db.Set<Slide>().AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToListAsync();
        m.Features = await _db.Set<Feature>().AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToListAsync();
        m.About = await GetPageAsync("home-about");
        m.AboutItems = await _db.Set<AboutItem>().AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToListAsync();
        m.Services = await _db.Set<Service>().AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToListAsync();
        m.Faqs = await _db.Set<Faq>().AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToListAsync();
        m.LatestPosts = await PublishedPosts().Include(p => p.Category).OrderByDescending(p => p.PublishedAt).Take(3).ToListAsync();
        m.Counters = await _db.Set<Counter>().AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToListAsync();
        m.Team = await _db.Set<TeamMember>().AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToListAsync();
        m.Testimonials = await _db.Set<Testimonial>().AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToListAsync();
        m.Clients = await _db.Set<ClientLogo>().AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToListAsync();
        return m;
    }

    public async Task<AboutPageModel> GetAboutAsync()
    {
        var m = new AboutPageModel();
        m.About = await GetPageAsync("about");
        m.Work = await GetPageAsync("about-work");
        m.Highlights = await _db.Set<AboutHighlight>().AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToListAsync();
        m.Counters = await _db.Set<Counter>().AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToListAsync();
        m.Team = await _db.Set<TeamMember>().AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToListAsync();
        m.Testimonials = await _db.Set<Testimonial>().AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToListAsync();
        return m;
    }

    public async Task<ServicesPageModel> GetServicesAsync()
    {
        var m = new ServicesPageModel();
        m.Services = await GetServiceListAsync();
        m.Faqs = await _db.Set<Faq>().AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToListAsync();
        m.Clients = await _db.Set<ClientLogo>().AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToListAsync();
        return m;
    }

    public Task<List<Service>> GetServiceListAsync() =>
        _db.Set<Service>().AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToListAsync();

    public async Task<ServiceDetailModel?> GetServiceAsync(string slug)
    {
        var service = await _db.Set<Service>().AsNoTracking().FirstOrDefaultAsync(s => s.Slug == slug && s.IsActive);
        if (service == null) return null;
        var all = await GetServiceListAsync();
        var member = await _db.Set<TeamMember>().AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.SortOrder).ThenBy(x => x.Id).FirstOrDefaultAsync();
        return new ServiceDetailModel
        {
            Service = service,
            AllServices = all,
            Related = all.Where(s => s.Id != service.Id).Take(3).ToList(),
            Member = member
        };
    }

    private async Task<BlogSidebarModel> LoadSidebarAsync()
    {
        var now = DateTime.UtcNow;
        var s = new BlogSidebarModel();
        s.Categories = await _db.Set<BlogCategory>().AsNoTracking()
            .Select(c => new CategoryItem
            {
                Name = c.Name,
                Slug = c.Slug ?? "",
                Count = c.Posts.Count(p => p.IsPublished && p.PublishedAt <= now)
            }).ToListAsync();
        s.Latest = await PublishedPosts().OrderByDescending(p => p.PublishedAt).Take(3).ToListAsync();
        s.TextWidget = await GetPageAsync("blog-text-widget");

        var meta = await PublishedPosts().Select(p => new { p.PublishedAt, p.Tags }).ToListAsync();
        s.Archives = meta.GroupBy(p => new { p.PublishedAt.Year, p.PublishedAt.Month })
            .OrderByDescending(g => g.Key.Year).ThenByDescending(g => g.Key.Month)
            .Select(g => new ArchiveItem { Year = g.Key.Year, Month = g.Key.Month, Count = g.Count() }).ToList();
        s.Tags = meta.Where(p => !string.IsNullOrWhiteSpace(p.Tags))
            .SelectMany(p => p.Tags!.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(t => t).ToList();
        return s;
    }

    public async Task<BlogListModel> GetBlogListAsync(int page, int pageSize, string? categorySlug, string? search, string? tag)
    {
        if (page < 1) page = 1;
        var q = PublishedPosts().Include(p => p.Category).AsQueryable();
        if (!string.IsNullOrWhiteSpace(categorySlug)) q = q.Where(p => p.Category != null && p.Category.Slug == categorySlug);
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            q = q.Where(p => p.Title.Contains(term) || (p.Summary != null && p.Summary.Contains(term)) || p.Content.Contains(term));
        }
        if (!string.IsNullOrWhiteSpace(tag))
        {
            var t = tag.Trim();
            q = q.Where(p => p.Tags != null && p.Tags.Contains(t));
        }

        var total = await q.CountAsync();
        var totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize));
        if (page > totalPages) page = totalPages;
        var posts = await q.OrderByDescending(p => p.PublishedAt).ThenByDescending(p => p.Id)
            .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new BlogListModel
        {
            Posts = posts, Page = page, TotalPages = totalPages,
            CategorySlug = categorySlug, Search = search, Tag = tag,
            Sidebar = await LoadSidebarAsync()
        };
    }

    public async Task<BlogDetailModel?> GetBlogPostAsync(string slug)
    {
        var post = await PublishedPosts().Include(p => p.Category).FirstOrDefaultAsync(p => p.Slug == slug);
        if (post == null) return null;
        var comments = await _db.Set<BlogComment>().AsNoTracking()
            .Where(c => c.BlogPostId == post.Id && c.IsApproved).OrderBy(c => c.CreatedAt).ToListAsync();
        var prev = await PublishedPosts().Where(p => p.PublishedAt < post.PublishedAt).OrderByDescending(p => p.PublishedAt).FirstOrDefaultAsync();
        var next = await PublishedPosts().Where(p => p.PublishedAt > post.PublishedAt).OrderBy(p => p.PublishedAt).FirstOrDefaultAsync();
        return new BlogDetailModel { Post = post, Comments = comments, Previous = prev, Next = next, Sidebar = await LoadSidebarAsync() };
    }
}
