using DinkToPdf;
using System;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var converter = new BasicConverter(new PdfTools());
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    //ColorMode = ColorMode.Color,
                    //Orientation = Orientation.Landscape,
                    //PaperSize = PaperKind.A4Plus,
                    Out = @"123.pdf",
                },
                Objects = {
                    new ObjectSettings() {
                        //PagesCount = true,
                        //HtmlContent = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. In consectetur mauris eget ultrices  iaculis. Ut                               odio viverra, molestie lectus nec, venenatis turpis.",
                        //WebSettings = { DefaultEncoding = "utf-8" },
                        //HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                        Page = "http://www.runoob.com/html/html-tutorial.html",
                    }
                }
            };
            converter.Convert(doc);
        }
    }
}
