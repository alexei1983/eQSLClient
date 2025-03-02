using System.Text.RegularExpressions;

namespace org.goodspace.Data.Radio.Eqsl
{
    /// <summary>
    /// 
    /// </summary>
    public partial class EqslResult
    {
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
        public string? Result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Warning { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Information { get; set; }

        static readonly string[] separator = ["<BR>"];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public EqslResult(string result)
        {
            // parse it
            var matches = BodyRegex().Matches(result);
            if (matches != null && matches.Count > 0)
            {
                var match = matches[0].Value;
                var matchParts = match.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                if (matchParts != null)
                {
                    foreach (var part in matchParts)
                    {
                        var partTrim = part.Replace("<BODY>", string.Empty).Replace("</BODY>", string.Empty).Trim();
                        if (string.IsNullOrEmpty(partTrim))
                            continue;

                        if (partTrim.StartsWith("Warning:"))
                            Warning = partTrim[7..].Trim();
                        else if (partTrim.StartsWith("Result:"))
                        {
                            Result = partTrim[7..].Trim();

                            // get the totals from the result string
                            var totalMatchResult = QsoCountRegex().Match(Result);
                            if (totalMatchResult.Success)
                            {
                                var captureText = totalMatchResult.Captures[0].Value;
                                var totalAddedStr = captureText[..(captureText.Length - captureText.IndexOf("out of") - 7)].Trim();
                                var totalRcvdStr = captureText[(captureText.IndexOf("out of") + 7)..].Trim();

                                if (int.TryParse(totalAddedStr, out int totalAdded))
                                    TotalAdded = totalAdded;

                                if (int.TryParse(totalRcvdStr, out int totalRcvd))
                                    TotalReceived = totalRcvd;
                            }

                        }
                        else if (partTrim.StartsWith("Information:"))
                            Information = partTrim[12..].Trim();
                    }
                }
            }
        }

        [GeneratedRegex(@"<BODY[^>]*>((.|[\n\r])*)<\/BODY>")]
        private static partial Regex BodyRegex();
        [GeneratedRegex("[0-9]{1,} out of [0-9]{1,} ")]
        private static partial Regex QsoCountRegex();
    }
}
