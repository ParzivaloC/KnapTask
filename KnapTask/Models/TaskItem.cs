using System.ComponentModel.DataAnnotations;

namespace KnapTask.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Напишите, что нужно сделать")]
        [Display(Name = "Задача")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Range(1, 24, ErrorMessage = "Время: от 1 до 24 ч.")]
        [Display(Name = "Трудозатраты (часы)")]
        public int Weight { get; set; } // "Вес" для алгоритма

        [Range(1, 10, ErrorMessage = "Приоритет: от 1 до 10")]
        [Display(Name = "Важность (1-10)")]
        public int Value { get; set; }  // "Ценность" для алгоритма

        [Display(Name = "Выполнено")]
        public bool IsCompleted { get; set; } = false;
    }
}
