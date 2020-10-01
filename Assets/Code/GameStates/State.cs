using System.Collections;
using UnityEngine;

public abstract class State {
    protected TurnSystem TurnSystem;

    public State(TurnSystem turnSystem) {
        TurnSystem = turnSystem;
    }

    public virtual IEnumerator Start() {
        yield break;
    }

    public void CheckGameConditions() {
        TurnSystem.CheckGameConditions();
    }
}
