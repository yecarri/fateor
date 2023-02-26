using Fateor.Application.Organization.Entities;
using MigraDocCore.DocumentObjectModel;
using MigraDocCore.Rendering;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using QRCoder;
using System.Collections;

namespace Fateor.Application.Credentials.Services
{
    public sealed class MLVCredentialService : ICredentialService
    {
        private readonly string _url;

        public MLVCredentialService(string url)
        {
            _url=url;
        }

        private void Generate(Stream outputStream,
                              string memberFullName,
                              string memberType,
                              DateTime date,
                              string unit,
                              string memberCode,
                              string url)
        {
            const string FontName = "Geosans Light";
            if (!(GlobalFontSettings.FontResolver is MLVCredentialFontResolver))
            {
                GlobalFontSettings.FontResolver = new MLVCredentialFontResolver();
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            }

            /* Root document */
            var pdfDocument = new PdfDocument();
            var page = pdfDocument.AddPage();
            page.Size = PageSize.Letter;

            /* Fonts */
            var normalFont = new Font(FontName, 14);
            normalFont.Color = Colors.Black;

            var titleFont = new Font(FontName, 20);
            titleFont.Color = Colors.Black;
            titleFont.Bold = true;

            var nameFont = new Font(FontName, 14);
            nameFont.Bold = true;

            /* Ornaments */
            var maxx = page.Width;
            var maxy = page.Height;
            var width = XUnit.FromMillimeter(5);
            var gap = XUnit.FromMillimeter(3);

            var document = new Document();
            var section = document.AddSection();

            var title = section.AddParagraph("Credencial de Miembro");
            title.Format.Alignment = ParagraphAlignment.Center;
            title.Format.Font = titleFont.Clone();

            var para1 = section.AddParagraph("El Movimiento Libertario de Venezuela certifica que:");
            para1.Format.Alignment = ParagraphAlignment.Left;
            para1.Format.Font = normalFont.Clone();

            var paraNombre = section.AddParagraph(memberFullName.ToUpper());
            paraNombre.Format.Alignment = ParagraphAlignment.Center;
            paraNombre.Format.Font = nameFont.Clone();

            var para2 = section.AddParagraph(
                $"ha sido miembro de esta organización desde el {date:dd 'de' MMMM 'de' yyyy} " +
                $"y actualmente ostenta la condición de miembro {memberType} " +
                $"adscrito a la Sección {unit}; con número de credencial {memberCode}.");

            para2.Format.Alignment = ParagraphAlignment.Justify;
            para2.Format.Font = normalFont.Clone();

            var render = new DocumentRenderer(document);

            var top = XUnit.FromInch(1);
            var line = XUnit.FromPoint(14);
            var marginX = XUnit.FromInch(1);
            var textWidth = maxx - marginX * 2;
            render.PrepareDocument();

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XGraphicsPath path = new XGraphicsPath();
            XPen black = new XPen(XColors.Black, width);
            path.AddLine(maxx - 0.5f * width, 0, maxx - 1.5f * width - gap, maxy);
            gfx.DrawPath(black, path);

            path = new XGraphicsPath();
            XPen yellow = new XPen(XColor.FromArgb(239, 183, 27), width);
            path.AddLine(maxx - 1.5f * width - gap, 0, maxx - 0.5f * width, maxy);
            gfx.DrawPath(yellow, path);




            using (var imageStream = new MemoryStream(MLVCredential.Logo))
            {
                XImage image = XImage.FromStream(() => imageStream);

                double logoHeight = 80;
                double logoWidth = logoHeight * image.PointWidth / image.PointHeight;

                gfx.DrawImage(image, (maxx - logoWidth) / 2, top, logoWidth, logoHeight);
            }
            render.RenderObject(gfx, marginX, top + 5 * line, textWidth, title);
            render.RenderObject(gfx, marginX, top + 9 * line, textWidth, para1);
            render.RenderObject(gfx, marginX, top + 11 * line, textWidth, paraNombre);
            render.RenderObject(gfx, marginX, top + 13 * line, textWidth, para2);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            byte[] qrCodeBytes = qrCode.GetGraphic(3);

            var stream = new MemoryStream(qrCodeBytes);
            stream.Seek(0, SeekOrigin.Begin);
            XImage qrImage = XImage.FromStream(() => stream);


            var qrPosX = (maxx - XUnit.FromPoint(qrImage.PointWidth)) / 2;
            var qrPosY = (maxy - XUnit.FromPoint(qrImage.PointHeight)) / 2;
            gfx.DrawImage(qrImage, qrPosX, qrPosY);
            stream.Dispose();

            var paraUrl = section.AddParagraph(url);
            paraUrl.Format.Alignment = ParagraphAlignment.Center;
            paraUrl.Format.Font = normalFont.Clone();

            render.RenderObject(gfx, marginX, qrPosY + XUnit.FromPoint(qrImage.PointHeight) + line, textWidth, paraUrl);

            pdfDocument.Save(outputStream);
        }


        public (bool, IList<string>) Generate(Stream stream, Member member)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            try
            {
                var status = member.MemberStatuses.OrderBy(s => s.StatusDate).FirstOrDefault(s => s.IsActiveMember);

                if (status == null)
                    return (false, new List<string>() { "Nunca ha sido miembro activo" });

                string memberFullName = member.FullName;
                string memberType = member.LastStatus.StatusDescription;
                DateTime date = status.StatusDate;
                string unit = status.UnitName;
                string memberCode = member.MemberCode;
                Generate(stream, memberFullName, memberType, date, unit, memberCode, _url);
                return (true, new List<string>());
            }
            catch (Exception ex)
            {
                return (false, new List<string>() { ex.Message });
            }
        }
    }
}
