using System.Threading.Tasks;
using RestEase;

namespace BuilderConsumer.RestEase;

[Header("User-Agent", "RestEase")]
public interface IGitHubApi
{
    [Get("users/{userId}")]
    Task<string> GetUserAsync([Path] string userId);
}