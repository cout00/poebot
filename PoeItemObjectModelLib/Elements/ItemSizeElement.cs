namespace PoeItemObjectModelLib.Elements {
    class ItemSizeElement :IItemSize, IElementParser<IItemSize> {
        public int SizeX { get; set; }

        public int SizeY { get; set; }

        public IItemSize ParseElement(string data) {
            var item = ItemBasePreloader.GetItem(data);
            SizeX = item.Size_X;
            SizeY = item.Size_Y;
            return this;
        }
    }
}