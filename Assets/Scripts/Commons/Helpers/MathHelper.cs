#define FIBONACCI_RECURSIVE

public static class MathHelper
{
#if FIBONACCI_RECURSIVE
    public static int Fibonacci(int n)
    {
        if (n <= 1)
            return n;

        return Fibonacci(n - 1) + Fibonacci(n - 2);
    }
#else
    public static int Fibonacci(int n)
    {
        var a = 0;
        var b = 1;
        var c = 0;

        if (n == 0)
            return a;

        for (var i = 2; i <= n; i++)
        {
            c = a + b;
            a = b;
            b = c;
        }

        return b;
    }
#endif
}
