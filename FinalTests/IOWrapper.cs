namespace Task.FinalTests;

public interface IIOWrapper
{
    string InValue();
    void OutValue(string message);
}

public class IOWrapper : IIOWrapper
{
    public string InValue() => Console.ReadLine()!;
    public void OutValue(string message) => Console.WriteLine(message);
}
