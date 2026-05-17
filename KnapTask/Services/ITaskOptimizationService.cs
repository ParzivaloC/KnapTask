using KnapTask.Models;
using KnapTask.Models.ViewModel;
//Сервис для оптимизации задач, который включает в себя алгоритм решения задачи о рюкзаке и метод для преобразования данных в формат, удобный для отображения в пользовательском интерфейсе.
//Этот сервис будет использоваться в контроллере для получения оптимального плана задач и передачи его в представление.
namespace KnapTask.Services
{
    public interface ITaskOptimizationService
    {
        List<TaskItem> GetOptimizedPlan(List<TaskItem> allTasks, int maxHours);

        IEnumerable<TaskItemViewModel> MapToViewModel(IEnumerable<TaskItem> tasks); 
    }
}
