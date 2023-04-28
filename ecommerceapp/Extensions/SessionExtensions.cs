using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ecommerceapp.Extensions
{
    public static class SessionExtensions
    {
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
    }
}
