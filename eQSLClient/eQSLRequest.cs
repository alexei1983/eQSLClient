using System;
using System.Collections.Generic;
using System.Net;
using System.IO;

namespace eQSLClient {

  /// <summary>
  /// 
  /// </summary>
  public abstract class eQSLRequest {

    /// <summary>
    /// 
    /// </summary>
    public string User { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string UserAgent { get; set; }

    const string URL = "https://www.eQSL.cc/qslcard/ImportADIF.cfm";
    const string EQSL_USER_FIELD = "EQSL_USER";
    const string EQSL_PASSWORD_FIELD = "EQSL_PSWD";
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    public eQSLRequest(string user, string password)
    {
      User = user;
      Password = password;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameters"></param>
    protected virtual HttpWebRequest GetRequest(IDictionary<string, string> parameters = null)
    {
      if (string.IsNullOrEmpty(User) || string.IsNullOrEmpty(Password))
        throw new InvalidOperationException("User and password are required.");

      var request = (HttpWebRequest)WebRequest.Create(URL);

      if (!string.IsNullOrEmpty(UserAgent))
        request.UserAgent = UserAgent;

      request.Method = "POST";
      request.ContentType = "application/x-www-form-urlencoded";
      string body = $"{EQSL_USER_FIELD}={User}&{EQSL_PASSWORD_FIELD}={Password}";

      if (parameters != null)
      {
        foreach(var parameter in parameters) 
          body += $"&{parameter.Key}={Uri.EscapeDataString(parameter.Value)}";
      }

      using (var writer = new StreamWriter(request.GetRequestStream()))
        writer.Write(body);

      // enable TLS 1.2 protocol
      request.ServicePoint.Expect100Continue = false;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

      return request;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    protected virtual string GetResponse(HttpWebRequest request)
    {
      var response = (HttpWebResponse)request.GetResponse();

      using (var responseStream = response.GetResponseStream())
      {
        if (responseStream == null)
          throw new Exception("Error reading response stream");

        using (var reader = new StreamReader(responseStream))
          return reader.ReadToEnd();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="response"></param>
    protected virtual eQSLResult GetResult(string response)
    {
      if (!string.IsNullOrEmpty(response))
      {
        var result = new eQSLResult(response);
        return result;
      }
      return null;
    }

   public abstract eQSLResult Execute();
  }
}
