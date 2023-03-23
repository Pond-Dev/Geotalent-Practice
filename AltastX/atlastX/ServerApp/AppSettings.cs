using System.Collections.Generic;

namespace atlastX
{
    public class AppSettings
    {
        public General General { get; set; }
        public DataParser DataParser { get; set; }
    }


    #region Model & Hard Config
    public class General
    {
        public string WebServiceEndpoint { get; set; }
    }

    public class DataParser
    {
        public IEnumerable<string> Paths { get; set; }
    }

    #endregion

}