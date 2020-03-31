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
    public class EmployeeReport
    {
        public void generatedReport(string rootpath)
        {
            PageSize pg = PageSize.A4;
            var helvetica = PdfFontFactory.CreateFont(StandardFontFamilies.HELVETICA);
            Image img = new Image(ImageDataFactory.Create(rootpath + "/img/helpdesk_1.jpg"))
                .ScaleAbsolute(150, 100)
                .SetFixedPosition(((pg.GetWidth() - 220) / 2), 710);


            PdfWriter writer = new PdfWriter(rootpath + "/pdfs/employeelist.pdf",
                                new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0));

            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);
            document.Add(img);
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("Current Employees")
                    .SetFont(helvetica)
                    .SetFontSize(24)
                    .SetBold()
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

            //document.Add(new Paragraph(""));
            //document.Add(new Paragraph(""));

            Table table = new Table(3);
            table
                .SetWidth(298)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetRelativePosition(0, 0, 0, 0)
                .SetHorizontalAlignment(HorizontalAlignment.CENTER);

            table.AddCell(addCell("Title", "h", 0));
            table.AddCell(addCell("Firstname", "h", 0));
            table.AddCell(addCell("Lastname", "h", 0));
            table.AddCell(addCell(" ", "d"));
            table.AddCell(addCell(" ", "d"));
            table.AddCell(addCell(" ", "d"));

            EmployeeViewModel employee = new EmployeeViewModel();
            List<EmployeeViewModel> employees = employee.GetAll();

            foreach (EmployeeViewModel emp in employees)
            {
                table.AddCell(addCell(emp.Title, "d", 8));
                table.AddCell(addCell(emp.Firstname, "d"));
                table.AddCell(addCell(emp.Lastname, "d"));
            }

            document.Add(table);
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("Employee report written on - " + DateTime.Now)
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
                    .SetFontSize(16)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBold()
                    )
                    .SetBorder(Border.NO_BORDER);
            }
            else
            {
                cell = new Cell().Add(
                    new Paragraph(data)
                    .SetFontSize(14)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetPaddingLeft(padLeft)
                    )
                    .SetBorder(Border.NO_BORDER);
            }
            return cell;
        }
    }
}
