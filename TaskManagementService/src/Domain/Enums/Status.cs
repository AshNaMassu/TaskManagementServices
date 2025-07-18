using System.ComponentModel;

namespace Domain.Enums
{
    public enum Status
    {
        [Description("Новая")]
        New,

        [Description("В работе")]
        InProcess,

        [Description("Выполненная")]
        Completed
    }
}
