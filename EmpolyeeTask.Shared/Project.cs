namespace EmployeeTask.Shared
{
    public class Project : IEntity
    {
#nullable disable
        /// <summary>
        /// Уникальный ключ
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Название проекта
        /// </summary>
        [Required]
        public string ProjectName { get; set; }
#nullable enable
        /// <summary>
        /// Список задач
        /// </summary>
        public IEnumerable<Issue>? Issues { get; set; }
    }
}
