using KnapTask.Models;

namespace KnapTask.Services
{
    public class OptimizationService
    {
        public List<TaskItem> GetOptimizedPlan(List<TaskItem> allTasks , int maxHours) 
        { 
            
            //Выбираем только те задачи, которые не выполнены, так как выполненные задачи уже не требуют времени и не приносят ценности
            var tasks = allTasks.Where(t => !t.IsCompleted).ToList();
            int n = tasks.Count;

            // Создаем таблицу для динамического программирования, где dp[i, j] будет хранить максимальную ценность, которую можно достичь, используя первые i задач и имея j часов
            int[,] dp = new int[n + 1, maxHours + 1];

            
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= maxHours; j++)
                {
                    // Если текущая задача может быть выполнена в оставшееся время, выбираем максимум между включением этой задачи и исключением ее
                    if (tasks[i - 1].Weight <= j)
                    {
                        dp[i, j] = Math.Max(tasks[i - 1].Value + dp[i - 1, j - tasks[i - 1].Weight], dp[i - 1, j]);// Вкл задачу и добавляем ее ценность к оптимальному решению для оставшегося времени, или исключаем задачу и сохраняем оптимальное решение для текущего времени
                    }
                    else
                    {
                        dp[i, j] = dp[i - 1, j];
                    }
                }
            }

            var result = new List<TaskItem>();
            int resWeight = maxHours;

            // Восстанавливаем оптимальный план задач, начиная с последней задачи и двигаясь назад
            for (int i = n; i > 0 && resWeight > 0; i--)
            {
                if (dp[i, resWeight] != dp[i - 1, resWeight])
                {
                    result.Add(tasks[i - 1]);
                    resWeight -= tasks[i - 1].Weight;
                }
            }

            return result;

        }
    }
}
