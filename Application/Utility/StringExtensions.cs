namespace Application.Utility
{
    public static class StringExtensions
    {
        public static Uri RelativeUri(this string str)
        {
            return new Uri(str, UriKind.Relative);
        }
    }
}
