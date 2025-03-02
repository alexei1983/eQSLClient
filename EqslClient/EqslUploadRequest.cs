using org.goodspace.Data.Radio.Adif;

namespace org.goodspace.Data.Radio.Eqsl
{
    /// <summary>
    /// 
    /// </summary>
    public class EqslUploadRequest : EqslRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public AdifQso[] QSOs { get; set; } = [];

        const string ADIF_FIELD = "ADIFData";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        public EqslUploadRequest(string user, string password) : base(user, password)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="qsos"></param>
        public EqslUploadRequest(string user, string password, params AdifQso[] qsos) : base(user, password)
        {
            QSOs = qsos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override async Task<EqslResult?> Execute()
        {
            if (QSOs == null || QSOs.Length < 1)
                throw new InvalidOperationException("At least one QSO is required.");

            var adifData = string.Empty;

            foreach (var qso in QSOs)
                adifData += qso.ToString("A");

            var response = await GetResponse(new Dictionary<string, string>() { { ADIF_FIELD, adifData } });
            return GetResult(await response.Content.ReadAsStringAsync());
        }
    }
}
