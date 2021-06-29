using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Azbit_Robotu
{
    class Program
    {
        static void Main(string[] args)
        {
            Kutuphane kh = new Kutuphane();
            double lot = 0;
            double fiyat = 0;
            int dongu = 100;
            for (int i = 1; i <= dongu; i++)
            {
                if (i % 2 == 1)
                {
                    try
                    {
                        fiyat = kh.IslemHesaplaFiyat();
                        lot = kh.IslemHesaplaLot();
                        string bodyAl = kh.Body("buy", "NVX_USDT", lot, fiyat);
                        kh.IslemAc(bodyAl);
                        Console.WriteLine(i + ". al emri gönderildi.");
                    }
                    catch
                    {
                        Console.WriteLine("1. Hata Oluştu...");
                    }
                }
                if (i % 2 == 0)
                {
                    try
                    {
                        string bodySat = kh.Body("sell", "NVX_USDT", lot, fiyat);
                        kh.IslemAc(bodySat);
                        Console.WriteLine(i + ". emir kapatıldı...");
                        var rand = new Random();
                        Thread.Sleep(rand.Next(60, 100) * 1000);
                    }
                    catch
                    {
                        Console.WriteLine("2. Hata Oluştu...");
                    }
                }
                if (i == dongu)
                {
                    Console.WriteLine("İşlem tamamlandı.");
                    i = 0;
                }
            }
        }
    }
}
