namespace KnapTask.Models.ViewModel
{
    
    public class TaskItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = "Другое";
        public int Weight { get; set; }
        public int Value { get; set; }
        public bool IsCompleted { get; set; }

        public double Efficiency => Weight > 0 ? (double)Value / Weight : 0;

        public string BadgeClass => Category switch
        {
            "Учеба" => "bg-info text-dark",
            "Работа" => "bg-warning text-dark",
            "Спорт" => "bg-danger text-white",
            _ => "bg-secondary text-white"
        };
    }
}
