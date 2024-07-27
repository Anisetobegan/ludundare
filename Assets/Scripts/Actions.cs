using System;

public static class Actions
{
    public static Action<Enemies> OnEnemyKilled;

    public static Action<Summon> OnSummonKilled;
}
