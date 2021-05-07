using System.ComponentModel;

namespace DataAccess.Enums
{
    public enum CompanyRole
    {
        [Description("Заказчик")]
        Customer,

        [Description("Исполнитель")]
        Contractor
    }
}