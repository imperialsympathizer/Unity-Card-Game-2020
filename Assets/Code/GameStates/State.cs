﻿using System.Collections;

public abstract class State {
    protected TurnSystem TurnSystem;

    public State(TurnSystem turnSystem) {
        TurnSystem = turnSystem;
    }

    public virtual IEnumerator Start() {
        yield break;
    }

    public bool CheckGameConditions() {
        return TurnSystem.CheckGameConditions();
    }
}
