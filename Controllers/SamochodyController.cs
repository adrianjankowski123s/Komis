using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;

using Komis;
using static System.Net.Mime.MediaTypeNames;
using Komis.Models;

namespace Komis.Controllers
{
    public class SamochodyController : Controller
    {
        private KomisContext db = new KomisContext();

        //sprzedajacy domyślne dane
        string imieS = "Adrian";
        string nazwiskoS = "Jankowski";
        string adresS = "Ks. Anny 1/1 18-400 Łomża";
        string nrS = "AAA2334";
        string wydanyS = "Burmistrza miasta Grajewo";
        string peselS = "11121313112";

        


        // GET: Samochody
        public ActionResult Index(string sortOrder, string searchString, string FiltrTyp, string FiltrKolor, string FiltrStan, int? FiltrLiczba_miejsc, int? FiltrLiczba_drzwi, int? FiltrCenaOd, int? FiltrCenaDo, int? FiltrPrzebiegOd, int? FiltrPrzebiegDo, string AutomatValue, string ManualValue, bool Automat = false, bool Manual = false)
        {

            ViewBag.RokSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc3" : "";
            ViewBag.PrzebiegSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc4" : "";
            ViewBag.RodzajPaliwaSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc5" : "";
            ViewBag.MarkaSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc6" : "";
            ViewBag.ModelSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc7" : "";
            ViewBag.KolorSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc9" : "";
            ViewBag.KrajSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc10" : "";
            ViewBag.KwotaSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc11" : "";

            var samochody = from s in db.Samochody
                            select s;

            switch (sortOrder)
            {
                case "name_desc3":
                    samochody = samochody.OrderBy(s => s.Rok);
                    break;
                case "name_desc4":
                    samochody = samochody.OrderBy(s => s.Przebieg);
                    break;
                case "name_desc5":
                    samochody = samochody.OrderBy(s => s.Rodzaj_paliwa);
                    break;
                case "name_desc6":
                    samochody = samochody.OrderBy(s => s.Marka);
                    break;
                case "name_desc7":
                    samochody = samochody.OrderBy(s => s.Model);
                    break;
                case "name_desc9":
                    samochody = samochody.OrderBy(s => s.Kolor);
                    break;
                case "name_desc10":
                    samochody = samochody.OrderBy(s => s.Kraj_pochodzenia);
                    break;
                case "name_desc11":
                    samochody = samochody.OrderBy(s => s.Kwota);
                    break;
                default:
                    samochody = samochody.OrderBy(s => s.Id_samochodu);
                    break;
            }


            if ((!String.IsNullOrEmpty(FiltrCenaDo.ToString())) && (!String.IsNullOrEmpty(FiltrCenaOd.ToString()))
                && (!String.IsNullOrEmpty(FiltrPrzebiegOd.ToString())) && (!String.IsNullOrEmpty(FiltrPrzebiegDo.ToString()))
                )
            {   

                samochody = samochody.Where(
                s => ((s.Kwota < FiltrCenaDo) && (s.Kwota > FiltrCenaOd))
                  &&
                     ((s.Przebieg < FiltrPrzebiegDo) && (s.Przebieg > FiltrPrzebiegOd))
                  
                );
                if (FiltrKolor != "")
                {
                    samochody = samochody.Where(
                    s => (s.Kolor == FiltrKolor)

                    );
                }

                if (FiltrTyp != "")
                {
                    samochody = samochody.Where(
                    s => (s.Typ == FiltrTyp)

                    );
                }
                if (FiltrLiczba_drzwi.ToString() != "")
                {

                    samochody = samochody.Where(
                s => (s.Liczba_drzwi == FiltrLiczba_drzwi)

                );
                }
                if (FiltrLiczba_miejsc.ToString() != "")
                {

                    samochody = samochody.Where(
                s => (s.Liczba_miejsc == FiltrLiczba_miejsc)

                );
                }
                if (FiltrStan != "")
                {

                    samochody = samochody.Where(
                    s => (s.Stan == FiltrStan)

                );
                }
                
               
            }


            if (!String.IsNullOrEmpty(searchString))
            {
                samochody = samochody.Where(
                   s => s.Tytul.Contains(searchString)
                || s.Podtytul.Contains(searchString)
                || s.Rok.ToString().Contains(searchString)
                || s.Przebieg.ToString().Contains(searchString)
                || s.Pojemnosc.ToString().Contains(searchString)
                || s.Rodzaj_paliwa.Contains(searchString)
                || s.Kategoria.Contains(searchString)
                || s.Marka.Contains(searchString)
                || s.Model.Contains(searchString)
                || s.Wersja.Contains(searchString)
                || s.Generacja.Contains(searchString)
                || s.Moc.Contains(searchString)
                || s.Skrzynia_biegow.Contains(searchString)
                || s.Naped.Contains(searchString)
                || s.Vin.Contains(searchString)
                || s.Emisja_co2.Contains(searchString)
                || s.Typ.Contains(searchString)
                || s.Liczba_drzwi.ToString().Contains(searchString)
                || s.Liczba_miejsc.ToString().Contains(searchString)
                || s.Kolor.Contains(searchString)
                || s.Pierwsza_rejestracja.ToString().Contains(searchString)
                || s.Numer_rejestracyjny.Contains(searchString)
                || s.Zarejestrowany_w_polsce.Contains(searchString)
                || s.Stan.Contains(searchString)
                || s.Kraj_pochodzenia.Contains(searchString)
                || s.Wyposazenie.Contains(searchString)
                || s.Kwota.ToString().Contains(searchString)
                );
            }
            

            return View(samochody.ToList());

        }


