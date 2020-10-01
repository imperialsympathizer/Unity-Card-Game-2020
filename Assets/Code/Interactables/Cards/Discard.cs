using System.Collections.Generic;

public class Discard : CardStore {
    public Discard() : base() {}

    public void Shuffle() {
        Randomize();
    }
}
