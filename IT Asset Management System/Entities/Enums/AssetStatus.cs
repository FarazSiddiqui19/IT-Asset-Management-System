using Microsoft.AspNetCore.Http.Connections;

namespace IT_Asset_Management_System.Entities.Enums
{
    public enum AssetStatus
    {
        Available = 0,
        Assigned = 1,
        UnderMaintenance = 2,
        Retired = 3
    }
}