        //Schowek

        public ActionResult Schowek(int? id)
        {
            List<Samochody> samochodyItems;
            if (Session["SamochodyS"] != null)
            {
                samochodyItems = (List<Samochody>)Session["SamochodyS"];
            }
            else
            {
                samochodyItems = new List<Samochody>();
            }
            Samochody samochod = db.Samochody.Find(id);
            if (samochodyItems.Any(x => x.Id_samochodu == samochod.Id_samochodu))
            {
                TempData["msg"] = "<script>alert('Ten samochód znajduje się już w schowku.');</script>";
                
            }
            else
            {
                samochodyItems.Add(samochod);
            }
            Session["SamochodyS"] = samochodyItems;


            
            return RedirectToAction("Index");
        }


        [HttpGet]

        public ActionResult SchowekClear()
        {
            List<Samochody> samochodyItems;
            samochodyItems = (List<Samochody>)Session["SamochodyS"];
           
            samochodyItems.Clear();
            Session["SamochodyS"] = samochodyItems;

            return RedirectToAction("SchowekErrorEmpty");
        }

        [HttpGet]
        public ActionResult SchowekIndex()
        {
            if (Session["SamochodyS"] == null)
            {
                return RedirectToAction("SchowekErrorEmpty");
            }

            List<Samochody> samochodyItems;
            samochodyItems = (List<Samochody>)Session["SamochodyS"];


            return View(samochodyItems);
        }

        public ActionResult SchowekErrorEmpty()
        {
            return View();
        }

        //Galeria
        [HttpGet]
        public ActionResult Gallery()
        {
            return View();
        }


        //Porównywarka
        [HttpPost]
        public ActionResult CompareCars(int[] checkCompare)
        {

            Samochody samochod1 = db.Samochody.Find(checkCompare[0]);
            Samochody samochod2 = db.Samochody.Find(checkCompare[1]);
            List<Samochody> CarsToCompare = new List<Samochody>();
            CarsToCompare.Add(samochod1);
            CarsToCompare.Add(samochod2);
            return View(CarsToCompare);
        }



        //Kartki za szybę
        

