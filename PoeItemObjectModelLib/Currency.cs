namespace PoeItemObjectModelLib {
    public class Currency :ItemModel, ICurrency {
        public int MaxStackSize { get; set; }
        public int StackSize { get; set; }
    }
}