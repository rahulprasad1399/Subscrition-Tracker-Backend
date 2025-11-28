using System.ComponentModel.DataAnnotations;
using Trackify.SubscriptionTracker.Domain.Entity;

public class UserNotification
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public int SubscriptionId { get; private set; }

    [Required]
    public string Title { get; private set; }

    [Required]
    public string Type { get; private set; }

    public bool IsRead { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? ReadAt { get; private set; }

    public User User { get; private set; }
    public Subscription Subscription { get; private set; }

    private UserNotification() { }

    public UserNotification(int userId, int subscriptionId, string title, string type)
    {
        UserId = userId;
        SubscriptionId = subscriptionId;
        Title = title;
        Type = type;
        IsRead = false; 
    }

    public void MarkAsRead()
    {
        if (!IsRead)
        {
            IsRead = true;
            ReadAt = DateTime.UtcNow;
        }
    }
}
