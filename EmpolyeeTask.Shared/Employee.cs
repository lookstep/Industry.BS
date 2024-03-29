﻿using Microsoft.EntityFrameworkCore;

namespace EmployeeTask.Shared
{
    [Index("Email", IsUnique = true)]
    public class Employee : IEntity
    {
#nullable disable
        /// <summary>
        /// Уникальный ключ
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        [Required]
        [MinLength(2)]
        public string SecondName { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Роль
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// Почтовый адресс
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Нерпавельный email адресс")]
        
        public string Email { get; set; }
        /// <summary>
        /// Вторичный ключ подразделения
        /// </summary>
        public int DivisionId { get; set; }
#nullable enable
        /// <summary>
        /// Отчество
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Код подтверждения
        /// </summary>
        public string? ConfirmationCode { get; set; }
        /// <summary>
        /// Картинка профиля
        /// </summary>
        public string? IconPath { get; set; }
        /// <summary>
        /// Табельный номер
        /// </summary>
        [Range(1, 99999)]
        public int ServiceNumber { get; set; }
        /// <summary>
        /// Код 1C
        /// </summary>
        [Range(1, 99999)]
        public int OneCPass { get; set; }
        /// <summary>
        /// Занимаемая должность
        /// </summary>
        public string? Post { get; set; }

        /// <summary>
        /// Подразделение
        /// </summary>
        [ForeignKey(nameof(DivisionId))]
        public virtual Division? Division { get; set; }
        /// <summary>
        /// Трудозатраты
        /// </summary>
        public virtual IEnumerable<LaborCost>? LaborCosts { get; set; }
    }

}
