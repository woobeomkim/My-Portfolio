using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusConditionsDB
{
    public static void Init()
    {
        foreach(var kvp in Conditions)
        {
            var conditionId = kvp.Key;
            var condition = kvp.Value;

            condition.Id = conditionId;
        }
    }

    public static Dictionary<StatusConditionID, StatusCondition> Conditions { get; set; } = new Dictionary<StatusConditionID, StatusCondition>()
    {
        {
            StatusConditionID.psn,
            new StatusCondition()
            {
                Name = "Poison",
                StartMessage = "독에 감염되었습니다",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.DecreaseHP(pokemon.MaxHp / 8);
                    pokemon.AddStatusEvent(StatusEventType.Damage , $"{pokemon.Base.Name}(이)가 독에 감염되어 스스로에게 데미지를 입혔다!");
                },
            }
        },
        {
            StatusConditionID.brn,
            new StatusCondition()
            {
                Name = "Burn",
                StartMessage = "화상을 입었습니다.",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.DecreaseHP(pokemon.MaxHp / 16);
                    pokemon.AddStatusEvent(StatusEventType.Damage, $"{pokemon.Base.Name}(이)가 화상을입어 스스로에게 데미지를 입혔다!");
                },
            }
        },
        {
            StatusConditionID.par,
            new StatusCondition()
            {
                Name = "Paralyzed",
                StartMessage = "마비되었습니다.",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(UnityEngine.Random.Range(1,5) ==1 )
                    {
                        pokemon.AddStatusEvent($"{pokemon.Base.Name}(이)가 마비되어 움직일수없다!");
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            StatusConditionID.frz,
            new StatusCondition()
            {
                Name = "Freeze",
                StartMessage = "동상에걸렸습니다.",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(UnityEngine.Random.Range(1,5) ==1 )
                    {
                        pokemon.CureStatus();
                        pokemon.AddStatusEvent($"{pokemon.Base.Name}(이)가 동상상태에서 깨어났습니다!");
                        return true;
                    }
                    return false;
                }
            }
        },
        {
            StatusConditionID.slp,
            new StatusCondition()
            {
                Name = "Sleep",
                StartMessage = "잠에들었다.",
                
                OnStart = (Pokemon pokemon) =>
                {
                    // Sleep for 1~3 turn
                    pokemon.StatusTime = Random.Range(1,4);
                    Debug.Log($"디버그용 {pokemon.StatusTime}만큼 잠들음");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(pokemon.StatusTime <=0)
                    {
                        pokemon.CureStatus();
                        pokemon.AddStatusEvent($"{pokemon.Base.Name}(이)가 깨어났다.");
                        return true;
                    }

                    pokemon.StatusTime--;
                    pokemon.AddStatusEvent($"{pokemon.Base.Name}(이)가 자고있는중이다.");
                    return false;
                }
            }
        },
        //Volatile Status
         {
            StatusConditionID.confusion,
            new StatusCondition()
            {
                Name = "Confusion",
                StartMessage = "혼란에 빠졌다.",

                OnStart = (Pokemon pokemon) =>
                {
                    // Sleep for 1~3 turn
                    pokemon.VolatileStatusTime = Random.Range(1,4);
                    Debug.Log($"디버그용 {pokemon.StatusTime}만큼 혼란에빠짐");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(pokemon.VolatileStatusTime <=0)
                    {
                        pokemon.CureVolatileStatus();
                        pokemon.AddStatusEvent($"{pokemon.Base.Name}(이)가 혼란에서 깨어났다!.");
                        return true;
                    }

                    pokemon.VolatileStatusTime--;

                    if(Random.Range(1,3) ==1)
                        return true;

                    pokemon.AddStatusEvent($"{pokemon.Base.Name}(이)가 혼란에빠져있다.");
                    pokemon.DecreaseHP(pokemon.MaxHp / 8);
                    pokemon.AddStatusEvent(StatusEventType.Damage,$"{pokemon.Base.Name}(이)가 영문도 모른채 자신을 공격했다.");
                    return false;
                }
            }
        },
    };

    public static float GetStatusBonus(StatusCondition condition)
    {
        if (condition == null)
            return 1.0f;
        else if (condition.Id == StatusConditionID.slp || condition.Id == StatusConditionID.frz)
            return 2f;
        else if (condition.Id == StatusConditionID.par || condition.Id == StatusConditionID.psn || condition.Id == StatusConditionID.brn)
            return 1.5f;

        return 1.0f;
    }
}

public enum StatusConditionID
{
    none,psn,brn,slp,par,frz,
    confusion,
}