        public FileResult CreateDocKartki(int? id)
        {
            var samochody = db.Samochody.Find(id);
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created   
            string strPDFFileName = string.Format("SamplePdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(40f, 40f, 40f, 40f);

            //file will created in this path  
            string strAttachment = Server.MapPath("~/Downloads/" + strPDFFileName);


            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            var bigFont = FontFactory.GetFont(BaseFont.COURIER, BaseFont.CP1257, 50, Font.BOLD);
            var normalFont = FontFactory.GetFont(BaseFont.COURIER, BaseFont.CP1257, 30);
            var normalBoldFont = FontFactory.GetFont(BaseFont.COURIER, BaseFont.CP1257, 30, Font.BOLD);
            var BoldFontPrice = FontFactory.GetFont(BaseFont.COURIER, BaseFont.CP1257, 65, Font.BOLD);
            string[] arrayOfwypos = samochody.Wyposazenie.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            string wypos = String.Join(", ", arrayOfwypos);
            
            
            var para1 = new Paragraph(samochody.Marka + " " + samochody.Model, bigFont);
            
            para1.Alignment = Element.ALIGN_CENTER;
            Chunk chunk1 = new Chunk("TYP NADWOZIA: ", normalBoldFont);
            Chunk chunk2 = new Chunk(samochody.Typ, normalFont);
            var para2 = new Paragraph();
            para2.SetLeading(50, 0);
            Chunk chunk3 = new Chunk("Rok produkcji: ", normalBoldFont);
            Chunk chunk4 = new Chunk(samochody.Rok.ToString() + " r", normalFont);
            var para3 = new Paragraph();
            para3.SetLeading(34, 0);
            Chunk chunk5 = new Chunk("Przebieg: ", normalBoldFont);
            Chunk chunk6 = new Chunk(samochody.Przebieg.ToString() + " km", normalFont);
            var para4 = new Paragraph();
            para4.SetLeading(34, 0);
            Chunk chunk7 = new Chunk("Paliwo: ", normalBoldFont);
            Chunk chunk8 = new Chunk(samochody.Rodzaj_paliwa, normalFont);
            var para5 = new Paragraph();
            para5.SetLeading(34, 0);
            Chunk chunk9 = new Chunk("Pojemność: ", normalBoldFont);
            Chunk chunk10 = new Chunk(samochody.Pojemnosc.ToString() + " cm3", normalFont);
            var para6 = new Paragraph();
            para6.SetLeading(34, 0);
            Chunk chunk11 = new Chunk("Moc: ", normalBoldFont);
            Chunk chunk12 = new Chunk(samochody.Moc.ToString() + " KM", normalFont);
            var para7 = new Paragraph();
            para7.SetLeading(34, 0);
            Chunk chunk13 = new Chunk("Wyposażenie: ", normalBoldFont);
            Chunk chunk14 = new Chunk(wypos, normalFont);
            var para8 = new Paragraph();
            para8.SetLeading(34, 0);
            var para = new Paragraph(samochody.Kwota.ToString() + " zł", BoldFontPrice);
            para.SetLeading(100, 0);
            para.Alignment = Element.ALIGN_CENTER;


            para2.Add(chunk1);
            para2.Add(chunk2);
            para2.Alignment = Element.ALIGN_LEFT;
            para3.Add(chunk3);
            para3.Add(chunk4);
            para3.Alignment = Element.ALIGN_LEFT;
            para4.Add(chunk5);
            para4.Add(chunk6);
            para4.Alignment = Element.ALIGN_LEFT;
            para5.Add(chunk7);
            para5.Add(chunk8);
            para5.Alignment = Element.ALIGN_LEFT;
            para6.Add(chunk9);
            para6.Add(chunk10);
            para6.Alignment = Element.ALIGN_LEFT;
            para7.Add(chunk11);
            para7.Add(chunk12);
            para7.Alignment = Element.ALIGN_LEFT;
            para8.Add(chunk13);
            para8.Add(chunk14);
            para8.Alignment = Element.ALIGN_LEFT;
           
            doc.Add(para1);
            doc.Add(para2);
            doc.Add(para3);
            doc.Add(para4);
            doc.Add(para5);
            doc.Add(para6);
            doc.Add(para7);
            doc.Add(para8);
            doc.Add(para);
            // Closing the document  
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return File(workStream, "application/pdf", strPDFFileName);

        }


        //Tworzenie umowy kupna-sprzedaży
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult CreatePDF(int? id)
        {
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samochody samochody = db.Samochody.Find(id);
            
            ViewBag.imieS = imieS;
            ViewBag.nazwiskoS = nazwiskoS;
            ViewBag.adresS = adresS;
            ViewBag.nrS = nrS;
            ViewBag.wydanyS = wydanyS;
            ViewBag.peselS = peselS;
            

            return View(samochody);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreatePDF(Samochody samochody, string imieS, string nazwiskoS, string adresS, string nrS, string wydanyS, string peselS,
            string imieK, string nazwiskoK, string adresK, string nrK, string wydanyK, string peselK, string przebieg)
        {
            return CreateDoc(samochody, imieS,nazwiskoS, adresS, nrS, wydanyS, peselS,  imieK,  nazwiskoK,  adresK,  nrK,  wydanyK,  peselK, przebieg);
        }


        public FileResult CreateDoc(Samochody samochody, string imieS, string nazwiskoS, string adresS, string nrS, string wydanyS, string peselS, 
            string imieK, string nazwiskoK, string adresK, string nrK, string wydanyK, string peselK, string przebieg)
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created   
            string strPDFFileName = string.Format("SamplePdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(40f, 40f, 40f, 40f);
           
            //file will created in this path  
            string strAttachment = Server.MapPath("~/Downloads/" + strPDFFileName);


            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            var bigFont = FontFactory.GetFont(BaseFont.COURIER, BaseFont.CP1257, 18, Font.BOLD);
            var normalFont = FontFactory.GetFont(BaseFont.COURIER, BaseFont.CP1257, 12);
            var normalBoldFont = FontFactory.GetFont(BaseFont.COURIER, BaseFont.CP1257, 12, Font.BOLD);
            string dateTime = DateTime.Now.ToString("dd-MM-yyyy");
         
            ViewBag.Marka = samochody.Marka;
            ViewBag.Model = samochody.Model;
            ViewBag.Wersja = samochody.Wersja;
            ViewBag.Generacja = samochody.Generacja;
            ViewBag.Rok = samochody.Rok;
            ViewBag.Vin = samochody.Vin;
            ViewBag.Przebieg = przebieg;
            ViewBag.Kolor = samochody.Kolor;
            ViewBag.Kwota = samochody.Kwota;
            var para1 = new Paragraph("Umowa kupna-sprzedaży samochodu", bigFont);
            para1.Alignment = Element.ALIGN_CENTER;
            var para2 = new Paragraph("zawarta w Łomży dnia " + dateTime + "r. pomiędzy:", normalFont);
            para2.Alignment = Element.ALIGN_CENTER;
            var para3 = new Paragraph("Sprzedającym:", normalBoldFont);
            var para4 = new Paragraph(imieS + " " + nazwiskoS + ". Adres: " + adresS + ". Dokument tożsamości numer: " + nrS 
                + ", wydany przez: " + wydanyS + ". Pesel/Nip: " + peselS, normalFont);
            var para5 = new Paragraph("a", normalFont);
            var para6 = new Paragraph("Kupującym:", normalBoldFont);
            var para7 = new Paragraph(imieK + " " + nazwiskoK + ". Adres: " + adresK + ". Dokument tożsamości numer: " + nrK
                + ", wydany przez: " + wydanyK + ". Pesel/Nip: " + peselK, normalFont);
            var para8 = new Paragraph("&1", normalBoldFont);
            para8.Alignment = Element.ALIGN_CENTER;
            var para9 = new Paragraph("Przedmiotem umowy jest sprzedaż samochodu:",  normalFont);
            var para10 = new Paragraph("marka: " + samochody.Marka + " " + samochody.Model + " " + samochody.Wersja + " " + samochody.Generacja + ", rok produkcji: " + samochody.Rok + "r.", normalFont);
            var para11 = new Paragraph("nr nadwozia: " + samochody.Vin + ", " + "numer rejestracyjny: " + samochody.Numer_rejestracyjny, normalFont);
            var para12 = new Paragraph("przebieg: " + przebieg + "km, " + "kolor: " + samochody.Kolor + ".", normalFont);
            var para13 = new Paragraph("&2", normalBoldFont);
            para13.Alignment = Element.ALIGN_CENTER;
            var para14 = new Paragraph("Sprzedający oświadcza, że samochód będący przedmiotem niniejszej umowy stanowi jego własność, nie toczy się wobec niego żadne postępowanie w związku z ww.samochodem, samochód jest wolny od wad prawnych i praw osób trzecich oraz nie stanowi przedmiotu zabezpieczenia.", normalFont) ;
            var para15 = new Paragraph("&3", normalBoldFont);
            para15.Alignment = Element.ALIGN_CENTER;
            var para16 = new Paragraph("Strony ustaliły wartość przedmiotu niniejszej umowy na kwotę: " + samochody.Kwota + "zł", normalFont);
            var para17 = new Paragraph("&4", normalBoldFont);
            para17.Alignment = Element.ALIGN_CENTER;
            var para18 = new Paragraph("Kupujący oświadcza, że dokładnie zapoznał się ze stanem technicznym nabywanego samochodu i nie ma do niego żadnych zastrzeżeń.", normalFont);
            var para19 = new Paragraph("&5", normalBoldFont);
            para19.Alignment = Element.ALIGN_CENTER;
            var para20 = new Paragraph("Strony ustaliły, że wszystkie koszty wynikające z zawarcia niniejszej umowy ponosi Kupujący.  ", normalFont);
            var para21 = new Paragraph("&6", normalBoldFont);
            para21.Alignment = Element.ALIGN_CENTER;
            var para22 = new Paragraph("1. Wszelkie zmiany i uzupełnienia niniejszej umowy wymagają formy pisemnej pod rygorem nieważności.", normalFont);
            var para23 = new Paragraph("2. W sprawach nieuregulowanych niniejszą umową zastosowanie mają odpowiednie przepisy Kodeksu cywilnego. ", normalFont);
            var para24 = new Paragraph("3. Umowę sporządzono w dwóch jednobrzmiących egzemplarzach, po jednym dla każdej ze stron.", normalFont);
            var para25 = new Paragraph(" ", normalFont);
            var para26 = new Paragraph("........................                    ........................ ", normalFont);
            var para27 = new Paragraph("       Sprzedający                                   Kupujący", normalFont);


            doc.Add(para1);
            doc.Add(para2);
            doc.Add(para3);
            doc.Add(para4);
            doc.Add(para5);
            doc.Add(para6);
            doc.Add(para7);
            doc.Add(para8);
            doc.Add(para9);
            doc.Add(para10);
            doc.Add(para11);
            doc.Add(para12);
            doc.Add(para13);
            doc.Add(para14);
            doc.Add(para15);
            doc.Add(para16);
            doc.Add(para17);
            doc.Add(para18);
            doc.Add(para19);
            doc.Add(para20);
            doc.Add(para21);
            doc.Add(para22);
            doc.Add(para23);
            doc.Add(para24);
            doc.Add(para25);
            doc.Add(para26);
            doc.Add(para27);
            // Closing the document  
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return File(workStream, "application/pdf", strPDFFileName);

        }


        // GET: Samochody/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samochody samochody = db.Samochody.Find(id);
            if (samochody == null)
            {
                return HttpNotFound();
            }
            return View(samochody);
        }

        // GET: Samochody/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.WyposazenieV = new SelectList(db.Wyposazenie, "Nazwa_wyposazenia", "Nazwa_wyposazenia");

            return View();
        }

