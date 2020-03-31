using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Font.Constants;
using iText.Layout.Properties;
using iText.Kernel.Geom;
using iText.IO.Image;
using HelpdeskViewModels;
using System.Collections.Generic;
using System;
using iText.Layout.Borders;

namespace CasestudyWebsite.Reports
{
    public class CallReport
    {
        public void generatedReport(string rootpath)
        {
            PageSize pg = PageSize.A4.Rotate();
            var helvetica = PdfFontFactory.CreateFont(StandardFontFamilies.HELVETICA);
            Image img = new Image(ImageDataFactory.Create(rootpath + "/img/helpdesk_1.jpg"))
                .ScaleAbsolute(150, 100)
                .SetFixedPosition(((pg.GetWidth()) / 2)-85, pg.GetHeight()-120);


            PdfWriter writer = new PdfWriter(rootpath + "/pdfs/calllist.pdf",
                                new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0));

            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf,pg);

            document.Add(img);
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("Current Calls")
                    .SetFont(helvetica)
                    .SetFontSize(24)
                    .SetBold()
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

            Table table = new Table(6);
            table
                .SetWidth(700)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetRelativePosition(0, 0, 0, 0)
                .SetHorizontalAlignment(HorizontalAlignment.CENTER);

            table.AddCell(addCell("Opened", "h", 0));
            table.AddCell(addCell("Lastname", "h", 0));
            table.AddCell(addCell("Tech", "h", 0));
            table.AddCell(addCell("Problem", "h", 0));
            table.AddCell(addCell("Status", "h", 0));
            table.AddCell(addCell("Closed", "h", 0));
            table.AddCell(addCell(" ", "d"));
            table.AddCell(addCell(" ", "d"));
            table.AddCell(addCell(" ", "d"));
            table.AddCell(addCell(" ", "d"));
            table.AddCell(addCell(" ", "d"));
            table.AddCell(addCell(" ", "d"));

            CallViewModel call = new CallViewModel();
            List<CallViewModel> calls = call.GetAll();

            foreach (CallViewModel c in calls)
            {
                //table.AddCell(addCell(emp.Title, "d", 8));
                table.AddCell(addCell(c.DateOpened.ToShortDateString(), "d", 8));
                table.AddCell(addCell(c.EmployeeName, "d"));
                table.AddCell(addCell(c.TechName, "d"));
                table.AddCell(addCell(c.ProblemDescription, "d"));
                
                if(c.OpenStatus == false)
                    table.AddCell(addCell("Open", "d"));
                else
                    table.AddCell(addCell("Closed", "d"));

                if(c.DateClosed == null)
                    table.AddCell(addCell("-", "d"));
                else 
                    table.AddCell(addCell(c.DateClosed ? .ToString("MM/dd/yyyy"),"d"));
            }

            document.Add(table);
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("Call report written on - " + DateTime.Now)
                        .SetFontSize(6)
                        .SetTextAlignment(TextAlignment.CENTER));

            document.Close();

        }

        private Cell addCell(string data, string celltype, int padLeft = 16)
        {
            Cell cell;

            if (celltype == "h")
            {
                cell = new Cell().Add(
                    new Paragraph(data)
                    .SetFontSize(14)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBold()
                    )
                    .SetBorder(Border.NO_BORDER);
            }
            else
            {
                cell = new Cell().Add(
                    new Paragraph(data)
                    .SetFontSize(12)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetPaddingLeft(padLeft)
                    )
                    .SetBorder(Border.NO_BORDER);
            }
            return cell;
        }
    }
}
