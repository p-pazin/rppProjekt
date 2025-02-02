using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarchiveAPI.Dto;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using ServiceLayer.Network.Dto;

namespace PresentationLayer.helpers
{
    public class PdfHelper
    {
        public byte[] GenerateSaleContractPdf(SaleContractDto contract)
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = $"Kupoprodajni ugovor {contract.ContactName} {contract.DateOfCreation}";

            if (contract.Vehicles != null && contract.Vehicles.Any())
            {
                foreach (var vehicle in contract.Vehicles)
                {
                    PdfPage page = document.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);

                    AddSaleContractPage(gfx, contract, vehicle);
                }
            }
            else
            {
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                AddSaleContractPage(gfx, contract, contract.Vehicle);
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                document.Save(memoryStream, false);
                return memoryStream.ToArray();
            }
        }

        private void AddSaleContractPage(XGraphics gfx, SaleContractDto contract, VehicleDto vehicle)
        {
            XFont boldFont = new XFont("Verdana", 15, XFontStyle.Bold);
            XFont regularFont = new XFont("Verdana", 12);
            XFont smallFont = new XFont("Verdana", 10);

            float startX = 55f;
            float lineY = 90f;
            float lineSpacing = 20f;

            string title = "UGOVOR O KUPOPRODAJI MOTORNOG VOZILA";
            gfx.DrawString(title, boldFont, XBrushes.Black, new XPoint(gfx.PageSize.Width / 2 - gfx.MeasureString(title, boldFont).Width / 2, 50));

            gfx.DrawLine(new XPen(XColors.Black), startX, lineY, 550f, lineY);
            lineY += lineSpacing;
            gfx.DrawString($"Prodavatelj (Ime i prezime / Pravna osoba): {contract.CompanyName}", regularFont, XBrushes.Black, new XPoint(startX, lineY));
            gfx.DrawString($"OIB: {contract.CompanyPin}", regularFont, XBrushes.Black, new XPoint(450f, lineY));
            lineY += lineSpacing;

            gfx.DrawString($"Adresa: {contract.CompanyAddress}", regularFont, XBrushes.Black, new XPoint(startX, lineY));
            lineY += lineSpacing;

            gfx.DrawLine(new XPen(XColors.Black), startX, lineY, 550f, lineY);
            lineY += lineSpacing;

            gfx.DrawString($"Kupac (Ime i prezime / Pravna osoba): {contract.ContactName}", regularFont, XBrushes.Black, new XPoint(startX, lineY));
            gfx.DrawString($"OIB: {contract.ContactPin}", regularFont, XBrushes.Black, new XPoint(450f, lineY));
            lineY += lineSpacing;

            gfx.DrawString($"Adresa: {contract.ContactAddress}", regularFont, XBrushes.Black, new XPoint(startX, lineY));
            lineY += lineSpacing;

            gfx.DrawString($"Datum: {contract.DateOfCreation}", regularFont, XBrushes.Black, new XPoint(startX, lineY));
            gfx.DrawLine(new XPen(XColors.Black), 102f, lineY + 5f, 250f, lineY + 5f);
            gfx.DrawString($"Mjesto: {contract.Place}", regularFont, XBrushes.Black, new XPoint(300f, lineY));
            gfx.DrawLine(new XPen(XColors.Black), 345f, lineY + 5f, 550f, lineY + 5f);
            lineY += lineSpacing;
            lineY += lineSpacing;

            gfx.DrawString("1. Prodavatelj prodaje kupcu motorno vozilo:", smallFont, XBrushes.Black, new XPoint(startX, lineY));
            lineY += lineSpacing;

            gfx.DrawString($"Marka vozila: {vehicle?.Brand} {vehicle?.Model}", smallFont, XBrushes.Black, new XPoint(startX + 10, lineY));
            gfx.DrawString($"Zapremnina motora: {vehicle?.CubicCapacity}cc", smallFont, XBrushes.Black, new XPoint(startX + 250f, lineY));
            lineY += lineSpacing;

            gfx.DrawString($"Registracijska oznaka: {vehicle?.Registration}", smallFont, XBrushes.Black, new XPoint(startX + 10, lineY));
            gfx.DrawString($"Registriran do: {vehicle?.RegisteredTo}", smallFont, XBrushes.Black, new XPoint(startX + 250f, lineY));
            lineY += lineSpacing;

            gfx.DrawString($"2. Prodajna cijena vozila iz članka 1. ugovorena je u iznosu od {vehicle?.Price} eura.", smallFont, XBrushes.Black, new XPoint(startX, lineY));
            lineY += lineSpacing;

            gfx.DrawString("3. Kupac je iznos isplatio prodavatelju u cijelosti.", smallFont, XBrushes.Black, new XPoint(startX, lineY));
            lineY += lineSpacing;

            gfx.DrawString("4. Prodavatelj jamči da je vozilo njegovo vlasništvo i da nije opterećeno.", smallFont, XBrushes.Black, new XPoint(startX, lineY));
            lineY += lineSpacing;

            gfx.DrawString("5. Kupac je pregledao vozilo i nema primjedbi.", smallFont, XBrushes.Black, new XPoint(startX, lineY));
            lineY += lineSpacing;

            gfx.DrawString("6. Kupac preuzima vozilo: Ključevi, prometna dozvola", smallFont, XBrushes.Black, new XPoint(startX, lineY));
            gfx.DrawLine(new XPen(XColors.Black), 185f, lineY + 5f, 550f, lineY + 5f);
            lineY += lineSpacing;

            gfx.DrawString("7. Porez i ostale troškove prijenosa snosi kupac.", smallFont, XBrushes.Black, new XPoint(startX, lineY));
            lineY += lineSpacing;

            gfx.DrawString($"8. U slučaju spora, nadležan je sud u: {contract.Place}", smallFont, XBrushes.Black, new XPoint(startX, lineY));
            gfx.DrawLine(new XPen(XColors.Black), 250f, lineY + 5f, 550f, lineY + 5f);
            lineY += lineSpacing;
            lineY += lineSpacing;

            gfx.DrawString("Prodavatelj:", smallFont, XBrushes.Black, new XPoint(100f, lineY));
            gfx.DrawLine(new XPen(XColors.Black), 50f, lineY + 50f, 250f, lineY + 50f);
            gfx.DrawString("Kupac:", smallFont, XBrushes.Black, new XPoint(400f, lineY));
            gfx.DrawLine(new XPen(XColors.Black), 350f, lineY + 50f, 550f, lineY + 50f);
        }
        public byte[] GenerateRentContractPdf(RentContractDto contract)
        {
            PdfDocument pdfDocument = new PdfDocument();
            pdfDocument.Info.Title = $"Najmodavni ugovor {contract.FirstNameContact} {contract.LastNameContact} {contract.DateOfCreation}";

            var boldFont = new XFont("Arial", 14, XFontStyle.Bold);
            var normalFont = new XFont("Arial", 12);

            var page = pdfDocument.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            float yPosition = 50f;
            const float lineSpacing = 25f;
            const float leftMargin = 55f;
            const float maxLineWidth = 500f;

            gfx.DrawString("UGOVOR O NAJMU AUTOMOBILA", boldFont, XBrushes.Black, new XPoint(200, yPosition));
            yPosition += lineSpacing * 2;

            string introduction = $@"
            “{contract.Name} iz {contract.City}, {contract.Address}, OIB: {contract.Pin}
            zastupano po direktoru {contract.FirstNameDirector} {contract.LastNameDirector}
            i
            {contract.FirstNameContact} {contract.LastNameContact}, OIB: {contract.PinContact}, iz {contract.CityContact}, {contract.AddressContact},
            sklopili su u {contract.City} {contract.DateOfCreation}
            ";

            yPosition = WriteTextBlock(introduction, gfx, normalFont, leftMargin, yPosition, lineSpacing, maxLineWidth);

            var articles = new List<string>
            {
                $"Članak 1.\nIznajmi i vrati auto d.o.o. (u daljnjem tekstu: iznajmljivač) iznajmljuje osobno vozilo {contract.FirstNameContact} {contract.LastNameContact} (u daljnjem tekstu: korisnik).",
                $"Članak 2.\nOsobni automobil iz članka 1 ovog ugovora je:\n{contract.Brand} {contract.Model}, {contract.Engine}, registracijskih oznaka {contract.Registration}.",
                $"Članak 3.\nOvim ugovorom se ugovara da je korisnik jedina ovlaštena osoba koja će koristiti osobni automobil iz članka 2 ovog ugovora, za vrijeme trajanja najma.",
                $"Članak 4.\nKorisnik je stariji od 22 godine i posjeduje važeću vozačku dozvolu najmanje 2 godine.",
                $"Članak 5.\nOsobni automobil iz članka 2 se iznajmljuje korisniku na vrijeme od {contract.StartDate} do {contract.EndDate}.",
                $"Članak 6.\nNajamnina za osobni automobil iz članka 2 iznosi {contract.Price}€ po danu najma.",
                $"Članak 7.\nKorisniku je vozilo predano u voznom tj. ispravnom stanju zajedno sa svom potrebnom opremom i dijelovima.",
                $"Članak 8.\nKorisnik se obvezuje da neće:\n– koristiti predmet najma radi obavljanja bilo kakvih radnji koje bi se mogle okarakterizirati kao radnje u suprotnosti sa zakonima koji su na snazi u zemlji najma.",
                $"Članak 9.\nKorisnik ne može otuđiti osobno vozilo koje je predmet najma ili bilo koji dio navedenog vozila.",
                $"Članak 10.\nZa vrijeme trajanja najma korisnik je obvezan se brinuti o vozilu, kao i poduzimati sve potrebne radnje radi osiguranja tehničke ispravnosti samoga osobnog automobila.",
                $"Članak 11.\nKorisnik snosi sve troškove koji su nastali normalnom upotrebom predmeta najma kao što su: prometni prekršaji, naknade za parkiranje i/ili garažiranje osobnog vozila, naknade za upotrebu autocesta, troškovi goriva i drugi slični troškovi.",
                $"Članak 12.\nKorisnik se obvezuje vratiti osobni automobil zajedno sa svom opremom i dijelovima, te u istom stanju u kojem ga je i zaprimio.",
                $"Članak 13.\nSvako oštećenje ili promjena stanja osobnog automobila koje je nastalo tijekom trajanja najma, korisnik je obvezan naznačiti tj. informirati iznajmljivača prilikom vraćanja samog vozila.",
                $"Članak 14.\nKorisnik se obvezuje da će snositi troškove čišćenja automobila ukoliko se u trenutku vračanja utvrdi da je vanjština i/ili unutrašnjost vozila posebno prljava.",
                $"Članak 15.\nUkoliko korisnik izgubi ključeve vozila koje je predmet najma, on se obvezuje nadoknaditi štetu u iznosu od 150€.",
                $"Članak 16.\nNajam osobnog vozila se može produžiti najkasnije 24 sata prije isteka roka iz članka 5 ovog ugovora.",
                $"Članak 17.\nIznajmljivač nije odgovoran za štetu nastalu gubitkom, krađom ili oštećenjem imovine korisnika koja se nalazi u vozilu za vrijeme trajanja najma i nakon završetka najma.",
                $"Članak 18.\nOsobni automobil iz članka 2. je osigurano za slučaj štete prouzrokovane trećim osobama.",
                $"Članak 19.\nIznajmljivač ima pravo uvida u stanje osobnog vozila za vrijeme trajanja samog najma.",
                $"Članak 20.\nIznajmljivač pridržava pravo vozilo oduzeti, bez prethodne obavijesti i o trošku korisnika, ukoliko se osobno vozilo koristi protivno ugovorenim uvjetima ovog ugovora.",
                $"Članak 21.\nUgovorne strane su suglasne da će eventualne sporove rješavati sporazumom.",
                $"Članak 22.\nUgovorne strane prihvaćaju sva prava i obveze koje proizlaze iz ovog ugovora stavljanjem vlastoručnih potpisa na isti.",
                $"Članak 23.\nOvaj ugovor je napravljen u 2 istovjetna primjerka. Svaka ugovorna strana zadržava po jedan primjerak."
            };

            foreach (var article in articles)
            {
                yPosition = WriteTextBlock(article, gfx, normalFont, leftMargin, yPosition, lineSpacing, maxLineWidth);

                if (yPosition > 850f)
                {
                    page = pdfDocument.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    yPosition = 50f;
                }
            }

            using (var ms = new MemoryStream())
            {
                pdfDocument.Save(ms);
                return ms.ToArray();
            }
        }

        private float WriteTextBlock(string text, XGraphics gfx, XFont font, float x, float y, float lineSpacing, float maxLineWidth)
        {
            var lines = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                string[] words = line.Split(' ');
                string currentLine = "";

                foreach (var word in words)
                {
                    string testLine = currentLine + word + " ";
                    XSize textSize = gfx.MeasureString(testLine, font);

                    if (textSize.Width > maxLineWidth)
                    {
                        gfx.DrawString(currentLine, font, XBrushes.Black, new XPoint(x, y));
                        y += lineSpacing;
                        currentLine = word + " ";
                    }
                    else
                    {
                        currentLine = testLine;
                    }
                }

                if (!string.IsNullOrEmpty(currentLine))
                {
                    gfx.DrawString(currentLine, font, XBrushes.Black, new XPoint(x, y));
                    y += lineSpacing;
                }

                y += lineSpacing * 0.5f;
            }

            return y;
        }
        public byte[] GenerateSaleInvoicePdf(SaleContractDto contract, InvoiceDto invoice)
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = $"Račun prodaja ID: {invoice.Id}";

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont boldFont = new XFont("Verdana", 15, XFontStyle.Bold);
            XFont regularFont = new XFont("Verdana", 12);
            XFont smallFont = new XFont("Verdana", 10);

            float startX = 55f;
            float lineY = 90f;
            float lineSpacing = 20f;

            string title = "PRODAJNI RAČUN";
            gfx.DrawString(title, boldFont, XBrushes.Black, new XPoint(gfx.PageSize.Width / 2 - gfx.MeasureString(title, boldFont).Width / 2, 50));

            gfx.DrawLine(new XPen(XColors.Black), startX, lineY, 550f, lineY);
            lineY += lineSpacing;

            gfx.DrawString($"Prodavatelj: {contract.CompanyName}", regularFont, XBrushes.Black, new XPoint(startX, lineY));
            gfx.DrawString($"OIB: {contract.CompanyPin}", regularFont, XBrushes.Black, new XPoint(400f, lineY)); 
            lineY += lineSpacing;

            gfx.DrawString($"Adresa: {contract.CompanyAddress}", regularFont, XBrushes.Black, new XPoint(startX, lineY));
            lineY += lineSpacing;

            gfx.DrawLine(new XPen(XColors.Black), startX, lineY, 550f, lineY);
            lineY += lineSpacing;

            gfx.DrawString($"Kupac: {contract.ContactName}", regularFont, XBrushes.Black, new XPoint(startX, lineY));
            gfx.DrawString($"OIB: {contract.ContactPin}", regularFont, XBrushes.Black, new XPoint(400f, lineY)); 
            lineY += lineSpacing;

            gfx.DrawString($"Adresa: {contract.ContactAddress}", regularFont, XBrushes.Black, new XPoint(startX, lineY));
            lineY += lineSpacing;

            gfx.DrawLine(new XPen(XColors.Black), startX, lineY, 550f, lineY);
            lineY += lineSpacing;

            gfx.DrawString($"Datum kreiranja: {invoice.DateOfCreation}", regularFont, XBrushes.Black, new XPoint(startX, lineY));
            gfx.DrawString($"Način plaćanja: {invoice.PaymentMethod}", regularFont, XBrushes.Black, new XPoint(400f, lineY)); 
            lineY += lineSpacing;

            gfx.DrawString($"PDV: {invoice.Vat}% ", regularFont, XBrushes.Black, new XPoint(startX, lineY));
            gfx.DrawString($"Ukupno: {invoice.TotalCost} EUR", regularFont, XBrushes.Black, new XPoint(400f, lineY));
            lineY += lineSpacing;

            gfx.DrawLine(new XPen(XColors.Black), startX, lineY, 550f, lineY);
            lineY += lineSpacing;

            gfx.DrawString("Vozila prodana:", boldFont, XBrushes.Black, new XPoint(startX, lineY));
            lineY += lineSpacing;

            if(contract.Vehicle != null)
            {
                gfx.DrawString($"Model: {contract.Vehicle.Brand} {contract.Vehicle.Model}", smallFont, XBrushes.Black, new XPoint(startX, lineY));
                gfx.DrawString($"Cijena: {contract.Vehicle.Price} EUR", smallFont, XBrushes.Black, new XPoint(400f, lineY));
                lineY += lineSpacing;

                gfx.DrawString($"Registracija: {contract.Vehicle.Registration}", smallFont, XBrushes.Black, new XPoint(startX, lineY));
                lineY += lineSpacing;
            }
            else
            {
                foreach (var vehicle in contract.Vehicles)
                {
                    gfx.DrawString($"Model: {vehicle.Brand} {vehicle.Model}", smallFont, XBrushes.Black, new XPoint(startX, lineY));
                    gfx.DrawString($"Cijena: {vehicle.Price} EUR", smallFont, XBrushes.Black, new XPoint(400f, lineY));
                    lineY += lineSpacing;

                    gfx.DrawString($"Registracija: {vehicle.Registration}", smallFont, XBrushes.Black, new XPoint(startX, lineY));
                    lineY += lineSpacing;
                }
            }

            gfx.DrawLine(new XPen(XColors.Black), startX, lineY, 550f, lineY);
            lineY += lineSpacing;

            gfx.DrawString($"Kilometraža: {invoice.Mileage} km", regularFont, XBrushes.Black, new XPoint(startX, lineY));
            lineY += lineSpacing;

            gfx.DrawString($"ID Ugovora: {contract.Id}", regularFont, XBrushes.Black, new XPoint(startX, lineY));
            lineY += lineSpacing;

            gfx.DrawString($"Račun izdao: {contract.UserName}", regularFont, XBrushes.Black, new XPoint(startX, lineY));
            lineY += lineSpacing;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                document.Save(memoryStream, false);
                return memoryStream.ToArray();
            }
        }
    }
}
