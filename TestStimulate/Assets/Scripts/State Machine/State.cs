using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase
{
    public bool IsInitialize { get; set; } = false;
    public abstract void StartState();
    public abstract void UpdateState();
    public abstract void EndState();
    public virtual void FixedUpdateState() { }
    public virtual void LateUpdateState() { }
    public virtual void TriggerState() { }
    public virtual void StopCoroutine() { }
}

public abstract class State<T> : StateBase
{
    protected T data;
    public abstract void Initialize(T data);
}




