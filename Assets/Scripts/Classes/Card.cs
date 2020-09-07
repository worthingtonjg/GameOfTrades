using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Card 
{
    public EnumRegion Region;

    public string Text;

    public List<EnumCardAction> Actions;

    public string Reward;

    public Card()
    {
        // For serialization
    }

    public Card(EnumRegion region, EnumCardAction action, string reward, string text)
    {
        Region = region;
        Actions = new List<EnumCardAction>();
        Actions.Add(action);

        Reward = reward;
        Text = text;
    }

    public Card(EnumRegion region, List<EnumCardAction> actions, string reward, string text)
    {
        Region = region;
        Actions = actions;
        Reward = reward;
        Text = text;        
    }

    public string GetReward(int multiplier, EnumRewardFormat format = EnumRewardFormat.Text)
    {
        string result = string.Empty;

        int index = 0;
        foreach(var action in Actions)
        {
            string reward = GetRewardFromAction(action, multiplier, format, index++);

            if(!string.IsNullOrEmpty(reward))
            {
                if(string.IsNullOrEmpty(result))
                {
                    result = reward;
                }
                else
                {
                    result = $"{result}  {reward}";
                }
            }
        }

        return result;
    }

    private string GetRewardFromAction(EnumCardAction action, int multiplier, EnumRewardFormat format, int index)
    {
        string reward = Reward;

        if(reward.Contains(","))
        {
            var tokens = Reward.Split(',');
            reward = tokens[index];
        }

        if(format != EnumRewardFormat.Text)
        {
            if(action == EnumCardAction.Gold && format == EnumRewardFormat.Gold)
            {
                return reward;
            }

            if(action == EnumCardAction.Turn && format == EnumRewardFormat.Turns)
            {
                return reward;
            }

            return string.Empty;
        }

        if(action == EnumCardAction.Gold)
        {
            if(reward.StartsWith("+"))
            {
                int goldAmount = int.Parse(reward.Substring(1)) * multiplier;
                return $"Collect {goldAmount} Gold!";
            }
            else if(reward.StartsWith("-"))
            {
                if(reward.Contains(".5"))
                {
                    return "Lose HALF your gold!";
                }
                else if(reward.Contains("*"))
                {
                    return "Lose ALL your gold!";
                }
                else
                {
                    return $"Lose {reward.Substring(1)} Gold!";
                }
            }
        }
        else if(action == EnumCardAction.Turn)
        {
            if(reward.StartsWith("+"))
            {
                return $"Gain {reward.Substring(1)} Turn!";
            }
            else if(reward.StartsWith("-"))
            {
                return $"Lose {reward.Substring(1)} Turn!";
            }            
        }

        return string.Empty;
    }

    public static List<Card> SetupCards()
    {
        var cards = new List<Card>();

        cards.Add(new Card(EnumRegion.MediterraneanSea, EnumCardAction.Gold, "+100", "You begin your expedition on May 10th, giving you the perfect weather to make it from Gibraltar to Greece.  Everything goes well, and you arrive safely."));
        cards.Add(new Card(EnumRegion.MediterraneanSea, EnumCardAction.Gold, "-30", "While sailing from Spain to Italy, the treacherous 'east winds' start blowing your ship off course.  The strength of the wind overpowers your ship and smashes it into a nearby cliff."));
        cards.Add(new Card(EnumRegion.MediterraneanSea, EnumCardAction.Gold, "-100", "Your slaves take longer than expected to create, package, and load your goods onto your trading vessel.  This pushes your venture back until October 15th.  Winter hits early, a thick fog covers the water, and your ship is pounded with rain and snow.  In order to maintain control of the ship, the crew throws all your goods overboard.  While your crew reaches their destination safely, they have no goods to sell the awaiting merchants."));
        cards.Add(new Card(EnumRegion.MediterraneanSea, EnumCardAction.Gold, "+50", "You know the summer weather is almost over, but you gamble and try to maximize your profits by commissioning one more trade run.  You leave from Tunisia on October 5th and head for Istanbul.  While the first few days of your journey have great weather, you can feel winter slowly coming on.  You get scared and cut your journey short, opting to land in Egypt.  Luckily, there are some merchants who are willing to buy some, but not all, of your goods"));
        cards.Add(new Card(EnumRegion.MediterraneanSea, new List<EnumCardAction> { EnumCardAction.Gold, EnumCardAction.Turn}, "-*,-1", "You leave with your crew on June 4th and everything goes unusually well -- that is until you see a pirate ship in the distance.  You order your crew to grab their weapons and prepare to defend your goods.  As the enemy ship approaches, several pirates board your ship, kill most your crew and kidnap you."));
        cards.Add(new Card(EnumRegion.MediterraneanSea, EnumCardAction.Gold, "+200", "Your crew arrives in Spain with a large trade vessel.  Luckily, there has been a local famine and people are desperate for supplies.  This allows you to charge double your normal price."));

        cards.Add(new Card(EnumRegion.SaharaDesert, EnumCardAction.Gold, "+200", "Your 2000 camel caravan arrives on time.  With this large number of camels, you can bring an immense amount of goods to trade along the coast."));
        cards.Add(new Card(EnumRegion.SaharaDesert, EnumCardAction.Gold, "-*", "You attempt to cross the desert with a 100-camel caravan.  Because you are leading such a small caravan, you become easy prey for bandits and they steal everything from you."));
        cards.Add(new Card(EnumRegion.SaharaDesert, EnumCardAction.Gold, "-50", "A sandstorm whips up, leading your caravan off course.  Your guide misses the markers, making you arrive late to your destination."));
        cards.Add(new Card(EnumRegion.SaharaDesert, EnumCardAction.Nothing, "-100", "Half your sleeping camels are affected by scorpion stings and snakebites during the night, causing your caravan to turn back."));
        cards.Add(new Card(EnumRegion.SaharaDesert, EnumCardAction.Gold, "+70", "Your caravan didn't pack enough water and needs to resupply at an oasis for longer than normal.  Some time is lost, and you arrive late, missing the meeting with several of your buyers.  You can't complete your full trade."));
        cards.Add(new Card(EnumRegion.SaharaDesert, EnumCardAction.Gold, "+30", "You pass through the territory of a local warlord during a new business route.  He buys your whole caravan but refuses to give you enough money to make a significant profit.  You are happy to leave with your life."));

        cards.Add(new Card(EnumRegion.IndianOcean, EnumCardAction.Gold, "+50","You begin a desperate trade venture during the off season leaving from East Africa.  You experience extremely heavy headwinds going to Asia to trade, arriving later than you intended."));
        cards.Add(new Card(EnumRegion.IndianOcean, EnumCardAction.Gold, "-*","You are stopped by the Portuguese military and are ordered to submit and become part of their empire.  You refuse and your ships are destroyed."));
        cards.Add(new Card(EnumRegion.IndianOcean, EnumCardAction.Gold, "+100","You request protection from the Chinese empire to protect your investments on your journey to East Africa.  Their Navy protects your trade fleet the whole way allowing your ships to arrive safely."));
        cards.Add(new Card(EnumRegion.IndianOcean, EnumCardAction.Turn, "-1","You arrive in port at the end of the season and all of your buyers already have a full inventory and don't need to buy anything more.  Because the winds and water currents are different, you must wait a year to return to your home port."));
        cards.Add(new Card(EnumRegion.IndianOcean, EnumCardAction.Gold, "-*","Severe monsoons boil up during your voyage to East Africa.  The winds and waves destroy your fleet with all your trading goods lost at sea."));
        cards.Add(new Card(EnumRegion.IndianOcean, EnumCardAction.Gold, "+200","You make a trade venture to China early in the season and arrive ahead of schedule to make a valuable sale of your goods.  This gives you enough time to gather enough inventory for a return visit to East Africa.  You make the return visit before the end of the season and again make a successful sale."));

        cards.Add(new Card(EnumRegion.StraitOfMalacca, EnumCardAction.Gold, "-.5", "You leave from southern China and head towards east India upon hearing there are merchants highly interested in purchasing your supplies.  You successfully arrive and make a massive profit.  On your way back, however, you are ambushed by waiting pirates who take everything.  You are lucky they leave you and your crew alive."));
        cards.Add(new Card(EnumRegion.StraitOfMalacca, EnumCardAction.Gold, "-.5", "Your ship, filled with spices from India, gets caught in a massive thunderstorm.  You helplessly watch as your barrels of expensive spices are tossed into the sea to reduce weight."));
        cards.Add(new Card(EnumRegion.StraitOfMalacca, new List<EnumCardAction> { EnumCardAction.Gold, EnumCardAction.Turn }, "+160,+1", "Before setting sail, you decide to hire additional crew to help defend your cargo from possible attacks.  As your ship emerges from the strait, you see a pirate ship in the distance.  As the pirates attempt to capture your ship, the extra crew you hired not only subdue the attackers, but they capture their ship.  Your bet in hiring more crew paid off."));
        cards.Add(new Card(EnumRegion.StraitOfMalacca, EnumCardAction.Gold, "+50", "Your vessel stocked with Indian spices makes it safely to China, but  you soon discover that this port  has received more than enough spices to meet normal demand.  Some merchants are still willing to purchase your cargo, but at a significantly lower price."));
        cards.Add(new Card(EnumRegion.StraitOfMalacca, EnumCardAction.Turn, "-1", "You plan to sail from China to India, but you and your crew come down with Malaria -- a Mosquito-borne illness that drains your energy.  You are forced to stay ashore while you recover."));
        cards.Add(new Card(EnumRegion.StraitOfMalacca, EnumCardAction.Gold, "+100", "Your ship carrying Chinese silk makes it safely to southern India."));

        return cards;
    }
}
