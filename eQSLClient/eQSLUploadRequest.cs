using System;
using System.Collections.Generic;
using ADIF.NET;

namespace eQSLClient {

  /// <summary>
  /// 
  /// </summary>
  public class eQSLUploadRequest : eQSLRequest {

    /// <summary>
    /// 
    /// </summary>
    public ADIFQSO[] QSOs { get; set; }

    const string ADIF_FIELD = "ADIFData";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    public eQSLUploadRequest(string user, string password) : base(user, password)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <param name="qsos"></param>
    public eQSLUploadRequest(string user, string password, params ADIFQSO[] qsos) : base(user, password)
    {
      QSOs = qsos;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override eQSLResult Execute()
    {
      if (QSOs == null || QSOs.Length < 1)
        throw new InvalidOperationException("At least one QSO is required.");

      var adifData = string.Empty;

      foreach (var qso in QSOs)
        adifData += qso.ToString("A");

      var request = GetRequest(new Dictionary<string, string>() { { ADIF_FIELD, adifData} });
      return GetResult(GetResponse(request));
    }
  }
}
