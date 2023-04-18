namespace EmployeeTask.Shared
{
    public class Issue : IEntity
    {
#nullable disable
        /// <summary>
        /// Уникальный ключ
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование поставленной задачи
        /// </summary>
        [Required]
        [MinLength(2)]
        public string TaskName { get; set; }
        /// <summary>
        /// Вторичный ключ для проекта
        /// </summary>
        public int ProjectId { get; set; }
#nullable enable
        /// <summary>
        /// Описания того, что надо сделать
        /// </summary>
        public string? TaskDiscribe { get; set; }
        /// <summary>
        /// Навигациооное свойство проекта
        /// </summary>
        [ForeignKey(nameof(ProjectId))]
        public virtual Project? Project { get; set; }
        /// <summary>
        /// Список трудозатрат
        /// </summary>
        public virtual IEnumerable<LaborCost>? LaborCosts { get; set; }
        
    }
}
