using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Pokemon
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;

    public Pokemon (PokemonBase pBase, int pLevel)
    {
        _base = pBase;
        level = pLevel;

        Init();
    }

    public PokemonBase Base { get { return _base; } private set { _base = value; } }
    int hp;

    List<Move> moves;

    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
        }
    }

    public int Exp { get; set; }
    public int HP { get { return hp; } set { hp = value; } }

    public List<Move> Moves
    {
        get { return moves; }
        private set { moves = value; }
    }
    public Move CurrentMove { get; set; }
    public Dictionary<Stat, int> Stats { get; private set; }
    public Dictionary<Stat, int> StatBoosts { get; private set; }

    public int StatusTime { get; set; }
    public int VolatileStatusTime { get; set; }

    public StatusCondition Status { get; private set; }
    public StatusCondition VolatileStatus { get; private set; }
    public Queue<StatusEvent> StatusChanges { get; private set; }

    public event Action OnStatusChanged;

    public event Action OnHPChanged;
    public void Init()
    {
        moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if(level >= move.level)
                moves.Add(new Move(move.moveBase));
        
            if(moves.Count >= PokemonBase.MaxNumOfMoves)
                break;
        }

        Exp = Base.GetExpForLevel(Level);

        CalculateStats();
        HP = MaxHp;

        StatusChanges = new Queue<StatusEvent>();
        ResetStatBoost();
        Status = null;
        VolatileStatus = null;
    }

    public Pokemon(PokemonSaveData saveData)
    {
        Base = PokemonDB.GetObjectByName(saveData.name);
        HP = saveData.hp;
        Level = saveData.level;
        Exp = saveData.exp;

        if (saveData.statusId != null)
            Status = StatusConditionsDB.Conditions[saveData.statusId.Value];
        else
            Status = null;

        Moves = saveData.moves.Select(m => new Move(m)).ToList();

        CalculateStats();
        StatusChanges = new Queue<StatusEvent>();
        ResetStatBoost();
        VolatileStatus = null;
    }

    public PokemonSaveData GetSaveData()
    {
        var saveData = new PokemonSaveData()
        {
            name = Base.name,
            hp = HP,
            level = Level,
            exp = Exp,
            statusId = Status?.Id,
            moves = Moves.Select(m => m.GetSaveData()).ToList()
        };

        return saveData;
    }

    void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack, Mathf.FloorToInt((Base.Attack * level) / 100f) + 5);
        Stats.Add(Stat.Defense, Mathf.FloorToInt((Base.Defense * level) / 100f) + 5);
        Stats.Add(Stat.SpAttack, Mathf.FloorToInt((Base.SpAttack * level) / 100f) + 5);
        Stats.Add(Stat.SpDefense, Mathf.FloorToInt((Base.SpDefense * level) / 100f) + 5);
        Stats.Add(Stat.Speed, Mathf.FloorToInt((Base.Speed * level) / 100f) + 5);

        int oldMaxHp = MaxHp;
        MaxHp = Mathf.FloorToInt((Base.MaxHp * level) / 100f) + 10 + Level;
      
        if(oldMaxHp != 0)
            HP += MaxHp - oldMaxHp;
    }

    void ResetStatBoost()
    {
        StatBoosts = new Dictionary<Stat, int>()
        {
            {Stat.Attack,0 },
            {Stat.Defense,0 },
            {Stat.SpAttack,0 },
            {Stat.SpDefense,0 },
            {Stat.Speed,0 },

            {Stat.Accuracy , 0 },
            {Stat.Evasion , 0 }
        };
    }

    int GetStat(Stat stat)
    {
        int statVal = Stats[stat];

        int boost = StatBoosts[stat];
        var boostValues = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

        if (boost >= 0)
            statVal = Mathf.FloorToInt(statVal * boostValues[boost]);
        else
            statVal = Mathf.FloorToInt(statVal / boostValues[-boost]);

        return statVal;
    }

    public void ApplyBoosts(List<StatBoost> statBoosts)
    {
        foreach(var statBoost in statBoosts)
        {
            var stat = statBoost.stat;
            var boost = statBoost.boost;

            StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boost, -6, 6);

            if (boost > 0)
                AddStatusEvent(StatusEventType.StatBoost, $"{Base.Name}의 {stat}이 증가하였습니다!");
            else
                AddStatusEvent(StatusEventType.StatBoost, $"{Base.Name}의 {stat}이 감소하였습니다!");

            Debug.Log($"{stat} 부스트 {StatBoosts[stat]}");
        }
    }

    public bool CheckForLevelUp()
    {
        if (Exp > Base.GetExpForLevel(Level + 1)) 
        {
            ++level;
            CalculateStats();
            return true;
        }
        return false;
    }

    public LearnableMove GetLearnableMoveAtCurrLevel()
    {
       return Base.LearnableMoves.Where(x => x.level == level).FirstOrDefault();
    }

    public void LearnMove(MoveBase moveToLearn)
    {
        if (Moves.Count > PokemonBase.MaxNumOfMoves)
            return;

        Moves.Add(new Move(moveToLearn));
    }

    public bool HasMove(MoveBase moveToCheck)
    {
        return Moves.Count(m => m.Base == moveToCheck) > 0;
    }

    public Evolution CheckForEvolution()
    {
        return Base.Evolutions.FirstOrDefault(E => E.RequiredLevel <= Level);
    }

    public Evolution CheckForEvolution(ItemBase item)
    {
        return Base.Evolutions.FirstOrDefault(E => E.RequiredItem == item);
    }

    public void Evolve(Evolution evolution)
    {
        _base = evolution.EvolvesInto;
        CalculateStats();
    }

    public void Heal()
    {
        HP = MaxHp;
        OnHPChanged?.Invoke();

        CureStatus();
    }

    public float GetNormalizedExp()
    {
        int currLevelExp = Base.GetExpForLevel(Level);
        int nextLevelExp = Base.GetExpForLevel(Level + 1);

        float normalizedExp = (float)(Exp - currLevelExp) / (nextLevelExp - currLevelExp);

        return Mathf.Clamp01(normalizedExp);

    }

    public DamageDetails TakeDamage(Move move, Pokemon attacker, float weatherModifier = 1.0f)
    {
        float critical = 1.0f;
        if (UnityEngine.Random.value * 100f <= 6.25f)
            critical = 2.0f;

        float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) * TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2);

        var damageDetails = new DamageDetails()
        {
            Fainted = false,
            Critical = critical,
            TypeEffectiveness = type,
        };

        float attack = (move.Base.Category == MoveCategory.Special) ? attacker.SpAttack : attacker.Attack;
        float defense = (move.Base.Category == MoveCategory.Special) ? SpDefense : Defense;

        float modifiers = UnityEngine.Random.Range(0.85f, 1.0f) * type * critical * weatherModifier;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        DecreaseHP(damage);

        return damageDetails;
    }

    public void DecreaseHP(int damage, bool callUpdateEvent = false)
    {
        HP = Mathf.Clamp(HP - damage, 0, MaxHp);
        
        if(callUpdateEvent)
            OnHPChanged?.Invoke();
    }

    public void IncreaseHP(int amount)
    {
        HP = Mathf.Clamp(HP + amount, 0, MaxHp);
        OnHPChanged?.Invoke();
    }

    public void SetStatus(StatusConditionID condtionId)
    {
        if (Status != null) return;

        Status = StatusConditionsDB.Conditions[condtionId];
        Status?.OnStart?.Invoke(this);
        AddStatusEvent($"{Base.name}(이)가 {Status.StartMessage}");
        OnStatusChanged?.Invoke();
    }
    public void SetVolatileStatus(StatusConditionID condtionId)
    {
        if (VolatileStatus != null) return;

        VolatileStatus = StatusConditionsDB.Conditions[condtionId];
        VolatileStatus?.OnStart?.Invoke(this);
        AddStatusEvent($"{Base.name}(이)가 {VolatileStatus.StartMessage}");
    }
    public void CureStatus()
    {
        Status = null;
        OnStatusChanged?.Invoke();
    }

    public void CureVolatileStatus()
    {
        VolatileStatus = null;
    }

    public Move GetRandomMove()
    {
        var movesWithPP = Moves.Where(x => x.PP > 0).ToList();

        int r = UnityEngine.Random.Range(0, movesWithPP.Count);
        return movesWithPP[r];
    }

    public bool OnBeforeMove()
    {
        bool canPerformMove = true;
        if(Status?.OnBeforeMove != null)
        {
            if (Status.OnBeforeMove.Invoke(this))
                canPerformMove = false;
        }

        if (VolatileStatus?.OnBeforeMove != null)
        {
            if (VolatileStatus.OnBeforeMove.Invoke(this))
                canPerformMove = false;
        }

        return canPerformMove;
    }

    public void OnAfterTurn()
    {
        Status?.OnAfterTurn?.Invoke(this);
        VolatileStatus?.OnAfterTurn?.Invoke(this);
    }

    public void OnBattleOver()
    {
        VolatileStatus = null;
        ResetStatBoost();
    }

    public void AddStatusEvent(StatusEventType type, string message)
    {
        StatusChanges.Enqueue(new StatusEvent(type, message));
    }
    public void AddStatusEvent(string message)
    {
        StatusChanges.Enqueue(new StatusEvent(StatusEventType.Text, message));
    }

    public bool IsOfType(PokemonType type)
    {
       return type == Base.Type1 || type == Base.Type2;
    }

    public int Attack
    {
        get
        {
            return GetStat(Stat.Attack);
        }
    }

    public int Defense
    {
        get
        {
            return GetStat(Stat.Defense);
        }
    }

    public int SpAttack
    {
        get
        {
            return GetStat(Stat.Attack);
        }
    }

    public int SpDefense
    {
        get
        {
            return GetStat(Stat.SpDefense);
        }
    }

    public int Speed
    {
        get
        {
            return GetStat(Stat.Speed);
        }
    }

    public int MaxHp
    {
        get; set;
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}

[System.Serializable]
public class PokemonSaveData
{
    public string name;
    public int hp;
    public int level;
    public int exp;
    public StatusConditionID? statusId;
    public List<MoveSaveData> moves;
}

public enum StatusEventType { Text,Damage,StatBoost}
public class StatusEvent
{
    public StatusEventType Type { get; private set; }
    public string Message { get; private set; }

    public StatusEvent(StatusEventType type, string message)
    {
        Type = type;
        Message = message;
    }
}