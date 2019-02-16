namespace PoeItemObjectModelLib {
    public interface IElementParser<T> where T:IElement {
        T ParseElement(string data);
    }
}