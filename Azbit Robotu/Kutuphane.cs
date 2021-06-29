using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Azbit_Robotu
{
    public class Kutuphane
    {
        string publicKey = "2ue0DaFBfXEVrWT34SNrquEk9kuS5fXXAnc1aQ";
        string privateKey = "hgP5xHAy5o6GxDNi15CBeoVCWQKBqC2SwRNnHpLnFGTnoxfZ-a-9e581IUWAAFDMT7bL8Q";
        public double IslemHesaplaFiyat()
        {
            double altIslemFiyati = ArzTalepFiyat("Bid");
            double ustIslemFiyati = ArzTalepFiyat("Ask");
            double aralik = ustIslemFiyati - altIslemFiyati;
            var rand = new Random();
            int aralikint = Convert.ToInt32(ustIslemFiyati - altIslemFiyati);
            double rastgeleAralik = altIslemFiyati + ((rand.Next(0, aralikint + 1) + rand.NextDouble()) % aralik);
            Console.WriteLine("Alt İşlem Fiyatı: " + altIslemFiyati + " Üst İşlem Fiyatı: " + ustIslemFiyati + " Aralık: " + aralik);
            Console.WriteLine("İşlem Fiyatı: " + rastgeleAralik);
            return rastgeleAralik;
        }
        public double IslemHesaplaLot()
        {
            var rand = new Random();
            double rastgeleLot = (rand.Next(10, 40)) + (rand.NextDouble());
            Console.WriteLine("İşlem Miktarı: " + rastgeleLot);
            return rastgeleLot;
        }
        public string Body(string islemYonu, string parite, double islemBuyuklugu, double islemFiyati)
        {
            return JsonConvert.SerializeObject(new Body()
            {
                side = islemYonu,
                currencyPairCode = parite,
                amount = islemBuyuklugu,
                price = islemFiyati
            });
        }
        public void IslemAc(string body)
        {
            string requestUrl = "https://data.azbit.com/api/orders";
            string signature = new HMACSHA256(Encoding.UTF8.GetBytes(privateKey))
                    .ComputeHash(Encoding.UTF8.GetBytes(publicKey + requestUrl + body))
                    .Aggregate(new StringBuilder(), (sb, b) => sb.AppendFormat("{0:x2}", b), (sb) => sb.ToString());
            var client = new RestClient("https://data.azbit.com/api/orders");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("API-PublicKey", publicKey);
            request.AddHeader("API-Signature", signature);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }
        public double ArzTalepFiyat(string askBid)
        {
            string requestUrl = "https://data.azbit.com/api/tickers?currencyPairCode=NVX_USDT";
            string signature = new HMACSHA256(Encoding.UTF8.GetBytes(privateKey))
                    .ComputeHash(Encoding.UTF8.GetBytes(publicKey + requestUrl))
                    .Aggregate(new StringBuilder(), (sb, b) => sb.AppendFormat("{0:x2}", b), (sb) => sb.ToString());
            var clientAskBid = new RestClient("https://data.azbit.com/api/tickers?currencyPairCode=NVX_USDT");
            clientAskBid.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("API-PublicKey", publicKey);
            request.AddHeader("API-Signature", signature);
            IRestResponse response = clientAskBid.Execute(request);
            ArzTalep arztalep = JsonConvert.DeserializeObject<ArzTalep>(response.Content);
            double fiyat = 0;
            if (askBid == "Ask")
                fiyat = Convert.ToDouble(arztalep.askPrice);
            if (askBid == "Bid")
                fiyat = Convert.ToDouble(arztalep.bidPrice);
            return fiyat;
        }
    }
}
