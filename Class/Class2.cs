using static Task.TasksWorkers;

namespace Task.Class
{
    public class TaskOnClass2 : ITask
    {
        public void Start()
        {
            Action task1 = () =>
            {
                l_t_05_09_2024.Task1.Coord xyCoord1 = new(12.5, 9);
                l_t_05_09_2024.Task1.Coord xyCoord2 = new(-1, 2);

                Console.WriteLine($"Distance between points: {xyCoord1 ^ xyCoord2}");
            };

            task1.Invoke();
        }
    }
}

namespace Task.Class.l_t_05_09_2024
{
    namespace Task1
    {
        public class XCoord(double coord)
        {
            public double Coord { get; private set; } = coord;
        }

        public class YCoord(double coord)
        {
            public double Coord { get; private set; } = coord;
        }

        public class Coord(double x, double y)
        {
            public Tuple<XCoord, YCoord> Point { get; set; } = new(new XCoord(x), new YCoord(y));

            public static double operator ^(Coord c1, Coord c2)
            {
                return Math.Sqrt(Math.Pow(c2.Point.Item1.Coord - c1.Point.Item1.Coord, 2) + Math.Pow(c2.Point.Item2.Coord - c1.Point.Item2.Coord, 2));
            }
        }
    }
}
