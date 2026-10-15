using ReyadConsultancy.Domain.Entities;

namespace ReyadConsultancy.Application.Website.Models;

public class LayoutModel
{
    public SiteSetting Settings { get; set; } = new();
    public List<Service> MenuServices { get; set; } = new();
    public List<BlogPost> RecentPosts { get; set; } = new();
}

public class HomePageModel
{
    public List<Slide> Slides { get; set; } = new();
    public List<Feature> Features { get; set; } = new();
    public PageContent? About { get; set; }
    public List<AboutItem> AboutItems { get; set; } = new();
    public List<Service> Services { get; set; } = new();
    public List<Faq> Faqs { get; set; } = new();
    public List<BlogPost> LatestPosts { get; set; } = new();
    public List<Counter> Counters { get; set; } = new();
    public List<TeamMember> Team { get; set; } = new();
    public List<Testimonial> Testimonials { get; set; } = new();
    public List<ClientLogo> Clients { get; set; } = new();
}

public class AboutPageModel
{
    public PageContent? About { get; set; }
    public PageContent? Work { get; set; }
    public List<AboutHighlight> Highlights { get; set; } = new();
    public List<Counter> Counters { get; set; } = new();
    public List<TeamMember> Team { get; set; } = new();
    public List<Testimonial> Testimonials { get; set; } = new();
}

public class ServicesPageModel
{
    public List<Service> Services { get; set; } = new();
    public List<Faq> Faqs { get; set; } = new();
    public List<ClientLogo> Clients { get; set; } = new();
}

public class ServiceDetailModel
{
    public Service Service { get; set; } = null!;
    public List<Service> AllServices { get; set; } = new();
    public List<Service> Related { get; set; } = new();
    public TeamMember? Member { get; set; }
}

public class CategoryItem
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class ArchiveItem
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Count { get; set; }
}

public class BlogSidebarModel
{
    public List<CategoryItem> Categories { get; set; } = new();
    public List<BlogPost> Latest { get; set; } = new();
    public PageContent? TextWidget { get; set; }
    public List<ArchiveItem> Archives { get; set; } = new();
    public List<string> Tags { get; set; } = new();
}

public class BlogListModel
{
    public List<BlogPost> Posts { get; set; } = new();
    public BlogSidebarModel Sidebar { get; set; } = new();
    public int Page { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public string? CategorySlug { get; set; }
    public string? Search { get; set; }
    public string? Tag { get; set; }
}

public class BlogDetailModel
{
    public BlogPost Post { get; set; } = null!;
    public List<BlogComment> Comments { get; set; } = new();
    public BlogPost? Previous { get; set; }
    public BlogPost? Next { get; set; }
    public BlogSidebarModel Sidebar { get; set; } = new();
}
