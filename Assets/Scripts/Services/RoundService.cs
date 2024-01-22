#define FIBONACCI_RECURSIVE
using UnityEngine;

public class RoundService
{
    public RoundService(EntityService entityService)
    {
        EntityService = entityService;
    }

    public EntityService EntityService { get; }

    public int Round { get; private set; } = 1;

    public void Initialization(float[] spawnHeights)
    {
        EntityService?.Initialization(spawnHeights);
    }

    public void Update()
    {
        EntityService?.Update();
    }

    public void StartRound()
    {
        EntityService?.Spawn(Fibonacci(Round));
    }

    private void EndRound()
    {
        Debug.Log("Round finished.");
    }

#if FIBONACCI_RECURSIVE
    private int Fibonacci(int n)
    {
        if (n <= 1)
            return n;

        return Fibonacci(n - 1) + Fibonacci(n - 2);
    }
#else
    private int Fibonacci(int n)
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
