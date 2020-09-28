public abstract class Character {
    // highest level of abstraction for characters (summons, enemies, players)
    public int id { get; protected set; }
    public string name { get; protected set; }

    // TODO: abstract this method and implement in children
    public void ResolveStaticEffects() {

    }
}
