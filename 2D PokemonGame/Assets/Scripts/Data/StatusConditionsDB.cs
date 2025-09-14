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
                StartMessage = "���� �����Ǿ����ϴ�",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.DecreaseHP(pokemon.MaxHp / 8);
                    pokemon.AddStatusEvent(StatusEventType.Damage , $"{pokemon.Base.Name}(��)�� ���� �����Ǿ� �����ο��� �������� ������!");
                },
            }
        },
        {
            StatusConditionID.brn,
            new StatusCondition()
            {
                Name = "Burn",
                StartMessage = "ȭ���� �Ծ����ϴ�.",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.DecreaseHP(pokemon.MaxHp / 16);
                    pokemon.AddStatusEvent(StatusEventType.Damage, $"{pokemon.Base.Name}(��)�� ȭ�����Ծ� �����ο��� �������� ������!");
                },
            }
        },
        {
            StatusConditionID.par,
            new StatusCondition()
            {
                Name = "Paralyzed",
                StartMessage = "����Ǿ����ϴ�.",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(UnityEngine.Random.Range(1,5) ==1 )
                    {
                        pokemon.AddStatusEvent($"{pokemon.Base.Name}(��)�� ����Ǿ� �����ϼ�����!");
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
                StartMessage = "���󿡰ɷȽ��ϴ�.",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(UnityEngine.Random.Range(1,5) ==1 )
                    {
                        pokemon.CureStatus();
                        pokemon.AddStatusEvent($"{pokemon.Base.Name}(��)�� ������¿��� ������ϴ�!");
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
                StartMessage = "�ῡ�����.",
                
                OnStart = (Pokemon pokemon) =>
                {
                    // Sleep for 1~3 turn
                    pokemon.StatusTime = Random.Range(1,4);
                    Debug.Log($"����׿� {pokemon.StatusTime}��ŭ �����");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(pokemon.StatusTime <=0)
                    {
                        pokemon.CureStatus();
                        pokemon.AddStatusEvent($"{pokemon.Base.Name}(��)�� �����.");
                        return true;
                    }

                    pokemon.StatusTime--;
                    pokemon.AddStatusEvent($"{pokemon.Base.Name}(��)�� �ڰ��ִ����̴�.");
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
                StartMessage = "ȥ���� ������.",

                OnStart = (Pokemon pokemon) =>
                {
                    // Sleep for 1~3 turn
                    pokemon.VolatileStatusTime = Random.Range(1,4);
                    Debug.Log($"����׿� {pokemon.StatusTime}��ŭ ȥ��������");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(pokemon.VolatileStatusTime <=0)
                    {
                        pokemon.CureVolatileStatus();
                        pokemon.AddStatusEvent($"{pokemon.Base.Name}(��)�� ȥ������ �����!.");
                        return true;
                    }

                    pokemon.VolatileStatusTime--;

                    if(Random.Range(1,3) ==1)
                        return true;

                    pokemon.AddStatusEvent($"{pokemon.Base.Name}(��)�� ȥ���������ִ�.");
                    pokemon.DecreaseHP(pokemon.MaxHp / 8);
                    pokemon.AddStatusEvent(StatusEventType.Damage,$"{pokemon.Base.Name}(��)�� ������ ��ä �ڽ��� �����ߴ�.");
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
