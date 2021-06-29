using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azbit_Robotu
{
    public class ArzTalep
    {
        public int timestamp { get; set; }
        public string currencyPairCode { get; set; }
        public double price { get; set; }
        public double price24hAgo { get; set; }
        public double priceChangePercentage24h { get; set; }
        public double volume24h { get; set; }
        public double bidPrice { get; set; }
        public double askPrice { get; set; }
    }
}
