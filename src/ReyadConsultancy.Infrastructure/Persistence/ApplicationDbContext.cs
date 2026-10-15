using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Common;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Infrastructure.Identity;

namespace ReyadConsultancy.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<SiteSetting> SiteSettings { get; set; } = null!;
    public DbSet<MediaFile> MediaFiles { get; set; } = null!;
    public DbSet<Slide> Slides { get; set; } = null!;
    public DbSet<Feature> Features { get; set; } = null!;
    public DbSet<AboutItem> AboutItems { get; set; } = null!;
    public DbSet<AboutHighlight> AboutHighlights { get; set; } = null!;
    public DbSet<PageContent> PageContents { get; set; } = null!;
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<Faq> Faqs { get; set; } = null!;
    public DbSet<BlogCategory> BlogCategories { get; set; } = null!;
    public DbSet<BlogPost> BlogPosts { get; set; } = null!;
    public DbSet<BlogComment> BlogComments { get; set; } = null!;
    public DbSet<Counter> Counters { get; set; } = null!;
    public DbSet<TeamMember> TeamMembers { get; set; } = null!;
    public DbSet<Testimonial> Testimonials { get; set; } = null!;
    public DbSet<ClientLogo> ClientLogos { get; set; } = null!;
    public DbSet<ContactMessage> ContactMessages { get; set; } = null!;
    public DbSet<Appointment> Appointments { get; set; } = null!;
    public DbSet<NewsletterSubscriber> NewsletterSubscribers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Service>().HasIndex(x => x.Slug).IsUnique();
        builder.Entity<BlogPost>().HasIndex(x => x.Slug).IsUnique();
        builder.Entity<BlogCategory>().HasIndex(x => x.Slug).IsUnique();
        builder.Entity<PageContent>().HasIndex(x => x.Key).IsUnique();
        builder.Entity<NewsletterSubscriber>().HasIndex(x => x.Email).IsUnique();

        builder.Entity<BlogPost>()
            .HasOne(p => p.Category).WithMany(c => c.Posts)
            .HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.SetNull);

        builder.Entity<BlogComment>()
            .HasOne(c => c.BlogPost).WithMany(p => p.Comments)
            .HasForeignKey(c => c.BlogPostId).OnDelete(DeleteBehavior.Cascade);

        builder.Entity<MediaFile>().Property(m => m.Data).HasColumnType("varbinary(max)");
        builder.Entity<Appointment>().Property(a => a.Status).HasConversion<int>();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        StampAuditFields();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        StampAuditFields();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void StampAuditFields()
    {
        var now = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added) entry.Entity.CreatedAt = now;
            else if (entry.State == EntityState.Modified) entry.Entity.UpdatedAt = now;
        }
    }
}
