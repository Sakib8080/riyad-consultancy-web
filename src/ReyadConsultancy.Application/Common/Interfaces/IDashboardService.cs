using ReyadConsultancy.Domain.Entities;

namespace ReyadConsultancy.Application.Common.Interfaces;

public class DashboardModel
{
    public int Services { get; set; }
    public int BlogPosts { get; set; }
    public int UnreadMessages { get; set; }
    public int PendingAppointments { get; set; }
    public int PendingComments { get; set; }
    public int Subscribers { get; set; }
    public List<ContactMessage> RecentMessages { get; set; } = new();
    public List<Appointment> RecentAppointments { get; set; } = new();
}

public interface IDashboardService
{
    Task<DashboardModel> GetAsync();
}
