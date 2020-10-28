using System;

[Serializable]
public class Element {
    public readonly ElementType type;
    // Some entities may have multiple of the same element
    // Count should be used instead of creating new Element objects
    public int count;

    public enum ElementType {
        AIR,
        EARTH,
        FIRE,
        WATER,
        LIFE,
        DEATH,
        ARTIFICE
    }

    public Element(ElementType type, int count) {
        this.type = type;
        this.count = count;
    }

    public static string GetElementString(ElementType elementType) {
        string result;
        switch (elementType) {
            default:
            case ElementType.AIR:
                result = "Air";
                break;
            case ElementType.EARTH:
                result = "Earth";
                break;
            case ElementType.FIRE:
                result = "Fire";
                break;
            case ElementType.WATER:
                result = "Water";
                break;
            case ElementType.LIFE:
                result = "Life";
                break;
            case ElementType.DEATH:
                result = "Death";
                break;
            case ElementType.ARTIFICE:
                result = "Artifice";
                break;
        }

        return result;
    }
}
