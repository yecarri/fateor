using PdfSharpCore.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Credentials.Services
{
    internal class MLVCredentialFontResolver : IFontResolver
    {
        public string DefaultFontName => "Geosans Light";

        public byte[] GetFont(string faceName)
        {
            switch (faceName)
            {
                case "Geosans Light Italic":
                    return MLVCredential.GEOSANSLIGHT_OBLIQUE;
                    //return GetBytes("GEOSANSLIGHT-OBLIQUE.TTF");

                default:
                    return MLVCredential.GEOSANSLIGHT;
                    //return GetBytes("GEOSANSLIGHT.TTF");
            }

        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (isItalic && familyName.Contains("Geosans"))
                return new FontResolverInfo("Geosans Light Italic", isBold, false);

            return new FontResolverInfo(familyName, isBold, isItalic);
        }

        private byte[] GetBytes(string fontFileName)
        {
            using (Stream fs = new FileStream(fontFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var len = (int)fs.Length;
                var bytes = new byte[len];
                fs.Read(bytes, 0, len);
                return bytes;
            }
        }
    }
}
