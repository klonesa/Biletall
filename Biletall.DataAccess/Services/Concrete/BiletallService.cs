using Biletall.DataAccess.Services.Abstraction;
using Biletall.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using WebService;

namespace Biletall.DataAccess.Services.Concrete
{
    public class BiletallService : IBiletallService
    {
        private readonly String _userName="stajyerWS";
        private readonly String _password="2324423WSs099";

        public async Task<List<KaraNokta>> KaraNoktaList()
        {
            try
            {
                XmlIsletRequestBody xirb = new XmlIsletRequestBody();
                XmlDocument xml = new XmlDocument();
                xml.LoadXml($"<Kullanici><Adi>{_userName}</Adi><Sifre>{_password}</Sifre></Kullanici>");
                xirb.xmlYetki = xml.DocumentElement;

                XmlDocument xml2 = new XmlDocument();
                xml2.LoadXml(@"<KaraNoktaGetirKomut/>");

                xirb.xmlIslem = xml2.DocumentElement;

                var service = await new ServiceSoapClient(ServiceSoapClient.EndpointConfiguration.ServiceSoap).XmlIsletAsync(xirb.xmlIslem, xirb.xmlYetki);

                List<KaraNokta> model = new List<KaraNokta>();

                XmlNodeList xmlNodeList = service.Body.XmlIsletResult.SelectNodes("/KaraNokta");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    KaraNokta karaNokta = new KaraNokta
                    {
                        ID = xmlNode["ID"].InnerText,
                        Ad = xmlNode["Ad"].InnerText,
                        Aciklama = xmlNode["Aciklama"].InnerText,
                        BagliOlduguNoktaID = xmlNode["BagliOlduguNoktaID"].InnerText,
                        Bolge = xmlNode["Bolge"].InnerText,
                        MerkezMi = xmlNode["MerkezMi"].InnerText,
                        SeyahatSehirID = xmlNode["SeyahatSehirID"].InnerText
                    };
                    if (karaNokta.MerkezMi == "1")
                    {
                        model.Add(karaNokta);
                    }

                }
                return model;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<Sefer>> SeferList(string nereden, string nereye, DateTime tarih)
        {
            try
            {
                XmlIsletRequestBody xirb = new XmlIsletRequestBody();
                XmlDocument xml = new XmlDocument();
                xml.LoadXml($"<Kullanici><Adi>{_userName}</Adi><Sifre>{_password}</Sifre></Kullanici>");
                xirb.xmlYetki = xml.DocumentElement;

                XmlDocument xml2 = new XmlDocument();
                xml2.LoadXml(@"<Sefer><FirmaNo>0</FirmaNo><KalkisNoktaID>" + nereden + "</KalkisNoktaID><VarisNoktaID>" + nereye +
                    "</VarisNoktaID><Tarih>" + tarih.ToString("yyyy-MM-dd") + "</Tarih><AraNoktaGelsin>1</AraNoktaGelsin><IslemTipi>0</IslemTipi><YolcuSayisi>1</YolcuSayisi><Ip>127.0.0.1</Ip></Sefer>");
                xirb.xmlIslem = xml2.DocumentElement;

                var service = await new ServiceSoapClient(ServiceSoapClient.EndpointConfiguration.ServiceSoap).XmlIsletAsync(xirb.xmlIslem, xirb.xmlYetki);

                List<Sefer> model = new List<Sefer>();

                XmlNodeList xmlNodeList = service.Body.XmlIsletResult.SelectNodes("/Table");

                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    Sefer sefer = new Sefer
                    {
                        FirmaNo=xmlNode["FirmaNo"].InnerText,
                        ID = xmlNode["ID"].InnerText,
                        BiletFiyati1 = Convert.ToInt32(xmlNode["BiletFiyati1"].InnerText),
                        BiletFiyatiInternet = Convert.ToInt32(xmlNode["BiletFiyatiInternet"].InnerText),
                        FirmaAdi = xmlNode["FirmaAdi"].InnerText,
                        KalkisNokta = xmlNode["KalkisNokta"].InnerText,
                        OTipOzellik = xmlNode["OTipOzellik"].InnerText,
                        VarisNokta = xmlNode["VarisNokta"].InnerText,
                        YaklasikSeyahatSuresi = xmlNode["YaklasikSeyahatSuresi"].InnerText,
                        OtobusKoltukYerlesimTipi = xmlNode["OtobusKoltukYerlesimTipi"].InnerText,
                        SeferTakipNo = xmlNode["SeferTakipNo"].InnerText,
                        SeyahatSuresi=ConvertDatetime(xmlNode["SeyahatSuresi"].InnerText),
                        SeyehatTarihi=CustomTime(xmlNode["Tarih"].InnerText),
                        SeferTipiAciklamasi=xmlNode["SeferTipiAciklamasi"].InnerText
                    };
                    sefer.Guzergahlar = await GuzergahList(nereden, nereye, tarih, sefer.SeferTakipNo);
                    sefer.KalkisSaati = ConvertDatetime(xmlNode["Saat"].InnerText);
                    model.Add(sefer);
                }

                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<Guzergah>> GuzergahList(string nereden, string nereye, DateTime tarih, string seferTakipNo)
        {
            try
            {
                XmlIsletRequestBody xirb = new XmlIsletRequestBody();
                XmlDocument xml = new XmlDocument();
                xml.LoadXml($"<Kullanici><Adi>{_userName}</Adi><Sifre>{_password}</Sifre></Kullanici>");
                xirb.xmlYetki = xml.DocumentElement;

                XmlDocument xml2 = new XmlDocument();
                xml2.LoadXml(@"<Hat><FirmaNo>37</FirmaNo><HatNo>1</HatNo><KalkisNoktaID>" + nereden + "</KalkisNoktaID><VarisNoktaID>" + nereye + "</VarisNoktaID><BilgiIslemAdi>GuzergahVerSaatli</BilgiIslemAdi>" +
                    "<SeferTakipNo>" + seferTakipNo + "</SeferTakipNo><Tarih>" + tarih.ToString("yyyy-MM-dd") + "</Tarih></Hat>");
                xirb.xmlIslem = xml2.DocumentElement;

                var service = await new ServiceSoapClient(ServiceSoapClient.EndpointConfiguration.ServiceSoap).XmlIsletAsync(xirb.xmlIslem, xirb.xmlYetki);

                List<Guzergah> model = new List<Guzergah>();

                XmlNodeList xmlNodeList = service.Body.XmlIsletResult.SelectNodes("/Table1");

                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    Guzergah sfr = new Guzergah
                    {
                        VarisYeri = xmlNode["VarisYeri"].InnerText,
                        SiraNo = xmlNode["SiraNo"].InnerText,
                        KalkisTarihSaat = xmlNode["KalkisTarihSaat"].InnerText,
                        VarisTarihSaat = xmlNode["VarisTarihSaat"].InnerText,
                        KaraNoktaID = xmlNode["KaraNoktaID"].InnerText,
                        KaraNoktaAd = xmlNode["KaraNoktaAd"].InnerText
                    };
                    DateTime kts, vts;
                    sfr.KalkisTarihSaat = DateTime.TryParse(xmlNode["KalkisTarihSaat"].InnerText, out kts) ? kts.Hour.ToString() + ":" + kts.Minute.ToString("00") : "";
                    sfr.VarisTarihSaat = DateTime.TryParse(xmlNode["VarisTarihSaat"].InnerText, out vts) ? vts.Hour.ToString() + ":" + vts.Minute.ToString("00") : "";

                    model.Add(sfr);
                }
                return model;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #region PRIVATE
        /// <summary>
        /// Xml to Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public T DeserializeToObject<T>(string filepath) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(filepath))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        public string ConvertDatetime(string date)
        {
            DateTime ss;
            return DateTime.TryParse(date, out ss) ? ss.Hour.ToString() + ":" + ss.Minute.ToString("00") : "";
        }

        public string CustomTime(string date)
        {
            var dateTime=Convert.ToDateTime(date);
            return dateTime.Day + " " + dateTime.ToString("MMM") + " " + dateTime.Year + " " + dateTime.DayOfWeek;
        }

        #endregion
    }
}
