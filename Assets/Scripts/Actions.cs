using System;

public static class Actions
{
    public static Action<Enemies> OnEnemyKilled;

    public static Action<Summon> OnSummonKilled;

    public static Action OnLevelUp;

    public static Action OnWaveWon;

    public static Action<Enemies, float> OnEnemyDamaged;
}
