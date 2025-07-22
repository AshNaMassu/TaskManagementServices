using System.ComponentModel;

namespace Domain.Enums
{
    /// <summary>
    /// Статусы задачи
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Новая задача
        /// </summary>
        [Description("Новая")]
        New,

        /// <summary>
        /// Задача в работе
        /// </summary>
        [Description("В работе")]
        InProcess,

        /// <summary>
        /// Задача выполнена
        /// </summary>
        [Description("Выполненная")]
        Completed
    }
}
