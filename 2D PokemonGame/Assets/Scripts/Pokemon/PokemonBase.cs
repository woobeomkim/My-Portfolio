using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pokemon", menuName = "Pokemon/Create new Pokemon")]
public class PokemonBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;

    [SerializeField] int expYield;
    [SerializeField] GrowthRate growthRate;

    [SerializeField] int catchRate = 255; // 0~255사이의값

    [SerializeField] List<LearnableMove> learnableMoves;
    [SerializeField] List<MoveBase> learnableByItems;

    [SerializeField] List<Evolution> evolutions;
    public static int MaxNumOfMoves { get; set; } = 4;

    public int GetExpForLevel(int level)
    {
        if(growthRate == GrowthRate.Fast)
        {
            return 4 * (level * level * level) / 5;
        }
        else if(growthRate == GrowthRate.MediumFast)
        {
            return level * level * level;
        }

        // Error
        return -1;
    }
    public string Name => name;
    public string Description => description;
    public Sprite FrontSprite => frontSprite;
    public Sprite BackSprite => backSprite;
    public PokemonType Type1 => type1;
    public PokemonType Type2 => type2;
    public int MaxHp => maxHp;
    public int Attack => attack;
    public int Defense => defense;
    public int SpAttack => spAttack;
    public int SpDefense => spDefense;
    public int Speed => speed;
    public List<LearnableMove> LearnableMoves => learnableMoves;
    public List<MoveBase> LearnableByItems => learnableByItems;

    public List<Evolution> Evolutions => evolutions;

    public int CatchRate => catchRate;
    public int ExpYield => expYield;
    public GrowthRate GrowthRate => growthRate;
}

[System.Serializable]
public class LearnableMove
{
    public MoveBase moveBase;
    public int level;

}

[System.Serializable]
public class Evolution
{
    [SerializeField] PokemonBase evolvesInto;
    [SerializeField] int requiredLevel;
    [SerializeField] EvolutionItem requiredItem;
    public PokemonBase EvolvesInto => evolvesInto;
    public int RequiredLevel => requiredLevel;

    public EvolutionItem RequiredItem => requiredItem;
}

public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon,
    Dark,
    Steel,
    Fairy
}

public enum GrowthRate
{
    Fast,MediumFast,
}

public enum Stat
{
    Attack,
    Defense,
    SpAttack,
    SpDefense,
    Speed,

    // 실제 스탯은아니지만 부스팅할수잇는 스탯
    Accuracy,
    Evasion
}

public class TypeChart
{
    static float[][] chart =
    {
         //Has to be same order as PokemonType class
        //                       Nor   Fir   Wat   Ele   Gra   Ice   Fig   Poi   Gro   Fly   Psy   Bug   Roc   Gho   Dra   Dar  Ste    Fai
        /*Normal*/  new float[] {1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 0,    1f,   1f,   0.5f, 1f},
        /*Fire*/    new float[] {1f,   0.5f, 0.5f, 1f,   2f,   2f,   1f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   0.5f, 1f,   2f,   1f},
        /*Water*/   new float[] {1f,   2f,   0.5f, 1f,   0.5f, 1f,   1f,   1f,   2f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f,   1f,   1f},
        /*Electric*/new float[] {1f,   1f,   2f,   0.5f, 0.5f, 1f,   1f,   1f,   0f,   2f,   1f,   1f,   1f,   1f,   0.5f, 1f,   1f,   1f},
        /*Grass*/   new float[] {1f,   0.5f, 2f,   1f,   0.5f, 1f,   1f,   0.5f, 2f,   0.5f, 1f,   0.5f, 2f,   1f,   0.5f, 1f,   0.5f, 1f},
        /*Ice*/     new float[] {1f,   0.5f, 0.5f, 1f,   2f,   0.5f, 1f,   1f,   2f,   2f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f},
        /*Fighting*/new float[] {2f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f,   0.5f, 0.5f, 0.5f, 2f,   0f,   1f,   2f,   2f,   0.5f},
        /*Poison*/  new float[] {1f,   1f,   1f,   1f,   2f,   1f,   1f,   0.5f, 0.5f, 1f,   1f,   1f,   0.5f, 0.5f, 1f,   1f,   0f,   2f},
        /*Ground*/  new float[] {1f,   2f,   1f,   2f,   0.5f, 1f,   1f,   2f,   1f,   0f,   1f,   0.5f, 2f,   1f,   1f,   1f,   2f,   1f},
        /*Flying*/  new float[] {1f,   1f,   1f,   0.5f, 2f,   1f,   2f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   1f,   1f,   0.5f, 1f},
        /*Psychic*/ new float[] {1f,   1f,   1f,   1f,   1f,   1f,   2f,   2f,   1f,   1f,   0.5f, 1f,   1f,   1f,   1f,   0f,   0.5f, 1f},
        /*Bug*/     new float[] {1f,   0.5f, 1f,   1f,   2f,   1f,   0.5f, 0.5f, 1f,   0.5f, 2f,   1f,   1f,   0.5f, 1f,   2f,   0.5f, 0.5f},
        /*Rock*/    new float[] {1f,   2f,   1f,   1f,   1f,   2f,   0.5f, 1f,   0.5f, 2f,   1f,   2f,   1f,   1f,   1f,   1f,   0.5f, 1f},
        /*Ghost*/   new float[] {0f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 1f,   1f,   2f,   1f,   0.5f, 1f,   1f},
        /*Dragon*/  new float[] {1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 0f},
        /*Dark*/    new float[] {1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 1f,   1f,   1f,   2f,   1f,   1f,   2f,   1f,   0.5f, 1f,   0.5f},
        /*Steel*/   new float[] {1f,   0.5f, 0.5f, 0.5f, 1f,   2f,   1f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   1f,   1f,   0.5f, 2f},
        /*Fairy*/   new float[] {1f,   0.5f, 1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   1f,   1f,   1f,   1f,   1f,   2f,   2f,   0.5f, 1f},
    };

    public static float GetEffectiveness(PokemonType attackerType, PokemonType defenseType)
    {
        if (attackerType == PokemonType.None || defenseType == PokemonType.None)
            return 1.0f;

        int row = (int)attackerType - 1;
        int col = (int)defenseType - 1;

        return chart[row][col];
    }
}