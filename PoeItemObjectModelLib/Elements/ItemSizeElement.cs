namespace PoeItemObjectModelLib.Elements {
    class ItemSizeElement :IItemSize, IElementParser<IItemSize> {
        public int SizeX { get; set; }

        public int SizeY { get; set; }

        public IItemSize ParseElement(string data) {
            var item = ItemBasePreloader.GetItem(data);
            if (item==null) {
                SizeX = 1;
                SizeY = 1;
                return this; 
            }
            SizeX = item.Size_X;
            SizeY = item.Size_Y;
            return this;
        }
    }
}