using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MahApps.Metro.Controls;
using System.IO;
using System.Net;

using PoeItemObjectModelLib;
using PoeItemObjectModelLib.Bases;
using PoeItemObjectModelLib.Elements;
using PoeItemObjectModelLib.PickitEngine.PickitStatistic;

namespace UI {

    public partial class MainWindow :MetroWindow {
        public MainWindow() {
            InitializeComponent();
            //ItemFactory factory = new ItemFactory();
            //var test = factory.GetModel();
            //Pickit pickit=new Pickit();
            //var valid = pickit.IsValid(test);

            StatisticProcessor builder=new StatisticProcessor();
            builder.OnData += Builder_OnData;
            builder.Add(new Currency() {Class = ItemClass.Currency, BaseName = "JewellerOrb", StackSize = 4}, Destination.Keep);
            builder.Add(new Currency() {Class = ItemClass.Currency, BaseName = "JewellerOrb", StackSize = 2}, Destination.Keep);
            builder.Add(new Currency() {Class = ItemClass.Currency, BaseName = "JewellerOrb", StackSize = 8}, Destination.Keep);
            builder.Add(new Currency() {Class = ItemClass.Currency, BaseName = "AlchemyOrb", StackSize = 1}, Destination.Keep);
            builder.Add(new Currency() {Class = ItemClass.Currency, BaseName = "AlchemyOrb", StackSize = 3}, Destination.Keep);
            builder.Add(new Currency() {Class = ItemClass.Currency, BaseName = "ExaltedOrb", StackSize = 1}, Destination.Keep);

            builder.Add(new DivinationCard() {Class = ItemClass.DivinationCard, BaseName = "Humility", StackSize = 1}, Destination.Keep);
            builder.Add(new DivinationCard() {Class = ItemClass.DivinationCard, BaseName = "Humility", StackSize = 1}, Destination.Keep);
            builder.Add(new DivinationCard() {Class = ItemClass.DivinationCard, BaseName = "Humility", StackSize = 2}, Destination.Keep);
            builder.Add(new DivinationCard() {Class = ItemClass.DivinationCard, BaseName = "Humility", StackSize = 3}, Destination.Keep);

            builder.Add(new DivinationCard() {Class = ItemClass.DivinationCard, BaseName = "Doctor", StackSize = 3}, Destination.Keep);
            builder.Add(new DivinationCard() {Class = ItemClass.DivinationCard, BaseName = "Doctor", StackSize = 3}, Destination.Keep);
            builder.Add(new DivinationCard() {Class = ItemClass.DivinationCard, BaseName = "Doctor", StackSize = 3}, Destination.Keep);

            builder.Add(new Weapons() {Class = ItemClass.One_Hand_Axe, Rarity =ItemRarity.Unique,  BaseName = "Doctor"}, Destination.Keep);
            builder.Add(new Weapons() {Class = ItemClass.One_Hand_Axe, Rarity =ItemRarity.Unique,  BaseName = "Doctor"}, Destination.Keep);
            builder.Add(new Weapons() {Class = ItemClass.One_Hand_Axe, Rarity =ItemRarity.Unique,  BaseName = "Doctor"}, Destination.Keep);
            builder.Add(new Weapons() {Class = ItemClass.One_Hand_Axe, Rarity =ItemRarity.Unique,  BaseName = "Doctor"}, Destination.Keep);

            builder.Add(new Weapons() {Class = ItemClass.One_Hand_Axe, Rarity =ItemRarity.Magic,  BaseName = "Doctor"}, Destination.Keep);
            builder.Add(new Weapons() {Class = ItemClass.One_Hand_Axe, Rarity =ItemRarity.Magic,  BaseName = "Doctor"}, Destination.Keep);

            //var str = 
        }

        private void Builder_OnData(object sender, StatisticProcessor e) {
            var str1 = e.CurrencyStatistic.MorphString();
            var str2 = e.DivCardStatistic.MorphString();
            var str3 = e.UniqueItemsStatistic.MorphString();
        }
    }
}
