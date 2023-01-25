namespace Collatz;
class Program
{
    static void Main(string[] args)
    {
        for (int i = 5;; i++)
        {
            Console.WriteLine($"Number: {i} : Collatz:{Collatz(i, out var powerOfTwo)} : Power of two:{powerOfTwo}");
        }
    }

    static bool Collatz(int number, out bool powerOfTwo)
    {
        powerOfTwo = false;
        for (int i = number; i != 1;)
        {
            powerOfTwo = PowerOfTwo(i);
            if (i % 2 == 0)
            {
                i = EvenNumber(i);
            }
            else
            {
                i = OddNumber(i);
            }
        }

        return true;
    }

    static int EvenNumber(int number)
    {
        return number / 2;
    }

    static int OddNumber(int number)
    {
        return (3 * number) + 1;
    }

    static bool PowerOfTwo(int number)
    {
        return ((number) & (number - 1)) == 0;
    }
}