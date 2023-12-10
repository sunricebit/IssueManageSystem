using IMS.ViewModels.Permission;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IMS.Extensions
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
            string? userJsonString = session.GetString("User")!;
            if (userJsonString == null) return null;
            return JsonSerializer.Deserialize<User>(userJsonString);
        }

        public static PermissionViewModel? GetPermission(this ISession session)
        {
            string? userJsonString = session.GetString("User")!;
            if (userJsonString == null) return null;
            var permissionVM = session.GetString("Permission");
            return JsonSerializer.Deserialize<PermissionViewModel>(permissionVM);
        }

    }

}

