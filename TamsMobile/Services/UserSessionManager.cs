using System.Threading.Tasks;
using TamsMobile.Models;

namespace TamsMobile.Services;

public class UserSessionManager
{
    private readonly AuthenticationService _authService;
    private readonly ProfileService _profileService;

    public UserData? CurrentUser { get; private set; }
    public Kakitangan? UserProfile { get; private set; }
    public string? AuthToken { get; private set; }
    public bool IsAuthenticated => CurrentUser != null && !string.IsNullOrEmpty(AuthToken);

    public UserSessionManager(
        AuthenticationService authService,
        ProfileService profileService)
    {
        _authService = authService;
        _profileService = profileService;
    }

    public async Task<(bool Success, string Message)> LoginAsync(string mykad, string password)
    {
        var (success, message, userData, token) = await _authService.LoginAsync(mykad, password);

        if (success && userData != null && token != null)
        {
            CurrentUser = userData;
            AuthToken = token;

            // Load user profile after successful login
            UserProfile = await _profileService.GetUserProfileAsync(userData.IdentificationNumber);

            return (true, message);
        }

        return (false, message);
    }

    public async Task Logout()
    {
       await _authService.LogoutAsync();
        CurrentUser = null;
        UserProfile = null;
        AuthToken = null;
    }

    public string GetUserDisplayName()
    {
        return UserProfile?.UserFullname ?? CurrentUser?.Email ?? "User";
    }

    public string GetAccessLevelName()
    {
        return UserProfile?.AccessLevelName ?? GetRoleDisplayName();
    }

    public string GetDepartmentName()
    {
        return UserProfile?.JabatanName ?? "-";
    }

    public string GetMykadNumber()
    {
        return UserProfile?.IdentificationNumber ?? "-";
    }

    public string GetPelulusId()
    {
        return UserProfile?.PelulusId ?? "-";
    }

    public string GetPelulusName()
    {
        return UserProfile?.PelulusName ?? "-";
    }

    public string GetPelulusMykad()
    {
        return UserProfile?.PelulusMykad ?? "-";
    }
    public bool HasTodayWbBdrHarian()
    {
        if (UserProfile?.PeriodName == "WB BDR" || 
            UserProfile?.PeriodName == "WBBDR" || 
            UserProfile?.PeriodName == "WB_BDR" || 
            UserProfile?.PeriodName == "BDR" || 
            UserProfile?.PeriodName == "BEKERJA DARI RUMAH" || 
            UserProfile?.PeriodName == "WAKTU BEKERJA DARI RUMAH")
        {
            return true;
        }
        return false;
    }

    // ===== NEW ROLE MANAGEMENT METHODS =====

    /// <summary>
    /// Gets the display name for the user's role
    /// </summary>
    public string GetRoleDisplayName()
    {
        return UserProfile?.AccessLevel switch
        {
            1 => "SUPERADMIN",
            3 => "ADMIN JABATAN",
            6 => "PENGGUNA BIASA",
            _ => "PENGGUNA"
        };
    }

    /// <summary>
    /// Check if user is SuperAdmin (Access Level 1)
    /// Full system access
    /// </summary>
    public bool IsSuperAdmin()
    {
        return UserProfile?.AccessLevel == 1;
    }

    /// <summary>
    /// Check if user is Department Admin (Access Level 3)
    /// Can view attendance of department workers
    /// </summary>
    public bool IsDepartmentAdmin()
    {
        return UserProfile?.AccessLevel == 3;
    }

    /// <summary>
    /// Check if user is Normal User (Access Level 6)
    /// Can only view own profile, attendance, and submit leave/update requests
    /// </summary>
    public bool IsNormalUser()
    {
        return UserProfile?.AccessLevel == 6;
    }

    /// <summary>
    /// Check if user is any type of admin (SuperAdmin or Department Admin)
    /// </summary>
    public bool IsAdmin()
    {
        return IsSuperAdmin() || IsDepartmentAdmin() || UserProfile?.IsLogPelulus == true;
    }

    /// <summary>
    /// Check if user can view department attendance
    /// Only SuperAdmin and Department Admin can view department attendance
    /// </summary>
    public bool CanViewDepartmentAttendance()
    {
        return IsSuperAdmin() || IsDepartmentAdmin();
    }

    /// <summary>
    /// Check if user can only view their own data
    /// Normal users can only view their own information
    /// </summary>
    public bool CanOnlyViewOwnData()
    {
        return IsNormalUser();
    }

    /// <summary>
    /// Check if user can submit leave or exit office requests
    /// All users can submit requests
    /// </summary>
    public bool CanSubmitRequests()
    {
        return IsAuthenticated;
    }

    /// <summary>
    /// Check if user can approve requests
    /// Only Pelulus, SuperAdmin, and Department Admin can approve
    /// </summary>
    public bool CanApproveRequests()
    {
        return IsSuperAdmin() || IsDepartmentAdmin() || UserProfile?.IsLogPelulus == true;
    }

    /// <summary>
    /// Get the user's department code
    /// </summary>
    public string? GetDepartmentCode()
    {
        return UserProfile?.JabatanCode;
    }

    /// <summary>
    /// Get user permissions summary for debugging/logging
    /// </summary>
    public string GetPermissionsSummary()
    {
        if (!IsAuthenticated) return "Not Authenticated";

        var permissions = new List<string>();

        if (IsSuperAdmin()) permissions.Add("SuperAdmin");
        if (IsDepartmentAdmin()) permissions.Add("Department Admin");
        if (IsNormalUser()) permissions.Add("Normal User");
        if (UserProfile?.IsLogPelulus == true) permissions.Add("Pelulus");
        if (UserProfile?.IsLogPengurusanTertinggi == true) permissions.Add("Pengurusan Tertinggi");

        return string.Join(", ", permissions);
    }
}