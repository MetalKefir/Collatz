namespace Collatz;
class Program
{
    static void Main(string[] args)
    {
        Parallel.For(1, long.MaxValue, new ParallelOptions { MaxDegreeOfParallelism = 10 }, (number) =>
        {
            var (collatzConverg, powerOfTwo, collatzSteps, convergPower) = Collatz(number);
            Console.WriteLine($"Number: {number} : Collatz:{collatzConverg} : Power:{convergPower}");
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