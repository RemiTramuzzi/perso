using System.Configuration;

namespace LRMSA.RiotApi.Misc
{
    public class Log
    {
        private static readonly string FilePath = ConfigurationManager.AppSettings["LogFilePath"];

        private readonly bool _logDateTime;
        private readonly string _prefix;

        public Log(bool logDateTime = true, string prefix = ":")
        {
            _logDateTime = logDateTime;
            _prefix = prefix;
        }

        public void Write(string @string)
        {
            //File.AppendAllText(FilePath, string.Format("{0}{1}{2}\r\n",
            //    _logDateTime ? " " + DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss.fff") + " " : string.Empty,
            //    string.IsNullOrWhiteSpace(_prefix) ? string.Empty : " " + _prefix + " ",
            //    @string));
        }
    }
}