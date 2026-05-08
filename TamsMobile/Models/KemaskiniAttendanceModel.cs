using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TamsMobile.Models
{
    public class KemaskiniAttendanceModel
    {
        public class KemaskiniDetails
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
            public string? UrusanId { get; set; }

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

            [JsonPropertyName("kemaskini_approvedByName")]
            public string? KemaskiniApprovedByName { get; set; }

            [JsonPropertyName("kemaskini_approvedByMykad")]
            public string? KemaskiniApprovedByMykad { get; set; }

            [JsonPropertyName("kemaskini_approvalDate")]
            public string? KemaskiniApprovedDate { get; set; }

            [JsonPropertyName("kemaskini_createDate")]
            public string? KemaskiniCreateDate { get; set; }

            [JsonPropertyName("kemaskini_notePelulus")]
            public string? KemaskiniNotePelulus { get; set; }

            [JsonPropertyName("kemaskini_newAttStatId")]
            public string? KemaskiniNewAttStatId { get; set; }

            [JsonPropertyName("leave_id")]
            public string? LeaveId { get; set; }

            [JsonPropertyName("sijil_cuti")]
            public string? sijil_cuti { get; set; }

            [JsonPropertyName("status_name")]
            public string? status_name { get; set; }
        }

    }


    public class KemaskiniAttendanceRequestModel
    {
        [JsonPropertyName("addKemasKini")]
        public KemaskiniAttendanceAddModel? addKemasKini { get; set; }

        [JsonPropertyName("updateKemasKini")]
        public KemaskiniAttendanceUpdateModel? updateKemasKini { get; set; }

        [JsonPropertyName("pelulusKemaskini")]
        public KemaskiniAttendancePelulusModel? pelulusKemaskini { get; set; }

        [JsonPropertyName("deleteKemasKiniId")]
        public string? deleteKemasKiniId { get; set; }
    }

    public class KemaskiniAttendanceAddModel
    {
        public string? kemaskini_date { get; set; }
        public string? kemaskini_groupKey { get; set; }
        public string? attendance_id { get; set; }
        public string? user_id { get; set; }
        public string? jenis_id { get; set; }
        public string? alasan_id { get; set; }
        public string? kemaskini_tagIn { get; set; }
        public string? kemaskini_tagOut { get; set; }
        public string? kemaskini_noteIn { get; set; }
        public string? kemaskini_noteOut { get; set; }
    }

    public class KemaskiniAttendanceUpdateModel
    {
        public string? attendance_id { get; set; }
        public string? user_id { get; set; }
        public string? jenis_id { get; set; }
        public string? alasan_id { get; set; }
        public string? kemaskini_tagIn { get; set; }
        public string? kemaskini_tagOut { get; set; }
        public string? kemaskini_noteIn { get; set; }
        public string? kemaskini_noteOut { get; set; }

    }

    public class KemaskiniAttendancePelulusModel
    {
        public string? attendance_id { get; set; }
        public string? user_id { get; set; }
        public string? status_id { get; set; }
        public string? kemaskini_notePelulus { get; set; }
    }
}
