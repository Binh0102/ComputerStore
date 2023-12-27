using Newtonsoft.Json;

namespace BaitapLon.Models
{
	public static class SessionExtension
	{
		public static void SetObject(this ISession session, string key, object value)
		{
			session.SetString(key, JsonConvert.SerializeObject(value));
		}

		public static List<Cart> GetObject(this ISession session, string key)
		{
			var value = session.GetString(key);
			return value == null ? default(List<Cart>) : JsonConvert.DeserializeObject<List<Cart>>(value);
		}
	}
}
