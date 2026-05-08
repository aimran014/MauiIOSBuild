using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TamsMobile.Models
{
    public class ReportingModel
    {
        public class ReportBulananIndividuRequest
        {
            [JsonPropertyName("bulan")]
            public int Bulan { get; set; }

            [JsonPropertyName("tahun")]
            public int Tahun { get; set; }

            [JsonPropertyName("userid")]
            public int UserId { get; set; }

            [JsonPropertyName("departmentid")]
            public int DepartmentId { get; set; }

            [JsonPropertyName("hierarchy")]
            public int Hierarchy { get; set; }
        }

        public class ReportBulananIndividuResponse
        {
            [JsonPropertyName("user_id")]
            public int? UserId { get; set; }

            [JsonPropertyName("user_firstname")]
            public string? Nama { get; set; }

            [JsonPropertyName("login_icNo")]
            public string? MykadNumber { get; set; }

            [JsonPropertyName("jabatan_name")]
            public string? JabatanName { get; set; }

            [JsonPropertyName("gred_level")]
            public string? Gred { get; set; }

            [JsonPropertyName("attendance_date")]
            public string? AttendanceDate { get; set; }

            public DateTime ParsedAttendanceDate => string.IsNullOrEmpty(AttendanceDate) ? DateTime.MinValue : DateTime.Parse(AttendanceDate);

            public DateTime? GetParsedAttendanceDate
            {
                get
                {
                    if (string.IsNullOrEmpty(AttendanceDate))
                        return null;

                    if (DateTime.TryParseExact(
                            AttendanceDate,
                            "MM/dd/yyyy HH:mm:ss",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out var result))
                    {
                        return result;
                    }

                    return null;
                }
            }

            [JsonPropertyName("period_name")]
            public string? PeriodName { get; set; }

            [JsonPropertyName("report_month")]
            public string? ReportMonth { get; set; }

            [JsonPropertyName("sum_warna_kad")]
            public string? SumWarnaKad { get; set; }

            [JsonPropertyName("attendance_status")]
            public string? AttendanceStatus { get; set; }

            [JsonPropertyName("status_permohonan")]
            public string? StatusPermohonan { get; set; }

            [JsonPropertyName("attendance_tagIn")]
            public string? AttendanceTagIn { get; set; }

            public DateTime ParsedAttendanceTagIn => string.IsNullOrEmpty(AttendanceTagIn) ? DateTime.MinValue : DateTime.Parse(AttendanceTagIn);

            [JsonPropertyName("attendance_notein")]
            public string? AttendanceNoteIn { get; set; }

            [JsonPropertyName("attendance_tagOut")]
            public string? AttendanceTagOut { get; set; }
            public DateTime ParsedAttendanceTagOut => string.IsNullOrEmpty(AttendanceTagOut) ? DateTime.MinValue : DateTime.Parse(AttendanceTagOut);

            [JsonPropertyName("attendance_noteOut")]
            public string? AttendanceNoteOut { get; set; }

            [JsonPropertyName("attendance_duration_second")]
            public string? AttendanceDurationSecond { get; set; }

            [JsonPropertyName("oldtagin")]
            public string? OldTagIn { get; set; }

            [JsonPropertyName("oldtagout")]
            public string? OldTagOut { get; set; }

            [JsonPropertyName("outstation_note")]
            public string? OutstationNote { get; set; }

            [JsonPropertyName("outstation_newtagin")]
            public string? OutstationNewTagIn { get; set; }

            [JsonPropertyName("outstation_newtagout")]
            public string? OutstationNewTagOut { get; set; }

            [JsonPropertyName("attendance_duration_str")]
            public string? AttendanceDurationStr { get; set; }

            [JsonPropertyName("attendance_duration_minutes")]
            public string? AttendanceDurationMinutes { get; set; }

            [JsonPropertyName("overtime_duration")]
            public string? OvertimeDuration { get; set; }

            [JsonPropertyName("overtime_duration_minutes")]
            public string? OvertimeDurationMinutes { get; set; }

            [JsonPropertyName("insuficient_duration")]
            public string? InsufficientDuration { get; set; }

            [JsonPropertyName("insuficient_duration_minutes")]
            public string? InsuficientDurationMinutes { get; set; }
        }
    }
}
