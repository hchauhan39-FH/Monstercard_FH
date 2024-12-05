using System;
using System.Collections.Generic;

namespace Monstercard_FH.Models
{
    public enum ElementType
    {
        Water,
        Fire,
        Normal
    }

    public abstract class Card
    {
        private static readonly Dictionary<string, ElementType> ElementMappings = new()
        {
            // Fire type mappings
            { "Dragons", ElementType.Fire },
            { "FireElves", ElementType.Fire },
            { "Amaterasu", ElementType.Fire },
            { "Raijin", ElementType.Fire },
            { "Tetsu", ElementType.Fire },
            
            // Water type mappings
            { "Kraken", ElementType.Water },
            { "DogMike", ElementType.Water },
            { "Susanoo", ElementType.Water },
            { "Bankai", ElementType.Water },
            { "Wizzard", ElementType.Water },
            
            // Normal type mappings
            { "Goblins", ElementType.Normal },
            { "Knights", ElementType.Normal },
            { "Orcs", ElementType.Normal },
            { "Rocklee", ElementType.Normal },
            { "Chauhan", ElementType.Normal }
        };

        private static readonly Dictionary<ElementType, int> BaseDamageByType = new()
        {
            { ElementType.Normal, 50 },
            { ElementType.Fire, 70 },
            { ElementType.Water, 70 }
        };

        public string Name { get; }
        public int Damage { get; }
        public ElementType CardElementType { get; }

        protected Card(string name)
        {
            Name = name;
            CardElementType = DetermineElementType(name);
            Damage = BaseDamageByType[CardElementType];
        }

        private static ElementType DetermineElementType(string name) =>
            ElementMappings.TryGetValue(name, out var elementType) ? elementType : ElementType.Normal;

        public double CalculateDamage(Card opponent)
        {
            double effectiveness = CalculateEffectiveness(opponent.CardElementType);
            return Damage * effectiveness;
        }

        private double CalculateEffectiveness(ElementType opponentType)
        {
            return (CardElementType, opponentType) switch
            {
                (ElementType.Water, ElementType.Fire) => 2.0,
                (ElementType.Fire, ElementType.Normal) => 2.0,
                (ElementType.Normal, ElementType.Water) => 2.0,
                (ElementType.Fire, ElementType.Water) => 0.5,
                (ElementType.Normal, ElementType.Fire) => 0.5,
                (ElementType.Water, ElementType.Normal) => 0.5,
                _ => 1.0 // Same type or undefined match-ups
            };
        }

        public abstract void PlayCard();
    }
}
