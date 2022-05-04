using System.Text;

namespace Calculadora.Hypermedia
{
    public class HyperMediaLink
    {
        public string Rel { get; set; }
        private string herf { get; set; }
        public string Herf
        {
            get
            {
                object _look = new object();
                lock (_look)
                {
                    StringBuilder sb = new StringBuilder(herf);
                    return sb.Replace("%2F", "/").ToString();
                }
            }
            set
            {
                herf = value;
            }
        }
        public string Type { get; set; }
        public string Action { get; set; }
    }
}