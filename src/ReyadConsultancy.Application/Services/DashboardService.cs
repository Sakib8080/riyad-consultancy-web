using Microsoft.EntityFrameworkCore;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Domain.Enums;

namespace ReyadConsultancy.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IApplicationDbContext _db;

    public DashboardService(IApplicationDbContext db) => _db = db;

    public async Task<DashboardModel> GetAsync() => new DashboardModel
    {
        Services = await _db.Set<Service>().CountAsync(),
        BlogPosts = await _db.Set<BlogPost>().CountAsync(),
        UnreadMessages = await _db.Set<ContactMessage>().CountAsync(m => !m.IsRead),
        PendingAppointments = await _db.Set<Appointment>().CountAsync(a => a.Status == AppointmentStatus.Pending),
        PendingComments = await _db.Set<BlogComment>().CountAsync(c => !c.IsApproved),
        Subscribers = await _db.Set<NewsletterSubscriber>().CountAsync(s => s.IsActive),
        RecentMessages = await _db.Set<ContactMessage>().AsNoTracking().OrderByDescending(m => m.CreatedAt).Take(5).ToListAsync(),
        RecentAppointments = await _db.Set<Appointment>().AsNoTracking().OrderByDescending(a => a.CreatedAt).Take(5).ToListAsync()
    };
}
