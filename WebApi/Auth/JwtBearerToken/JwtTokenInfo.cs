using System.Text.Json.Serialization;

namespace WebApi.Auth.JwtBearerToken
{
    public class JwtTokenInfo
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("expire_in")]
        public int ExpireIn { get; set; }
    }
}
