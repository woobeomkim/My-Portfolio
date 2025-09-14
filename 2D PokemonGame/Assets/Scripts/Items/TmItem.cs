using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(menuName = "Items/Create new TM or HM")]
public class TmItem : ItemBase
{
    [SerializeField] MoveBase move;
    [SerializeField] bool isHM;

    public override string Name => base.Name + $": {move.Name}";
    
    public override bool Use(Pokemon pokemon)
    {
        // �κ��丮 UI��ũ��Ʈ���� ����� ����ٸ� TRUE����ȯ 
        return pokemon.HasMove(move);
    }

    public bool CanBeTaugth(Pokemon pokemon)
    {
        return pokemon.Base.LearnableByItems.Contains(move);
    }

    public override bool CanUseInBattle => false;

    public override bool IsReusable => isHM;
    public MoveBase Move => move;

    public bool IsHM => isHM;
}
