using UnityEngine;
using System.Collections;

public class StateList : ScriptableObject 
{
    public AbstractState Idle;
    public AbstractState Run;
    public AbstractState Attack;
}
