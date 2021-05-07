using System.ComponentModel;

namespace Common.Models.Enums
{
    public enum InspectionStatus
    {
        [Description("Новая")]
        New,

        [Description("В процессе")]
        InProgress,

        [Description("На согласовании")]
        InReview,

        [Description("Утверждена")]
        Approved,

        [Description("Завершена")]
        Finished
    }
}