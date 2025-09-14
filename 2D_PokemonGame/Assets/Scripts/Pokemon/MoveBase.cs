using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move", menuName = "Pokemon/Create new Move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] PokemonType type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int pp;
    [SerializeField] int priority;
    [SerializeField] bool alwaysHits;
    [SerializeField] MoveCategory category;
    [SerializeField] MoveEffects effects;
    [SerializeField] List<SecondaryEffects> secondaries;
    [SerializeField] MoveTarget target;

    [SerializeField] bool isMultiHitMove = false;
    [SerializeField] Vector2Int hitRange = new Vector2Int(2, 0);

    [SerializeField] AudioClip sound;

    public int GetHitTimes()
    {
        if (isMultiHitMove)
        {
            if (hitRange.y == 0)
                return hitRange.x;
            else
                return Random.Range(hitRange.x, hitRange.y + 1);
        }
        else
            return 1;
    }

    public string Name => name;
    public string Description => description;
    public PokemonType Type => type;
    public int Power => power;
    public int PP => pp;
    public int Priority => priority;
    public int Accuracy => accuracy;

    public bool AlwaysHits
    {
        get { return alwaysHits; }
    }

    public MoveCategory Category
    {
        get { return category; }
    }

   public MoveEffects Effects
    {
        get
        {
            return effects;
        }
    }

    public List<SecondaryEffects> Secondaries
    {
        get { return secondaries; }
    }

    public MoveTarget Target
    {
        get { return target; }
    }

    public bool IsMultiHitMove => isMultiHitMove;

    public AudioClip Sound => sound;
}

[System.Serializable]
public class MoveEffects
{
    [SerializeField] List<StatBoost> boosts;
    [SerializeField] StatusConditionID status;
    [SerializeField] StatusConditionID volatileStatus;
    [SerializeField] WeatherConditionID weather;
    public List<StatBoost> Boosts
    {
        get { return boosts; }
    }

    public StatusConditionID Status
    {
        get { return status; }
    }

    public StatusConditionID VolatileStatus
    {
        get { return volatileStatus; }
    }

    public WeatherConditionID Weather => weather;
}

[System.Serializable]
public class SecondaryEffects : MoveEffects
{
    [SerializeField] int chance;
    [SerializeField] MoveTarget target;

    public int Chance
    {
        get { return chance; }
    }

    public MoveTarget Target
    {
        get { return target; }
    }
}


[System.Serializable]
public class StatBoost
{
    public Stat stat;
    public int boost;
}

public enum MoveCategory
{
    Physical,Special,Status
}

public enum MoveTarget
{
    Foe,Self
}