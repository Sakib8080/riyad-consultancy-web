using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReyadConsultancy.Application.Common;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Infrastructure.Identity;

namespace ReyadConsultancy.Infrastructure.Persistence;

/// <summary>Creates / migrates the MS SQL database and seeds starter content + the first administrator.</summary>
public static class DbInitializer
{
    private const string Img = "/assets/images/";

    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var sp = scope.ServiceProvider;
        var db = sp.GetRequiredService<ApplicationDbContext>();
        var config = sp.GetRequiredService<IConfiguration>();
        var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("DbInitializer");

        // If you created EF migrations they are applied; otherwise the schema is created from the model.
        if (db.Database.GetMigrations().Any()) await db.Database.MigrateAsync();
        else await db.Database.EnsureCreatedAsync();

        await SeedAdminAsync(sp, config, logger);
        await SeedContentAsync(db);
    }

    private static async Task SeedAdminAsync(IServiceProvider sp, IConfiguration config, ILogger logger)
    {
        var roles = sp.GetRequiredService<RoleManager<IdentityRole>>();
        if (!await roles.RoleExistsAsync(IdentityService.AdminRole))
            await roles.CreateAsync(new IdentityRole(IdentityService.AdminRole));

        var email = config["SeedAdmin:Email"];
        var password = config["SeedAdmin:Password"];
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) return;

        var users = sp.GetRequiredService<UserManager<ApplicationUser>>();
        if (await users.FindByEmailAsync(email) != null) return;

        var admin = new ApplicationUser
        {
            UserName = email, Email = email, EmailConfirmed = true, IsApproved = true,
            FullName = config["SeedAdmin:FullName"] ?? "Administrator"
        };
        var result = await users.CreateAsync(admin, password);
        if (result.Succeeded)
        {
            await users.AddToRoleAsync(admin, IdentityService.AdminRole);
            logger.LogWarning("Default administrator '{Email}' created. Change the password after the first login!", email);
        }
        else
        {
            logger.LogError("Could not create default administrator: {Errors}", string.Join("; ", result.Errors.Select(e => e.Description)));
        }
    }

    private static async Task SeedContentAsync(ApplicationDbContext db)
    {
        if (!await db.SiteSettings.AnyAsync())
        {
            db.SiteSettings.Add(new SiteSetting
            {
                SiteName = "Reyad Business Solution and Consultancy",
                ShortName = "RBSC",
                Tagline = "Complete Solution of Taxes, VAT, Accounts & Company Law",
                MetaDescription = "Complete solution of taxes, VAT, accounts and company law in Chattogram - income tax lawyer and NBR certified VAT consultant.",
                LogoUrl = Img + "logo.png",
                FaviconUrl = Img + "favicon.ico",
                FooterImageUrl = Img + "ftr-abt-img.jpg",
                Address = "Chamber: Zamzam Tower (1st Floor), 383 Halishahar Road, Baparipara (West Side of Baparipara Mosque), South Agrabad, Chattogram.",
                Phone1 = "+88 01815-250265",
                Phone2 = "+88 01511-250265",
                Email1 = "riyed54@gmail.com",
                FooterAboutText = "Complete solution of taxes, VAT, accounts and company law - income tax, VAT, appeals, audit, RJSC and licensing services.",
                NewsletterText = "Sign up with your email to get fresh updates about taxes, VAT and company law",
                CopyrightText = "ALL RIGHTS RESERVED"
            });
        }

        if (!await db.Slides.AnyAsync())
        {
            db.Slides.AddRange(
                new Slide { Subtitle = "Welcome to RBSC", Title = "Taxes, VAT & Company Law", Description = "Complete solution of taxes, VAT, accounts and company law for individuals and businesses.", ImageUrl = Img + "slide-1.jpg", ButtonText = "Our Services", ButtonUrl = "/services", SortOrder = 1 },
                new Slide { Subtitle = "Trusted Consultancy", Title = "Expert Advice for Your Business", Description = "From income tax returns to VAT, audit and RJSC work, we handle the paperwork so you can focus on business.", ImageUrl = Img + "slide-2.jpg", ButtonText = "Book Appointment", ButtonUrl = "/appointment", SortOrder = 2 },
                new Slide { Subtitle = "Growth Partner", Title = "Results You Can Measure", Description = "Clear plans, honest advice and follow-through so your business keeps moving forward.", ImageUrl = Img + "slide-3.jpg", ButtonText = "About Us", ButtonUrl = "/about", SortOrder = 3 });
        }

        if (!await db.Features.AnyAsync())
        {
            db.Features.AddRange(
                new Feature { Title = "Open for new ideas and outstanding partnerships", Description = "We listen first, then design solutions that fit your goals.", ImageUrl = Img + "feature-1.jpg", LinkUrl = "/services", SortOrder = 1 },
                new Feature { Title = "Practical advice you can act on today", Description = "Straightforward recommendations with clear next steps.", ImageUrl = Img + "feature-2.jpg", LinkUrl = "/services", SortOrder = 2 },
                new Feature { Title = "Experienced and highly professional team", Description = "Specialists across strategy, finance, compliance and technology.", ImageUrl = Img + "feature-3.jpg", LinkUrl = "/about", SortOrder = 3 });
        }

        if (!await db.AboutItems.AnyAsync())
        {
            db.AboutItems.AddRange(
                new AboutItem { Title = "Our History", Description = "Reyad Business Solution and Consultancy was created to give businesses of every size access to clear, practical professional advice.", IconUrl = Img + "about-cnt-icon-1.png", SortOrder = 1 },
                new AboutItem { Title = "Our Mission", Description = "To help our clients make confident decisions and build sustainable, compliant and profitable businesses.", IconUrl = Img + "about-cnt-icon-2.png", SortOrder = 2 },
                new AboutItem { Title = "Our Vision", Description = "To be the most trusted consultancy partner for growing businesses in the markets we serve.", IconUrl = Img + "about-cnt-icon-3.png", SortOrder = 3 });
        }

        if (!await db.AboutHighlights.AnyAsync())
        {
            db.AboutHighlights.AddRange(
                new AboutHighlight { Title = "Brilliant ideas", IconClass = "icon icon-Bulb", SortOrder = 1 },
                new AboutHighlight { Title = "Professional specialists", IconClass = "icon icon-User", SortOrder = 2 },
                new AboutHighlight { Title = "Multi-industry experience", IconClass = "icon icon-WorldGlobe", SortOrder = 3 },
                new AboutHighlight { Title = "Free consultation", IconClass = "icon icon-WorldGlobe", SortOrder = 4 });
        }

        if (!await db.PageContents.AnyAsync())
        {
            db.PageContents.AddRange(
                new PageContent { Key = "home-about", Title = "About Us", ImageUrl = Img + "service-img.jpg" },
                new PageContent { Key = "about", Title = "About Us", Body = "<p>Reyad Business Solution and Consultancy supports owners and managers with strategy, finance, compliance, people and technology advice. We combine hands-on experience with a clear, honest approach so you always know where you stand and what to do next.</p>" },
                new PageContent { Key = "about-work", Title = "We Do Awesome Work", Body = "<p>Every engagement starts with understanding your business. We then agree a plan, deliver it with your team and review the results together.</p>", ImageUrl = Img + "work-1.jpg" },
                new PageContent { Key = "why-choose-us", Title = "Why Choose Us?", Body = "<p>Independent advice, a responsive team and a focus on results. We keep things simple and stay with you until the job is done.</p>" },
                new PageContent { Key = "blog-text-widget", Title = "About Our Blog", Body = "<p>Guides and news on income tax, VAT, accounts and company law from the RBSC team.</p>" });
        }

        if (!await db.Services.AnyAsync())
        {
            string Body(string topic) =>
                $"<p>Our {topic} service gives you a clear picture of where your business is today and a practical roadmap for where you want to be. We work alongside your team, explain every recommendation in plain language and help you put it into action.</p>" +
                "<h3>What you get</h3><ul><li>An initial discovery session to understand your goals</li><li>A written assessment with prioritised recommendations</li><li>Hands-on support during implementation</li><li>Regular reviews to measure progress</li></ul>" +
                "<p>Contact us to book a free consultation and find out how we can help.</p>";

            Service S(int order, string title, string sub, string shortText, string card, string icon) => new Service
            {
                Title = title, Slug = Slugger.Create(title), Subtitle = sub, ShortDescription = shortText,
                Description = Body(title.ToLowerInvariant()), ImageUrl = Img + card, IconUrl = Img + icon,
                BannerImageUrl = Img + "services-single.jpg", SortOrder = order
            };

            db.Services.AddRange(
                S(1, "Income Tax", "Personal & Corporate Tax", "Income tax planning, return preparation and filing for individuals and companies.", "practice-1.jpg", "services-icon-1.png"),
                S(2, "VAT Return, Management & Advisory", "VAT", "VAT return submission, VAT management and expert VAT advisory services.", "practice-2.jpg", "services-icon-2.png"),
                S(3, "Tax & VAT Appeal and Tribunal", "Appeal & Tribunal", "Representation and drafting for tax and VAT appeals and tribunal cases.", "practice-3.jpg", "services-icon-3.png"),
                S(4, "Accounts Preparation & Internal Audit", "Accounts & Audit", "Preparation of accounts and internal audit services for your business.", "practice-4.jpg", "services-icon-4.png"),
                S(5, "RJSC Service", "Company Law", "Company registration and RJSC filings, amendments and compliance support.", "practice-5.jpg", "practice-icon-5.png"),
                S(6, "Business Licences", "Licensing", "All licences relating to business - applications, renewals and follow-up.", "practice-6.jpg", "practice-icon-6.png"),
                S(7, "Financial Planning Service", "Planning", "Practical financial planning to help you manage tax, cash flow and growth.", "practice-7.jpg", "practice-icon-7.png"));
        }

        if (!await db.Faqs.AnyAsync())
        {
            db.Faqs.AddRange(
                new Faq { Question = "How do I start working with you?", Answer = "Book a free consultation using the appointment form. We will review your needs and propose a plan and timeline.", SortOrder = 1 },
                new Faq { Question = "Which industries do you support?", Answer = "We work with start-ups, small and medium businesses across trading, services, manufacturing and technology.", SortOrder = 2 },
                new Faq { Question = "How are your services priced?", Answer = "After the free consultation we provide a clear fixed or monthly fee so there are no surprises.", SortOrder = 3 },
                new Faq { Question = "Is my business information kept confidential?", Answer = "Yes. All client information is treated as strictly confidential.", SortOrder = 4 });
        }

        if (!await db.BlogCategories.AnyAsync())
        {
            db.BlogCategories.AddRange(
                new BlogCategory { Name = "Income Tax", Slug = "income-tax" },
                new BlogCategory { Name = "VAT", Slug = "vat" },
                new BlogCategory { Name = "Company Law", Slug = "company-law" },
                new BlogCategory { Name = "Accounts & Audit", Slug = "accounts-audit" });
            await db.SaveChangesAsync();
        }

        if (!await db.BlogPosts.AnyAsync())
        {
            var cats = await db.BlogCategories.ToDictionaryAsync(c => c.Slug!, c => c.Id);
            string Content(string intro) => $"<p>{intro}</p><p>Every business is different, so the best starting point is a short conversation about your goals, your numbers and your constraints. Our team is happy to help you turn the ideas in this article into a plan.</p><blockquote><p>Good decisions come from good information and a clear plan.</p></blockquote><p>Contact us to book a free consultation.</p>";
            var now = DateTime.UtcNow;
            db.BlogPosts.AddRange(
                new BlogPost { Title = "Income tax return: a simple checklist before you file", Slug = "income-tax-return-checklist", Summary = "Documents and details to gather so your return is accurate and on time.", Content = Content("A correct return starts with complete records of income, expenses and assets for the year."), ImageUrl = Img + "blog-1.jpg", Author = "RBSC Team", PublishedAt = now.AddDays(-30), Tags = "income tax, return", CategoryId = cats["income-tax"] },
                new BlogPost { Title = "VAT basics every business owner should know", Slug = "vat-basics-for-business-owners", Summary = "Registration, returns and record keeping explained in plain language.", Content = Content("Timely VAT returns and well kept records help you avoid penalties and claim what you are entitled to."), ImageUrl = Img + "blog-2.jpg", Author = "RBSC Team", PublishedAt = now.AddDays(-18), Tags = "vat, returns", CategoryId = cats["vat"] },
                new BlogPost { Title = "What to know before registering a company with RJSC", Slug = "before-registering-a-company-rjsc", Summary = "Structure, licences and compliance questions to settle before you file.", Content = Content("Choosing the right structure and understanding your compliance duties early saves time and money later."), ImageUrl = Img + "blog-3.jpg", Author = "RBSC Team", PublishedAt = now.AddDays(-6), Tags = "company law, rjsc", CategoryId = cats["company-law"] });
        }

        if (!await db.Counters.AnyAsync())
        {
            db.Counters.AddRange(
                new Counter { Title = "Happy Customers", Value = 100, IconUrl = Img + "counter-icon-1.png", SortOrder = 1 },
                new Counter { Title = "Finished Projects", Value = 150, IconUrl = Img + "counter-icon-2.png", SortOrder = 2 },
                new Counter { Title = "Our Partners", Value = 25, IconUrl = Img + "counter-icon-2.png", SortOrder = 3 },
                new Counter { Title = "Team Members", Value = 12, IconUrl = Img + "counter-icon-1.png", SortOrder = 4 },
                new Counter { Title = "Awards Won", Value = 5, IconUrl = Img + "counter-icon-3.png", SortOrder = 5 });
        }

        if (!await db.TeamMembers.AnyAsync())
        {
            db.TeamMembers.Add(new TeamMember { Name = "Hasan Mahamud Riyed", Designation = "Income Tax Lawyer & NBR Certified VAT Consultant (BBA, MBA Accounting, LLB)", PhotoUrl = Img + "team-1.jpg", SortOrder = 1 });
        }

        if (!await db.Testimonials.AnyAsync())
        {
            db.Testimonials.AddRange(
                new Testimonial { Name = "Client Name", Designation = "Business Owner", Message = "Replace this sample testimonial in Admin > Testimonials with real client feedback.", PhotoUrl = Img + "testi-1.jpg", SortOrder = 1 },
                new Testimonial { Name = "Another Client", Designation = "Managing Director", Message = "The team was professional, responsive and genuinely interested in our success.", PhotoUrl = Img + "testi-1.jpg", SortOrder = 2 });
        }

        if (!await db.ClientLogos.AnyAsync())
        {
            for (var i = 1; i <= 4; i++)
                db.ClientLogos.Add(new ClientLogo { Name = "Client " + i, ImageUrl = Img + $"client-{i}.png", LinkUrl = "#", SortOrder = i });
        }

        await db.SaveChangesAsync();
    }
}
