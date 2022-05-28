using System;
using System.Text.RegularExpressions;

namespace eQSLClient {

  /// <summary>
  /// 
  /// </summary>
  public class eQSLResult {

    /// <summary>
    /// 
    /// </summary>
    public int? TotalReceived { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? TotalAdded { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Result { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Warning { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Information { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="result"></param>
    public eQSLResult(string result)
    {
      // parse it
      var regex = new Regex(@"<BODY[^>]*>((.|[\n\r])*)<\/BODY>");
      var matches = regex.Matches(result);
      if (matches != null && matches.Count > 0)
      {
        var match = matches[0].Value;
        var matchParts = match.Split(new string[] { "<BR>" }, StringSplitOptions.RemoveEmptyEntries);
        if (matchParts != null)
        {
          foreach (var part in matchParts)
          {
            var partTrim = part.Replace("<BODY>", string.Empty).Replace("</BODY>", string.Empty).Trim();
            if (string.IsNullOrEmpty(partTrim))
              continue;

            if (partTrim.StartsWith("Warning:"))
              Warning = partTrim.Substring(7).Trim();
            else if (partTrim.StartsWith("Result:"))
            {
              Result = partTrim.Substring(7).Trim();

              // get the totals from the result string
              var totalMatchResult = Regex.Match(Result, "[0-9]{1,} out of [0-9]{1,} ");
              if (totalMatchResult.Success)
              {
                var captureText = totalMatchResult.Captures[0].Value;
                var totalAddedStr = captureText.Substring(0, captureText.Length - captureText.IndexOf("out of") - 7).Trim();
                var totalRcvdStr = captureText.Substring(captureText.IndexOf("out of") + 7).Trim();

                if (int.TryParse(totalAddedStr, out int totalAdded))
                  TotalAdded = totalAdded;

                if (int.TryParse(totalRcvdStr, out int totalRcvd))
                  TotalReceived = totalRcvd;
              }

            } else if (partTrim.StartsWith("Information:"))
              Information = partTrim.Substring(12).Trim();
          }
        }
      }
    }
  }
}
