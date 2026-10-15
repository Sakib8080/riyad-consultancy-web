using Microsoft.EntityFrameworkCore;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Entities;

namespace ReyadConsultancy.Application.Website;

public class WebsiteSubmissionService : IWebsiteSubmissionService
{
    private readonly IApplicationDbContext _db;

    public WebsiteSubmissionService(IApplicationDbContext db) => _db = db;

    public async Task SubmitContactAsync(ContactMessage message)
    {
        message.Id = 0;
        message.IsRead = false;
        _db.Set<ContactMessage>().Add(message);
        await _db.SaveChangesAsync();
    }

    public async Task SubmitAppointmentAsync(Appointment appointment)
    {
        appointment.Id = 0;
        appointment.Status = ReyadConsultancy.Domain.Enums.AppointmentStatus.Pending;
        _db.Set<Appointment>().Add(appointment);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> AddCommentAsync(string postSlug, string name, string email, string? phone, string message)
    {
        var postId = await _db.Set<BlogPost>().Where(p => p.Slug == postSlug && p.IsPublished)
            .Select(p => (int?)p.Id).FirstOrDefaultAsync();
        if (postId == null) return false;

        // Comments are held for moderation: IsApproved=false until an admin approves them.
        _db.Set<BlogComment>().Add(new BlogComment
        {
            BlogPostId = postId.Value, Name = name, Email = email, Phone = phone, Message = message, IsApproved = false
        });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SubscribeAsync(string email)
    {
        email = email.Trim();
        var exists = await _db.Set<NewsletterSubscriber>().AnyAsync(s => s.Email == email);
        if (exists) return false;
        _db.Set<NewsletterSubscriber>().Add(new NewsletterSubscriber { Email = email, IsActive = true });
        await _db.SaveChangesAsync();
        return true;
    }
}
