using UnityEngine;

/**
 * AbstractState for unity inspector exposure/serialization
 */ 
public abstract class AbstractState : MonoBehaviour, IState {
    public abstract void execute();
}
