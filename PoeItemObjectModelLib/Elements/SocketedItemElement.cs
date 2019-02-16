using System.Linq;

using PoeItemObjectModelLib.Bases;

namespace PoeItemObjectModelLib {
    class SocketedItemElement :ISocketedItem, IElementParser<ISocketedItem> {
        public int SocketsCount { get; set; }

        public ISocketedItem ParseElement(string data) {
            var line = data.Replace("Sockets: ", string.Empty);
            var res = line.Where(a => ItemExtensions.IsOneOf(a, 'R', 'G', 'B')).Count();
            SocketsCount = res;
            return this;
        }
    }
}