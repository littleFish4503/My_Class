using System;
using System.Drawing;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Class_类
{
    

    public static class PDFHelper
    {
        public static void WriteText(string filePath, int page, string text)
        {
            // 打开PDF文件
            PdfReader reader = new PdfReader(filePath);
            FileStream outputStream = new FileStream(filePath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, outputStream);

            // 获取指定页的内容
            PdfContentByte cb = stamper.GetOverContent(page);

            // 写入文本内容
            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(new iTextSharp.text.Rectangle(36, 36, 559, 806));
            ct.AddText(new Phrase(text));
            ct.Go();

            // 保存PDF文件
            stamper.Close();
            reader.Close();
            outputStream.Close();
        }

        public static string ReadText(string filePath, int page)
        {
            // 打开PDF文件
            PdfReader reader = new PdfReader(filePath);

            // 读取指定页的文本内容
            string text = PdfTextExtractor.GetTextFromPage(reader, page, new SimpleTextExtractionStrategy());

            // 关闭PDF文件
            reader.Close();

            // 返回文本内容
            return text;
        }
    }
}
