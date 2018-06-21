using Microsoft.AspNetCore.Http;
using System.Text;

namespace Blog.Controllers
{
    public class Util
    {
        public static bool GetUsername(ISession session, out string username)
        {
            if (session.TryGetValue("Username", out byte[] value))
            {
                username = Encoding.UTF8.GetString(value);
                return username != null && username.Length > 0;
            }
            username = null;
            return false;
        }
    }
}
