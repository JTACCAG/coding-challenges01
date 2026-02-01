using System.Runtime.Serialization;

namespace Api.Application.Enums
{
    public enum RoleEnum
    {
        [EnumMember(Value = "regular")]
        Regular,

        [EnumMember(Value = "admin")]
        Admin,

        [EnumMember(Value = "superadmin")]
        Superadmin
    }
}
