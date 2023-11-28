using System.Text.Json;
using System.Text.Json.Serialization;
using IMS.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Services
{
    public static class SessionExtensions
    {
        public static void SetUser(this ISession session, User user)
        {
            string userJsonString = JsonSerializer.Serialize(user!, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });

            session.SetString("User", userJsonString);
        }

        public static User? GetUser(this ISession session)
        {
            string userJsonString = session.GetString("User")!;
            return JsonSerializer.Deserialize<User>(userJsonString);
        }
    }
}