using Fateor.Application.Organization.Entities;
using QRCoder;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Data;
using System.Globalization;
using System.IO;

namespace Fateor.Application.Credentials.Services
{
    public sealed class MLVCredentialService : ICredentialService
    {
        private readonly string _url;
        private static readonly Image LogoImage = Image.FromBinaryData(MLVCredential.Logo);

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

            var culture = new CultureInfo("es-ES");

            Document.Create(container =>
            {
                container.Page(page =>
                {

                    page.Size(PageSizes.A4);
                    page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);

                    page.Content()
                        .Layers(layers =>
                        {
                            layers.Layer().Svg(size =>
                            {
                                return $"""
                                    <svg width="{size.Width}" height="{size.Height}"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <line x1="{size.Width + 40}" y1="-40"
                                              x2="{size.Width +20 }" y2="{size.Height+40}"
                                              stroke="black" stroke-width="5" />

                                        <line x1="{size.Width + 20 }" y1="-40"
                                              x2="{size.Width + 40 }" y2="{size.Height+40}"
                                              stroke="#f8b202" stroke-width="5" />
                                    </svg>
                                    """;
                            });

                            layers.PrimaryLayer().Column(column =>
                            {
                                column.Spacing(12);

                                var logoImage = column.Item()
                                    .AlignCenter()
                                    .PaddingTop(2,QuestPDF.Infrastructure.Unit.Centimetre)
                                    .Width(4, QuestPDF.Infrastructure.Unit.Centimetre)
                                    .Image(LogoImage);

                                column.Item()
                                    .AlignCenter()
                                    .Text("Movimiento Libertario de Venezuela")
                                    .FontFamily("Chamelton 10 Blk")
                                    .FontSize(20);

                                column.Item()
                                    .AlignCenter()
                                    .Text("Credencial de Miembro")
                                    .FontFamily("Chamelton 10 Blk")
                                    .FontSize(14);

                                column.Item()
                                    .AlignCenter()
                                    .Text("El Movimiento Libertario de Venezuela certifica que:")
                                    .FontFamily("DejaVu Sans")
                                    .FontSize(14);

                                column.Item()
                                    .AlignCenter()
                                    .Text(memberFullName)
                                    .FontFamily("DejaVu Sans Bold")
                                    .FontSize(14);

                                column.Item()
                                    .Text(text =>
                                    {
                                        text.DefaultTextStyle(x => x
                                           .FontFamily("DejaVu Sans")
                                           .FontSize(14)
                                           );

                                        text.Justify();

                                        text.Span("ha sido miembro de esta organización desde el ");
                                        text.Span(date.ToString("dd 'de' MMMM 'de' yyyy", culture));
                                        text.Span($" y actualmente ostenta la condición de miembro activo adscrito a la Sección {unit}; con número de credencial {memberCode}.");
                                    });

                                QRCodeGenerator qrGenerator = new QRCodeGenerator();

                                QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                                BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
                                byte[] qrCodeBytes = qrCode.GetGraphic(3);
                                var qrImage = Image.FromBinaryData(qrCodeBytes);

                                column.Item()
                                   .AlignCenter()
                                   .Width(4, QuestPDF.Infrastructure.Unit.Centimetre)
                                   .Image(qrImage);

                            });
                        });
                });
            }).GeneratePdf(outputStream);
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
