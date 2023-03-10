using ApplicationCore.Models;
using BusinessService;
using FoodyWebApplication.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace FoodyWebApplication.Helper
{
    public static class SessionExtension
    {
        public static void Set<T>(this ISession session, string key, T? value)
        {
            JsonSerializerOptions option = new JsonSerializerOptions();
            option.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            string json = JsonSerializer.Serialize(value, option);
            session.SetString(key, json);
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            if (value == null)
            {
                return default;
            }
            else
            {
                var result = JsonSerializer.Deserialize<T>(value);
                return result;
            }
        }
        public static Account? GetLoginUser(this ISession session)
        {
            var value = session.GetString("login-user");
            if (value != null)
            {
                return JsonSerializer.Deserialize<Account>(value);
            }
            else
            {
                return null;
            }
        }
        public static bool Logout(this ISession session)
        {
            //Set<Account>(session, "login-user", null);
            session.Clear();
            return true;
        }
        public static CartModel? GetCart(this ISession session)
        {
            var value = session.GetString("GioHang");
            if (value != null)
            {
                return JsonSerializer.Deserialize<CartModel>(value);
            }
            else
            {
                return null;
            }
        }
    }
}