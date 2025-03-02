
using org.goodspace.Data.Radio.Adif;

namespace org.goodspace.Data.Radio.Eqsl
{
    /// <summary>
    /// 
    /// </summary>
    public class EqslClient
    {
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
        public EqslClient(string user, string password)
        {
            User = user;
            Password = password;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qso"></param>
        public async Task<EqslResult?> Upload(AdifQso qso)
        {
            if (qso == null)
                throw new ArgumentNullException(nameof(qso), "QSO cannot be null.");

            return await Upload(new AdifQso[] { qso });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qsos"></param>
        public async Task<EqslResult?> Upload(params AdifQso[] qsos)
        {
            if (qsos == null || qsos.Length < 1)
                throw new ArgumentException("At least one QSO is required.", nameof(qsos));

            return await new EqslUploadRequest(User, Password, qsos) { UserAgent = UserAgent }.Execute();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qsos"></param>
        public async Task<EqslResult?> Upload(AdifQsoCollection qsos)
        {
            if (qsos == null)
                throw new ArgumentNullException(nameof(qsos), "QSO collection cannot be null.");

            return await Upload(qsos.ToArray());
        }
    }
}
