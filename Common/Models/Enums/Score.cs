using System.ComponentModel;

namespace Common.Models.Enums
{
    public enum Score
    {
        [Description("Хорошо")]
        Good,

        [Description("Удовлетворительно")]
        Passable,

        [Description("Сомнительно")]
        Doubtful,

        [Description("Неудовлетворительно")]
        Inefficient
    }
}