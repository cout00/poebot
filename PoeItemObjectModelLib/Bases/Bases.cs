using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeItemObjectModelLib.Bases {


    public static class ItemClass {
        public const string LifeFlask = "LifeFlask";
        public const string ManaFlask = "ManaFlask";
        public const string HybridFlask = "HybridFlask";
        public const string Amulet = "Amulet";
        public const string Ring = "Ring";
        public const string Claw = "Claw";
        public const string Dagger = "Dagger";
        public const string Wand = "Wand";
        public const string One_Hand_Sword = "One Hand Sword";
        public const string Thrusting_One_Hand_Sword = "Thrusting One Hand Sword";
        public const string One_Hand_Axe = "One Hand Axe";
        public const string One_Hand_Mace = "One Hand Mace";
        public const string Bow = "Bow";
        public const string Two_Hand_Sword = "Two Hand Sword";
        public const string Staff = "Staff";
        public const string Two_Hand_Axe = "Two Hand Axe";
        public const string Two_Hand_Mace = "Two Hand Axe";
        public const string Active_Skill_Gem = "Active Skill Gem";
        public const string Support_Skill_Gem = "Support Skill Gem";
        public const string Quiver = "Quiver";
        public const string Belt = "Belt";
        public const string Gloves = "Gloves";
        public const string Boots = "Boots";
        public const string Body_Armour = "Body Armour";
        public const string Helmet = "Helmet";
        public const string Shield = "Shield";
        public const string Currency = "Currency";
        public const string Sceptre = "Sceptre";
        public const string UtilityFlask = "UtilityFlask";
        public const string UtilityFlaskCritical = "UtilityFlaskCritical";
        public const string Map = "Map";
        public const string FishingRod = "FishingRod";
        public const string MapFragment = "MapFragment";
        public const string Jewel = "Jewel";
        public const string DivinationCard = "DivinationCard";
        public const string AbyssJewel = "AbyssJewel";
        public const string UniqueFragment = "UniqueFragment";
        public const string None = "None";

        public static string Coerce(this string str) {
            if(str==Currency) {
                return "StackableCurrency";
            }
            if(str== "StackableCurrency") {
                return Currency;
            }
            return string.Empty;
        }

    }
   
    
    public static class ItemExtensions {
    
        public static ItemStatus ToStatus(this string name) {
            try {
                return (ItemStatus)Enum.Parse(typeof(ItemStatus), name);
            }
            catch (Exception) {
                return ItemStatus.Debug;
            }
        }

        public static ItemRarity ToRarity(this string name) {
            try {
                return (ItemRarity)Enum.Parse(typeof(ItemRarity), name.Replace(" ", "_"));
            }
            catch (Exception) {
                return ItemRarity.Debug;
            }
        }

        public static IEnumerable<string> RemoveEmpty(this IEnumerable<string> self) {
            return self.Where(a => a != string.Empty);
        }

        public static bool IsOneOf<T>(this T self, params T[] args) {
            return args.ToList().Contains(self);
        }

        public static bool IsArmor(this string itemClass) {
            return itemClass.IsOneOf(ItemClass.Shield,
                ItemClass.Body_Armour,
                ItemClass.Boots,
                ItemClass.Gloves,
                ItemClass.Helmet,
                ItemClass.Quiver);
        }

        public static bool IsWeapon(this string itemClass) {
            return itemClass.IsOneOf(ItemClass.Claw,
                ItemClass.Bow,
                ItemClass.Dagger,
                ItemClass.FishingRod,
                ItemClass.One_Hand_Axe,
                ItemClass.One_Hand_Sword,
                ItemClass.Sceptre,
                ItemClass.Staff,
                ItemClass.Thrusting_One_Hand_Sword,
                ItemClass.One_Hand_Mace,
                ItemClass.Two_Hand_Axe,
                ItemClass.Two_Hand_Mace,
                ItemClass.Two_Hand_Sword,
                ItemClass.Wand);
        }

        public static bool IsExtendedItem(this ItemRarity itemRarity) {
            return itemRarity.IsOneOf(ItemRarity.Magic,
                ItemRarity.Rare, ItemRarity.Unique);
        }

        public static bool IsGem(this string itemClass) {
            return itemClass.IsOneOf(ItemClass.Active_Skill_Gem,
                ItemClass.Support_Skill_Gem);
        }

        public static int ToInt(this string str) {
            return int.Parse(str);
        }

        public static double ToDouble(this string str) {
            return double.Parse(str);
        }

        public static double ToFloat(this string str) {
            return float.Parse(str);
        }
    }

}
