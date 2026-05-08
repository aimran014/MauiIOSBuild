using System.Globalization;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TamsMobile.Models;

public class AttendanceResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("messages")]
    public string Messages { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public List<AttendanceRecord>? RekodKehadiran { get; set; }
}

public class AttendanceRecord
{
    [JsonPropertyName("attendance_id")]
    public int? AttendanceId { get; set; }

    [JsonPropertyName("attendance_on9")]
    public int? AttendanceOn9 { get; set; }

    [JsonPropertyName("myDate")]
    public string? MyDate { get; set; }

    [JsonPropertyName("myDateShiftEnd")]
    public string? MyDateShiftEnd { get; set; }

    [JsonPropertyName("dayNo")]
    public int? DayNo { get; set; }

    [JsonPropertyName("attendance_tagIn")]
    public string? AttendanceTagIn { get; set; }

    [JsonPropertyName("attendance_tagOut")]
    public string? AttendanceTagOut { get; set; }

    [JsonPropertyName("attendance_noteOut")]
    public string? AttendanceNoteOut { get; set; }

    [JsonPropertyName("attendance_NoteIn")]
    public string? AttendanceNoteIn { get; set; }

    [JsonPropertyName("status_no")]
    public string? StatusNo { get; set; }

    [JsonPropertyName("dayStatus_id")]
    public string? DayStatusId { get; set; }

    [JsonPropertyName("kemaskini_statusId")]
    public string? KemaskiniStatusId { get; set; }

    [JsonPropertyName("alasan_id")]
    public string? AlasanId { get; set; }

    [JsonPropertyName("zonKerjaId")]
    public string? ZonKerjaId { get; set; }

    [JsonPropertyName("isWeekend")]
    public bool IsWeekend { get; set; }

    [JsonPropertyName("dayName")]
    public string? DayName { get; set; }

    [JsonPropertyName("statusKehadiran")]
    public string? StatusKehadiran { get; set; }

    [JsonPropertyName("urusanKeluarPejabat")]
    public UrusanKeluarPejabatInfo? UrusanKeluarPejabat { get; set; }

    [JsonPropertyName("hrmisCuti")]
    public string? HrmisCuti { get; set; }

    [JsonPropertyName("hasPermohonanLulus")]
    public PermohonanLulus? HasPermohonanLulus { get; set; }

    // Helper properties for display
    public DateTime ParsedDate => string.IsNullOrEmpty(MyDate) ? DateTime.MinValue : DateTime.Parse(MyDate);
    public string FormattedDate => ParsedDate != DateTime.MinValue ? ParsedDate.ToString("dd MMM yyyy") : "-";
    public string FormattedTagIn => string.IsNullOrEmpty(AttendanceTagIn) ? "--:--" : AttendanceTagIn;
    public string FormattedTagOut => string.IsNullOrEmpty(AttendanceTagOut) ? "--:--" : AttendanceTagOut;
    public string DisplayStatus => StatusKehadiran ?? (IsWeekend ? "Hujung Minggu" : "-");
    public bool HasApplication => HasPermohonanLulus != null;
}

public class AttendanceSummary
{
    [JsonPropertyName("sumWarnaKad")]
    public string? WarnaKad { get; set; }

    [JsonPropertyName("bulan")]
    public int Bulan { get; set; }

    [JsonPropertyName("tahun")]
    public int Tahun { get; set; }

    [JsonPropertyName("totalTidakLengkap")]
    public int TidakLengkap { get; set; }

    [JsonPropertyName("totalTidakHadir")]
    public int TidakHadir { get; set; }

    [JsonPropertyName("totalLewat")]
    public int Lewat { get; set; }

    [JsonPropertyName("totalBalikAwal")]
    public int BalikAwal { get; set; }

    [JsonPropertyName("totalLewatDanBalikAwal")]
    public int LewatDanBalikAwal { get; set; }
}   

