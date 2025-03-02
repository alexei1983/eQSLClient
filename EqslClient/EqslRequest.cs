using Llc.GoodConsulting.Web.EnhancedWebRequest;

namespace org.goodspace.Data.Radio.Eqsl
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class EqslRequest
    {
        /// <summary>
        /// 
        /// </summary>
        readonly HttpClient client;

        /// <summary>
        /// 
        /// </summary>
        public string User { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public string? UserAgent { get; set; }


        const string URL = "https://www.eQSL.cc/qslcard/ImportADIF.cfm";
        const string EQSL_USER_FIELD = "EQSL_USER";
        const string EQSL_PASSWORD_FIELD = "EQSL_PSWD";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        public EqslRequest(string user, string password)
        {
            if (string.IsNullOrEmpty(user))
                throw new ArgumentException("Username is required.", nameof(user));

            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password is required.", nameof(password));

            User = user;
            Password = password;

            client = new HttpClient();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        protected virtual HttpRequestMessage GetRequest(IDictionary<string, string>? parameters = null)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, URL);

            if (parameters?.Count > 0)
                req = req.WithQueryString(parameters);

            if (!string.IsNullOrEmpty(User) && !string.IsNullOrEmpty(Password))
            {
                var body = new Dictionary<string, string>()
            {
                { EQSL_USER_FIELD,  User},
                { EQSL_PASSWORD_FIELD, Password},
            };

                req = req.WithFormValues(body);
            }
            return req;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        protected virtual async Task<HttpResponseMessage> GetResponse(IDictionary<string, string>? parameters = null)
        {
            return await client.SendAsync(GetRequest(parameters));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        protected virtual EqslResult? GetResult(string response)
        {
            if (!string.IsNullOrEmpty(response))
            {
                var result = new EqslResult(response);
                return result;
            }
            return default;
        }

        public abstract Task<EqslResult?> Execute();
    }
}
