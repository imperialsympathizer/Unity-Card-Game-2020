public abstract class Character {
    // highest level of abstraction for characters (summons, enemies, players)
    // No instantiable classes should be directly inheriting from this class
    public readonly int id;
    public readonly string name;

    public Character(string name) {
        this.id = ResourceController.GenerateId();
        this.name = name;
    }

    // TODO: abstract this method and implement in children
    public void ResolveStaticEffects() {

    }
}
