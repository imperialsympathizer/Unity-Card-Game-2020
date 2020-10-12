public class CardRarity {
    private CardRarity(string value) { Value = value; }

    public string Value { get; set; }

    public static CardRarity Common { get { return new CardRarity("Common"); } }
    public static CardRarity Uncommon { get { return new CardRarity("Uncommon"); } }
    public static CardRarity Rare { get { return new CardRarity("Rare"); } }
}