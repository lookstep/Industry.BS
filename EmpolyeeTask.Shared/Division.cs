namespace EmployeeTask.Shared
{
    public class Division : IEntity
    {
#nullable disable
        /// <summary>
        /// Уникальный ключ
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Название подразделения
        /// </summary>
        [Required]
        [MinLength(2)]
        public string DivisionName { get; set; }
#nullable enable
        /// <summary>
        /// Сотруддник, принадлежащий этому подразделению
        /// </summary>
        public virtual IEnumerable<Employee>? Employees { get; set; }
    }
}
