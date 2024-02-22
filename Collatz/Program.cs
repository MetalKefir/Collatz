using System.Collections.Concurrent;

namespace Collatz;

public class MultiThreadFileWriter
{
    private static ConcurrentQueue<string> _textToWrite = new();
    private CancellationTokenSource _source = new();
    private CancellationToken _token;

    public MultiThreadFileWriter()
    {
        _token = _source.Token;
        Task.Run(WriteToFile, _token);
    }

    public void WriteLine(string line)
    {
        _textToWrite.Enqueue(line);
    }

    public void Stop() => _source.Cancel();

    private async void WriteToFile()
    {
        while (true)
        {
            if (_token.IsCancellationRequested)
            {
                return;
            }
            using StreamWriter writer = File.AppendText("D:\\Collatz.csv");
            while (_textToWrite.TryDequeue(out string? textLine))
            {
                await writer.WriteLineAsync(textLine);
            }
            writer.Flush();

            if(_textToWrite.IsEmpty)
            {
                Console.WriteLine("DONE");
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var writer = new MultiThreadFileWriter();

        writer.WriteLine("Number&Collatz&Power&Steps&ConvergPower");

        Parallel.For(1, 2_000_000, new ParallelOptions { MaxDegreeOfParallelism = 10 }, (number) =>
        {
            var (collatzConverg, powerOfTwo, collatzSteps, convergPower) = Collatz(number);
            writer.WriteLine($"{number}&{collatzConverg}&{powerOfTwo}&{collatzSteps}&{convergPower}");
        });
    }

    static (bool collatzConverg, bool powerOfTwo, ulong collatzSteps, ulong convergPower) Collatz(long number)
    {
        bool powerOfTwo = false;
        ulong collatzSteps = 0;
        ulong convergPower = 0;
        for (long i = number; i != 1; collatzSteps++)
        {
            if (!powerOfTwo)
            {
                powerOfTwo = PowerOfTwo(i);
            }

            if (powerOfTwo)
            {
                convergPower++;
            }

            if (i % 2 == 0)
            {
                i = EvenNumber(i);
            }
            else
            {
                i = OddNumber(i);
            }
        }

        return (true, powerOfTwo, collatzSteps, convergPower);
    }

    static long EvenNumber(long number)
    {
        return number / 2;
    }

    static long OddNumber(long number)
    {
        return (3 * number) + 1;
    }

    static bool PowerOfTwo(long number)
    {
        return ((number) & (number - 1)) == 0;
    }
}