using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static TamsMobile.Models.ReportingModel;
using Cell = iText.Layout.Element.Cell;
using TextAlignment = iText.Layout.Properties.TextAlignment;
using VerticalAlignment = iText.Layout.Properties.VerticalAlignment;

namespace TamsMobile.Services
{
    public class ExportService : BaseApiService
    {
        private readonly ILogger<ExportService>? _logger;

        public ExportService(HttpClient httpClient, ILogger<ExportService>? logger = null)
            : base(httpClient)
        {
            _logger = logger;
        }

        public async Task DownloadAndSharePdf(List<ReportBulananIndividuResponse> data)
        {
            try
            {
                if (data == null || data.Count == 0)
                {
                    await Shell.Current.DisplayAlert("Error", "No data available to generate report.", "OK");
                    return;
                }

                byte[] pdfBytes = CreateAttendancePdf(data);

                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    await Shell.Current.DisplayAlert("Error", "PDF generation failed (empty file).", "OK");
                    return;
                }

                string fileName = $"Laporan_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
                string? filePath = await SavePdfAsync(pdfBytes, fileName);

                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    //await Application.Current.MainPage.DisplayAlert("Error", "File was not saved correctly.", "OK");
                    await Shell.Current.DisplayAlert("Error", "File was not saved correctly.", "OK");
                    return;
                }

