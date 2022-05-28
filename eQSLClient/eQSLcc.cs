using System;
using ADIF.NET;

namespace eQSLClient {

  /// <summary>
  /// 
  /// </summary>
  public class eQSLcc {

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
    public string UserAgent { get; set; } = "eQSLClient";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    public eQSLcc(string user, string password)
    {
      User = user;
      Password = password;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="qso"></param>
    public eQSLResult Upload(ADIFQSO qso)
    {
      if (qso == null)
        throw new ArgumentNullException(nameof(qso), "QSO cannot be null.");

      return Upload(new ADIFQSO[] { qso });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="qsos"></param>
    public eQSLResult Upload(params ADIFQSO[] qsos)
    {
      if (qsos == null || qsos.Length < 1)
        throw new ArgumentException("At least one QSO is required.", nameof(qsos));

      return new eQSLUploadRequest(User, Password, qsos) { UserAgent = UserAgent }.Execute();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="qsos"></param>
    public eQSLResult Upload(ADIFQSOCollection qsos)
    {
      if (qsos == null)
        throw new ArgumentNullException(nameof(qsos), "QSO collection cannot be null.");

      return Upload(qsos.ToArray());
    }
  }
}
