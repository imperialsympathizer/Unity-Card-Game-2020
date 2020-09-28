public abstract class EffectTiming {

    public enum Trigger {
        NONE,           // Some static effects will never be triggered, they are simply persistant
        BEGIN_TURN,
        END_TURN,
        BEGIN_COMBAT,
        END_COMBAT,
        ATTACK,         // When attacking
        HIT,            // When hit by an attack
        DAMAGE,         // When damaged, should not be used in conjunction with HIT
        DAMAGE_NO_ATK,  // When damaged by a non-attack
        HEAL,
        DEATH,
        DRAW,
        DISCARD,
        CARD_PLAY
    }
}