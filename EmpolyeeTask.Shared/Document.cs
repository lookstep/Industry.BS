using System.Text.Json.Serialization;

namespace EmployeeTask.Shared
{
    public class Document : IEntity
    {
#nullable disable
        public int Id { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        /// <summary>
        /// Путь
        /// </summary>
        [Required]
        public string Path { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        [Required]
        public DateTime CreationDate { get; set; }
#nullable enable
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Текст
        /// </summary>
        public string Title { get; set; }

        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee? Employee { get; set; }

    }
}
