using Task.Homework.h_t_19_09_2024;
using static Task.TasksWorkers;

namespace Task.Homework
{
    public class Homework5 : ITask
    {
        public enum Number
        {
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10
        }
        public void Start()
        {
            int[] array = Enum.GetValues(typeof(Number)).Cast<int>().ToArray();
            int[] sortedArray = BubbleSort.Sort(array, (a, b) => a < b);
            Console.WriteLine(string.Join(',', sortedArray));
            Console.WriteLine(string.Join(',', array));
        }
    }
}

namespace Task.Homework.h_t_19_09_2024
{
    public static class BubbleSort
    {
        private static readonly int start = 0;
        private static readonly int next = 1;
        private static readonly int offsetLength = 1;
        public static T[] Sort<T>(T[] array, Func<T, T, bool> predicate)
        {
            T[] arrayCopy = (T[])array.Clone();
            for (int i = start; i < arrayCopy.Length - offsetLength; i++)
                for (int j = start; j < arrayCopy.Length - offsetLength - i; j++)
                    if (predicate(arrayCopy[j], arrayCopy[j + next]))
                        (arrayCopy[j + next], arrayCopy[j]) = (arrayCopy[j], arrayCopy[j + next]);
            return arrayCopy;
        }
    }
}
