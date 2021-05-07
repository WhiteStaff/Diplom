using System.ComponentModel;

namespace Common.Models.Enums
{
    public enum CompanyRole
    {
        [Description("Заказчик")]
        Customer,

        [Description("Исполнитель")]
        Contractor
    }
}