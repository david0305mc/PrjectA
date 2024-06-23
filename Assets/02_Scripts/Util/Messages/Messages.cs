using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UniRx;
//using Cysharp.Text;
using Cysharp.Threading.Tasks;

public enum EMessage
{    
    Update_Heart,
    Update_Coral,
    Update_Skill,
    Update_Artifact,
    Update_Gem,
    Update_Fish,    
    /// <summary>활동 가능한 최대 물고기 수 변경</summary>
    Update_Fish_Activate_Maximum,
    Update_Fish_ProductAmount,
    Update_Fish_Count,
    Update_Fish_ActiveCount,
    Update_Fish_TotalCount,
    Update_Fish_TotalActiveCount,

    Update_MatPropertyBlock,

    Active_Fish_Add,
    Active_Fish_Remove,
    Fish_Unlocked,

    Skill_Use,
    

    /// <summary>소라게 밥먹이기</summary>
    Hermitcrab_UseFood,

    Buy_Coral,	

	Update_Food,
	Update_Crystal,

    Update_Storage_Heart,

    System,
    Error,
    Confirm,

    HeartTouch,
    HeartHarvest,

    UpdateTile,
}


public struct EventParm<T1, T2>
{
    public T1 arg1;
    public T2 arg2;    

    public EventParm(T1 arg1, T2 arg2)
    {
        this.arg1 = arg1;
        this.arg2 = arg2;        
    }
}

public struct EventParm<T1, T2, T3>
{
    public T1 arg1;
    public T2 arg2;
    public T3 arg3;

    public EventParm(T1 arg1, T2 arg2, T3 arg3)
    {
        this.arg1 = arg1;
        this.arg2 = arg2;
        this.arg3 = arg3;
    }
}

public partial class MessageDispather
{
    public static void Publish(EMessage message)
    {
        Publish(message, Unit.Default);        
    }

    public static UniTask PublishAsync(EMessage message, CancellationToken cancellationToken = default(CancellationToken))
    {
        return PublishAsync(message, Unit.Default, cancellationToken);
    }

    public static IObservable<Unit> Receive(EMessage message)
    {
        return Receive<EMessage, Unit>(message);
    }

    public static IObservable<T> Receive<T>(EMessage message)
    {
        return Receive<EMessage, T>(message);         
    }
}