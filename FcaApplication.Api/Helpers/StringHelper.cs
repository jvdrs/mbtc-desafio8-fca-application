namespace FcaApplication.Api.Helpers
{
    public static class StringHelper
    {
        public static string RemoveAccents(this string valor)
        {
            var bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(valor);

            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}
