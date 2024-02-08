using System.ComponentModel;

namespace Domain.Main.Enums
{
    public enum ProductStatus: byte
    {
        [Description("Inactivo")]
        Inactive = 0,

        [Description("Activo")]
        Active = 1
    }
}