                // DEBUG (optional)
                System.Diagnostics.Debug.WriteLine($"PDF saved at: {filePath}");

                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = "Download Laporan PDF",
                    File = new ShareFile(filePath)
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: {ex}");
                await Shell.Current.DisplayAlert("Exception", ex.Message, "OK");
            }
        }

        public byte[] CreateAttendancePdf(List<ReportBulananIndividuResponse> data)
        {
            try
            {
                using MemoryStream ms = new MemoryStream();

                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf, PageSize.A4.Rotate());

                document.Add(new Paragraph("LAPORAN KEHADIRAN BULANAN")
                    .SimulateBold()
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(16));

                if (data == null || data.Count == 0)
                {
                    document.Add(new Paragraph("No data available"));
                }
                else
                {
                    // Add Header Info
                    AddHeaderInfo(document, data);

                    // Add Table Data
                    AddTableData(document, data);
                }

                // Add Footer
                AddFooter(document);

                document.Close();

                return ms.ToArray();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"PDF Error: {ex}");
                Application.Current?.Windows.FirstOrDefault()?.Page?.DisplayAlert("Data Error", ex.Message, "OK");
                return Array.Empty<byte>();
            }
        }

        public async Task<string?> SavePdfAsync(byte[] data, string fileName)
        {
            try
            {
                string filePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, fileName);

                File.WriteAllBytes(filePath, data);

                // Verify file
                if (!File.Exists(filePath))
                    throw new Exception("File write failed.");

                return filePath;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Save Error: {ex}");
                await Shell.Current.DisplayAlert("Save Error", ex.Message, "OK");
                return null;
            }
        }

        private static string FormatMinutes(int totalMinutes)
        {
            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;

            return $"{hours:D2} Jam {minutes:D2} Minit";
        }
        private static Cell CreateNoBorderCell(string text)
        {
            return new Cell()
                .Add(new Paragraph(text))
                .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
                .SetPadding(2).SetTextAlignment(TextAlignment.LEFT);
        }
        private static Cell HeaderCell(string text)
        {
            return new Cell()
                .Add(new Paragraph(text)
                    .SimulateBold()
                    .SetFontSize(10))
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .SetPadding(5)
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY);
        }

        private static void AddHeaderInfo(Document document, List<ReportBulananIndividuResponse> data)
        {
            Table infoTable = new Table(2).UseAllAvailableWidth();
            infoTable.AddCell(CreateNoBorderCell($"Name : {data.FirstOrDefault()?.Nama}"));
            infoTable.AddCell(CreateNoBorderCell($"WARNA KAD : {data.FirstOrDefault()?.SumWarnaKad}"));

            infoTable.AddCell(CreateNoBorderCell($"MyKAD : {data.FirstOrDefault()?.MykadNumber}"));
            infoTable.AddCell(CreateNoBorderCell($"BULAN LAPORAN : {data.FirstOrDefault()?.GetParsedAttendanceDate?.ToString("MMMM, yyyy")}"));
            infoTable.AddCell(CreateNoBorderCell($"JABATAN : {data.FirstOrDefault()?.JabatanName}"));
            infoTable.AddCell(CreateNoBorderCell($"TARIKH CETAK : {DateTime.Now.ToString("dd/MM/yyyy hh:mm tt")}"));

            document.Add(infoTable);
        }

        private static void AddTableData(Document document, List<ReportBulananIndividuResponse> data)
        {
            Table table = new Table(9).UseAllAvailableWidth();
            table.SetFontSize(8);

            table.AddHeaderCell(HeaderCell("Tarikh"));
            table.AddHeaderCell(HeaderCell("WBB"));
            table.AddHeaderCell(HeaderCell("Status"));
            table.AddHeaderCell(HeaderCell("Waktu Masuk"));
            table.AddHeaderCell(HeaderCell("Waktu Keluar"));
            table.AddHeaderCell(HeaderCell("Keterangan"));
            table.AddHeaderCell(HeaderCell("Status Permohonan"));
            table.AddHeaderCell(HeaderCell("Masa Kerja"));
            table.AddHeaderCell(HeaderCell("Masa Lebih"));

            // ===== TOTAL =====
            int totalMinutesWork = 0;
            int totalMinutesOT = 0;

            foreach (var item in data)
            {
                string AttendanceTagInStr = item.ParsedAttendanceTagIn.TimeOfDay == TimeSpan.Zero ? "" : item.ParsedAttendanceTagIn.ToString("HH:mm:ss");
                string AttendanceTagOutStr = item.ParsedAttendanceTagOut.TimeOfDay == TimeSpan.Zero ? "" : item.ParsedAttendanceTagOut.ToString("HH:mm:ss");

                string OutstationTagInStr = string.IsNullOrEmpty(item.OutstationNewTagOut) ? "" : $"({item.OutstationNewTagOut})";
                string OutstationTagOutStr = string.IsNullOrEmpty(item.OutstationNewTagIn) ? "" : $"({item.OutstationNewTagIn})";

                table.AddCell(item.GetParsedAttendanceDate?.ToString("dd/MM/yyyy") ?? "");
                table.AddCell(string.IsNullOrEmpty(item.PeriodName) ? "" : item.PeriodName).SetTextAlignment(TextAlignment.CENTER);
                table.AddCell(string.IsNullOrEmpty(item.AttendanceStatus) ? "" : item.AttendanceStatus).SetTextAlignment(TextAlignment.CENTER);
                table.AddCell($"{AttendanceTagInStr} {OutstationTagInStr}").SetTextAlignment(TextAlignment.CENTER);
                table.AddCell($"{AttendanceTagOutStr} {OutstationTagOutStr}").SetTextAlignment(TextAlignment.CENTER);
                table.AddCell(string.IsNullOrEmpty(item.AttendanceNoteIn) ? "" : item.AttendanceNoteIn);
                table.AddCell(string.IsNullOrEmpty(item.StatusPermohonan) ? "" : item.StatusPermohonan).SetTextAlignment(TextAlignment.CENTER);
                table.AddCell(string.IsNullOrEmpty(item.AttendanceDurationStr) ? "" : item.AttendanceDurationStr);
                table.AddCell(string.IsNullOrEmpty(item.OvertimeDuration) ? "" : item.OvertimeDuration);

                // ✅ GRAND TOTAL SOURCE (MINUTES)
                totalMinutesWork += Convert.ToInt32(item.AttendanceDurationMinutes);
                totalMinutesOT += Convert.ToInt32(item.OvertimeDurationMinutes);
            }

            table.AddCell(new Cell(1, 7)
            .Add(new Paragraph("JUMLAH KESELURUHAN").SimulateBold())
            .SetTextAlignment(TextAlignment.RIGHT)
            .SetBackgroundColor(new DeviceRgb(220, 220, 220)));

            table.AddCell(new Cell()
                .Add(new Paragraph(FormatMinutes(totalMinutesWork)).SimulateBold())
                .SetTextAlignment(TextAlignment.CENTER)
                .SetBackgroundColor(new DeviceRgb(220, 220, 220)));

            table.AddCell(new Cell()
                .Add(new Paragraph(FormatMinutes(totalMinutesOT)).SimulateBold())
                .SetTextAlignment(TextAlignment.CENTER)
                .SetBackgroundColor(new DeviceRgb(220, 220, 220)));

            document.Add(table);

        }

        private static void AddFooter(Document document)
        {
            document.Add(new Paragraph());
            document.Add(new Paragraph());
            document.Add(new Paragraph());
            document.Add(new Paragraph());
            document.Add(new Paragraph());
            document.Add(new Paragraph());
            document.Add(new Paragraph("________________________"));
            document.Add(new Paragraph());
            document.Add(new Paragraph(" TANDATANGAN PENYELIA"));
            document.Add(new Paragraph());
            document.Add(new Paragraph());
            document.Add(new Paragraph());
            document.Add(new Paragraph());
            document.Add(new Paragraph());
            document.Add(new Paragraph());
            document.Add(new Paragraph("Nota : Waktu Masuk/Waktu Keluar di dalam kurungan merujuk kepada waktu permohonan urusan rasmi"));
        }
    }
}
