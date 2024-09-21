using Task.Homework.h_t_19_09_2024;
using static Task.TasksWorkers;

namespace Task.Homework
{
    public class Homework5 : ITask
    {
        public void Start()
        {
            int[] sortedArray = BubbleSort.Sort([1, 2, 3, 4, 5, 7, 6, 8, 9, 10], (a, b) => a < b);
            Console.WriteLine(string.Join(',', sortedArray));
        }
    }
}

namespace Task.Homework.h_t_19_09_2024
{
    public static class BubbleSort
    {
        public static T[] Sort<T>(T[] array, Func<T, T, bool> predicate)
        {
            T[] arrayCopy = (T[])array.Clone();
            for (int i = 0; i < arrayCopy.Length - 1; i++)
                for (int j = 0; j < arrayCopy.Length - 1 - i; j++)
                    if (predicate(arrayCopy[j], arrayCopy[j + 1]))
                        (arrayCopy[j + 1], arrayCopy[j]) = (arrayCopy[j], arrayCopy[j + 1]);
            return arrayCopy;
        }
    }
}