public class UrusanKeluarPejabatInfo
{
    [JsonPropertyName("outstation_id")]
    public int? OutstationId { get; set; }

    [JsonPropertyName("outType_name")]
    public string? OutTypeName { get; set; }

    [JsonPropertyName("outSub_Name")]
    public string? OutSubName { get; set; }

    [JsonPropertyName("outstation_note")]
    public string? OutstationNote { get; set; }

    [JsonPropertyName("outstation_newTagOut")]
    public string? OutstationNewTagOut { get; set; }

    [JsonPropertyName("outstation_newTagIn")]
    public string? OutstationNewTagIn { get; set; }

    [JsonPropertyName("outstation_StartDate")]
    public string? OutstationStartDate { get; set; }

    [JsonPropertyName("outstation_EndDate")]
    public string? OutstationEndDate { get; set; }
}

public class PermohonanLulus
{
    [JsonPropertyName("kemaskini_id")]
    public string? KemaskiniId { get; set; }

    [JsonPropertyName("kemaskini_groupKey")]
    public string? KemaskiniGroupKey { get; set; }

    [JsonPropertyName("kemaskini_date")]
    public string? KemaskiniDate { get; set; }

    [JsonPropertyName("attendance_id")]
    public string? AttendanceId { get; set; }

    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }

    [JsonPropertyName("jenis_id")]
    public string? JenisId { get; set; }

    [JsonPropertyName("alasan_id")]
    public string? AlasanId { get; set; }

    [JsonPropertyName("kemaskini_tagIn")]
    public string? KemaskiniTagIn { get; set; }

    [JsonPropertyName("kemaskini_tagOut")]
    public string? KemaskiniTagOut { get; set; }

    [JsonPropertyName("kemaskini_noteIn")]
    public string? KemaskiniNoteIn { get; set; }

    [JsonPropertyName("kemaskini_noteOut")]
    public string? KemaskiniNoteOut { get; set; }

    [JsonPropertyName("status_id")]
    public string? StatusId { get; set; }

    [JsonPropertyName("email_notification_status")]
    public string? EmailNotificationStatus { get; set; }

    [JsonPropertyName("kemaskini_approvedBy")]
    public string? KemaskiniApprovedBy { get; set; }

    [JsonPropertyName("kemaskini_approvalDate")]
    public string? KemaskiniApprovalDate { get; set; }

    [JsonPropertyName("kemaskini_createDate")]
    public string? KemaskiniCreateDate { get; set; }

    [JsonPropertyName("kemaskini_notePelulus")]
    public string? KemaskiniNotePelulus { get; set; }

    [JsonPropertyName("kemaskini_newAttStatId")]
    public string? KemaskiniNewAttStatId { get; set; }

    [JsonPropertyName("leave_id")]
    public string? LeaveId { get; set; }

    [JsonPropertyName("sijil_cuti")]
    public string? SijilCuti { get; set; }

    [JsonPropertyName("status_name")]
    public string? StatusName { get; set; }
}

public class RekodImbasanModel
{
    [JsonPropertyName("staffId")]
    public int? StaffId { get; set; }

    [JsonPropertyName("staffName")]
    public string? StaffName { get; set; }

    [JsonPropertyName("staffMykad")]
    public string? StaffMykad { get; set; }

    [JsonPropertyName("tarikh")]
    public string? TarikhImbasan { get; set; }

    [JsonPropertyName("masa")]
    public string? MasaImbasan { get; set; }

    [JsonPropertyName("lokasi")]
    public string? LokasiImbasan { get; set; }

    //public DateTime ParsedDate => string.IsNullOrEmpty(TarikhImbasan) ? DateTime.MinValue : DateTime.Parse(TarikhImbasan);
    public DateTime ParsedDate =>
    string.IsNullOrEmpty(TarikhImbasan)
        ? DateTime.MinValue
        : DateTime.ParseExact(TarikhImbasan, "dd/MM/yyyy", CultureInfo.InvariantCulture);

}