        // POST: Samochody/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_samochodu,Tytul,Podtytul,Rok,Przebieg,Pojemnosc,Rodzaj_paliwa,Kategoria," +
            "Marka,Model,Wersja,Generacja,Moc,Skrzynia_biegow,Naped,Vin,Emisja_co2,Typ,Liczba_drzwi,Liczba_miejsc,Kolor," +
            "Pierwsza_rejestracja,Numer_rejestracyjny,Zarejestrowany_w_polsce,Stan,Kraj_pochodzenia," +
            "Opis,Wyposazenie,Kwota")] Samochody samochody, string _wyposazenie, Zdjecia zdjecia, IEnumerable<HttpPostedFileBase> uploadImages)
        {

            if (ModelState.IsValid)
            {
                samochody.Wyposazenie = _wyposazenie;
                db.Samochody.Add(samochody);
                if (uploadImages != null)
                { 
                    var imageList = new List<Zdjecia>();
                    foreach (var image in uploadImages)
                    {
                        if (image != null)
                        {
                            using (var br = new BinaryReader(image.InputStream))
                            {
                                var data = br.ReadBytes(image.ContentLength);
                                var img = new Zdjecia
                                {
                                    Id_samochodu = samochody.Id_samochodu,
                                    Nazwa_zdjecia = image.FileName
                                };
                                img.ImageData = data;
                                imageList.Add(img);

                            }
                        }
                    }
                    if (imageList != null)
                    {
                        samochody.Zdjecia = imageList;
                    }

                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            ViewBag.WyposazenieV = new SelectList(db.Wyposazenie, "Nazwa_wyposazenia", "Nazwa_wyposazenia", samochody.Id_samochodu);
            return View(samochody);
        }



        public ActionResult Show(int id)
        {
            var zdjecie = db.Zdjecia.Find(id);
            var imageData = zdjecie.ImageData;
            return File(imageData, "image/jpg");
        }

        // GET: Samochody/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {

            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samochody samochody = db.Samochody.Find(id);
            string wyposazenieE = samochody.Wyposazenie;
            if (wyposazenieE != null)
            {
                string[] words = wyposazenieE.Split(',');
                ViewBag.WyposazenieSelected = new MultiSelectList(db.Wyposazenie, "Nazwa_wyposazenia", "Nazwa_wyposazenia", words);
            }
            else
            {
                ViewBag.WyposazenieSelected = new MultiSelectList(db.Wyposazenie, "Nazwa_wyposazenia", "Nazwa_wyposazenia");
            }
            ViewBag.RodzajP = new SelectList(new string[] { "", "Benzyna", "Diesel", "Benzyna+LPG" }, samochody.Rodzaj_paliwa);
           




            if (samochody == null)
            {
                return HttpNotFound();
            }
            return View(samochody);
        }
        

        // POST: Samochody/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_samochodu,Tytul,Podtytul,Rok,Przebieg,Pojemnosc,Rodzaj_paliwa," +
            "Kategoria,Marka,Model,Wersja,Generacja,Moc,Skrzynia_biegow,Naped,Vin,Emisja_co2,Typ,Liczba_drzwi,Liczba_miejsc," +
            "Kolor,Pierwsza_rejestracja,Numer_rejestracyjny,Zarejestrowany_w_polsce,Stan," +
            "Kraj_pochodzenia,Opis,Wyposazenie,Kwota")] Samochody samochody, string _wyposazenieE, Zdjecia zdjecia, IEnumerable<HttpPostedFileBase> uploadImages1, string[] words)
        {
            ViewBag.WyposazenaieE = new SelectList(db.Wyposazenie, "Nazwa_wyposazenia", "Nazwa_wyposazenia", words);

            if (ModelState.IsValid)
            {
                samochody.Wyposazenie = _wyposazenieE;
                if (uploadImages1 != null)
                {
                    samochody.Zdjecia = null;
                    var imageList = new List<Zdjecia>();
                    foreach (var image in uploadImages1)
                    {
                        if (image != null)
                        {
                            using (var br = new BinaryReader(image.InputStream))
                            {
                                var data = br.ReadBytes(image.ContentLength);
                                var img = new Zdjecia
                                {
                                    Id_samochodu = samochody.Id_samochodu,
                                    Nazwa_zdjecia = image.FileName
                                };
                                img.ImageData = data;
                                imageList.Add(img);

                            }
                        }
                    }
                    if (imageList != null)
                    {
                        samochody.Zdjecia = imageList;
                    }

                }

                ViewBag.WyposazenieV = new SelectList(db.Wyposazenie, "Nazwa_wyposazenia", "Nazwa_wyposazenia");

                ViewBag.WyposazenaieE = new SelectList(db.Wyposazenie, "Nazwa_wyposazenia", "Nazwa_wyposazenia");

                db.Entry(samochody).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
          

            return View(samochody);
        }

        // GET: Samochody/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samochody samochody = db.Samochody.Find(id);
            
            if (samochody == null)
            {
                return HttpNotFound();
            }

            return View(samochody);
        }

        // POST: Samochody/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Samochody samochody = db.Samochody.Find(id);
            var zdjecia = db.Zdjecia.Include(z => z.Samochody);
            
            foreach (var zdjecie in zdjecia)
            {
                if (zdjecie.Id_samochodu == samochody.Id_samochodu)
                {
                    db.Zdjecia.Remove(zdjecie);
                }
            }
           
            db.Samochody.Remove(samochody);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
