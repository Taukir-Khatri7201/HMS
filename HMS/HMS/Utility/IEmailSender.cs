using System.Threading.Tasks;

namespace HMS.Utility
{
    public interface IEmailSender
    {
        Task SendEmail(Message message);
    }
}
