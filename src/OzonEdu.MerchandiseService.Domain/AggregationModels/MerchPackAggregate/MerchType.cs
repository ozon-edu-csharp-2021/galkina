using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate
{
    public abstract class MerchType: Enumeration, IEnumerable<ItemType>
    {
        private List<ItemType> items;
        
        public static readonly MerchType Welcome = new WelcomeType();
        public static readonly MerchType ConferenceListener = new ConferenceListenerType();
        public static readonly MerchType ConferenceSpeaker = new ConferenceSpeakerType();
        public static readonly MerchType Starter = new StarterType();
        public static readonly MerchType Veteran = new VeteranType();

        private MerchType(int value, string name) : base(value, name)
        {
        }
        
        public List<ItemType> Items
        {
            get { return items; }
        }

        private class WelcomeType : MerchType
        {
            public WelcomeType() : base(10, nameof(Welcome))
            {
                items = new List<ItemType>()
                {
                    ItemType.Pen, 
                    ItemType.Notepad
                };
            }
        }
        
        private class ConferenceListenerType : MerchType
        {
            public ConferenceListenerType() : base(20, nameof(ConferenceListener))
            {
                items = new List<ItemType>()
                {
                    ItemType.Pen, 
                    ItemType.Notepad
                };
            }
        }
        
        private class ConferenceSpeakerType : MerchType
        {
            public ConferenceSpeakerType() : base(30, nameof(ConferenceSpeaker))
            {
                items = new List<ItemType>()
                {
                    ItemType.TShirt,
                    ItemType.Pen,
                    ItemType.Notepad
                };
            }
        }
        
        private class StarterType : MerchType
        {
            public StarterType() : base(40, nameof(Starter))
            {
                items = new List<ItemType>()
                {
                    ItemType.TShirt,
                    ItemType.Sweatshirt
                };
            }
        }
        
        private class VeteranType : MerchType
        {
            public VeteranType() : base(50, nameof(Veteran))
            {
                items = new List<ItemType>()
                {
                    ItemType.CardHolder,
                    ItemType.Socks
                };
            }
        }
        
        public static MerchType Parse(int merchPackIndex)
            => merchPackIndex switch
            {
                10  => new WelcomeType(),
                20  => new ConferenceListenerType(),
                30  => new ConferenceSpeakerType(),
                40  => new StarterType(),
                50  => new VeteranType(),
                _ => throw new ArgumentException($"Unknown merch pack type {merchPackIndex}.")
            };

        public int ParseToInt()
            => this.Id;
        
        public IEnumerator<ItemType> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (obj is not MerchType)
                return false;

            MerchType mt = (MerchType) obj;
            
            return this.Id == mt.Id;
        }
        
        public static bool operator == (MerchType a, MerchType b) => a.Equals(b);
        
        public static bool operator != (MerchType a, MerchType b) => !a.Equals(b);
    }
}