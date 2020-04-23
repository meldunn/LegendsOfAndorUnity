using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRound
{
    // Heroes participating in this round
    List<Hero> Participants;

    // Creature
    Creature Creature;

    // Hero rolls
    Dictionary<Hero, Roll> HeroRolls = new Dictionary<Hero, Roll>();

    // Creature roll
    Roll CreatureRoll;

    // Keeps track of whether the wizard has flipped the die this round
    bool WizardFlippedDie = false;

    // Constructor
    public BattleRound(Creature Creature, List<Hero> Participants)
    {
        this.Creature = Creature;
        this.Participants = Participants;

        // Deduct one hour from each participating hero
        foreach (Hero Hero in Participants)
        {
            Hero.AdvanceTimeMarker(1);
        }
    }

    // Rolls the dice for the specified hero. Returns true if the roll is new; false if it is continued (for archer / bow user).
    public bool RollHeroDice(Hero Hero)
    {
        bool IsNewRoll = false;

        // Get the hero's dice
        DiceType HeroDiceType = Hero.GetDiceType();
        int HeroNumDice = Hero.GetNumOfDice();

        // Check whether the hero already has a roll in progress (for the Archer)
        Roll CurrentRoll;
        HeroRolls.TryGetValue(Hero, out CurrentRoll);

        if (CurrentRoll == null)
        {
            IsNewRoll = true;

            // Check whether the roll is an archer/bow roll (use last die)
            bool BowOrArcherRoll = false;
            if (Hero.GetHeroType() == HeroType.Archer) BowOrArcherRoll = true;

            // Create a new roll for the hero
            CurrentRoll = new Roll(HeroDiceType, HeroNumDice, BowOrArcherRoll);

            // Add the roll to the mapping
            HeroRolls.Add(Hero, CurrentRoll);
        }

        // Launch (or continue) the roll
        if (Hero.GetHeroType() == HeroType.Archer) CurrentRoll.RollOneDie();
        else
        {
            // Validate that the roll has not been made already
            if (CurrentRoll.RollIsFinished())
            {
                Debug.LogError("The " + Hero.GetHeroType() + "cannot roll the dice twice in the same round.");
                IsNewRoll = false;
            }

            CurrentRoll.RollAllDice();
        }

        return IsNewRoll;
    }

    // Sets the roll for the specified hero (used to set a copy of a roll made on another machine)
    public void SetHeroRoll(Hero Hero, Roll Roll)
    {
        HeroRolls.Add(Hero, Roll);
    }

    // Sets the roll for the creature (used to set a copy of a roll made on another machine)
    public void SetCreatureRoll(Roll Roll)
    {
        CreatureRoll = Roll;
    }

    public void RollCreatureDice()
    {
        // Get the creature's dice
        DiceType CreatureDiceType = Creature.GetDiceType();
        int CreatureNumDice = Creature.GetNumOfDice();

        // Create a new roll for the creature
        CreatureRoll = new Roll(CreatureDiceType, CreatureNumDice, false);

        // Launch the roll
        CreatureRoll.RollAllDice();
    }

    // Gets the roll for the specified hero
    public Roll GetRoll(Hero Hero)
    {
        if (Hero == null) return null;

        Roll CurrentRoll;
        HeroRolls.TryGetValue(Hero, out CurrentRoll);
        return CurrentRoll;
    }

    public Roll GetCreatureRoll()
    {
        return CreatureRoll;
    }

    public int[] GetHeroRollValues(Hero Hero)
    {
        if (Hero == null) return new int[0];
        Roll CurrentRoll = GetRoll(Hero);
        if (CurrentRoll == null) return new int[0];
        else return CurrentRoll.GetValues();
    }

    public int[] GetCreatureRollValues()
    {
        if (CreatureRoll == null) return new int[0];
        else return CreatureRoll.GetValues();
    }

    // Gets the roll dice type for the specified hero
    public DiceType GetRollDiceType(Hero Hero)
    {
        if (Hero == null) return DiceType.Regular;                  // Default
        Roll CurrentRoll;
        HeroRolls.TryGetValue(Hero, out CurrentRoll);
        if (CurrentRoll == null) return Hero.GetDiceType();         // Result before rolling
        else return CurrentRoll.GetDiceType();                      // Result after rolling
    }

    // Gets the roll dice type for the creature
    public DiceType GetCreatureRollDiceType()
    {
        if (CreatureRoll == null) return Creature.GetDiceType();    // Result before rolling
        else return CreatureRoll.GetDiceType();                     // Result after rolling
    }
    
    public bool HasStartedRoll(Hero Hero)
    {
        if (Hero == null) return false;
        // Check whether the hero already has a roll in progress
        Roll CurrentRoll;
        HeroRolls.TryGetValue(Hero, out CurrentRoll);

        if (CurrentRoll == null) return false;
        else return CurrentRoll.RollIsStarted();
    }

    public bool HasFinishedRoll(Hero Hero)
    {
        if (Hero == null) return false;
        // Check whether the hero already has a roll in progress
        Roll CurrentRoll;
        HeroRolls.TryGetValue(Hero, out CurrentRoll);

        if (CurrentRoll == null) return false;
        else return CurrentRoll.RollIsFinished();
    }

    public bool WizardHasFlippedDie()
    {
        return WizardFlippedDie;
    }

    public void FlipDie(Hero Hero, int DieIndex)
    {
        // Check that the wizard has not already flipped a die in this round
        if (WizardFlippedDie)
        {
            Debug.LogError("Error: The wizard cannot flip a die more than once per battle round.");
            return;
        }

        // Find the hero's roll
        Roll CurrentRoll;
        HeroRolls.TryGetValue(Hero, out CurrentRoll);

        if (CurrentRoll == null)
        {
            Debug.LogError("Cannot flip a die for a hero who has not rolled yet.");
            return;
        }

        WizardFlippedDie = true;

        CurrentRoll.FlipDie(DieIndex);
    }

    public int GetRollValue(Hero Hero)
    {
        if (Hero == null) return 0;
        // Check whether the hero already has a roll in progress
        Roll CurrentRoll;
        HeroRolls.TryGetValue(Hero, out CurrentRoll);

        if (CurrentRoll == null) return 0;
        else return CurrentRoll.GetRollValue();
    }

    public int GetCreatureRollValue()
    {
        if (CreatureRoll == null) return 0;
        else return CreatureRoll.GetRollValue();
    }

    public int GetHeroBattleValue()
    {
        int BattleValue = 0;

        // Iterate through all the participating heroes and sum their rolls and strength
        foreach (Hero Participant in Participants)
        {
            // Check whether the hero already has a roll in progress
            Roll CurrentRoll;
            HeroRolls.TryGetValue(Participant, out CurrentRoll);

            if (CurrentRoll != null) BattleValue += CurrentRoll.GetRollValue() + Participant.getStrength();
        }

        return BattleValue;
    }

    public int GetCreatureBattleValue()
    {
        if (CreatureRoll == null) return 0;
        else return CreatureRoll.GetRollValue() + Creature.GetStrength();
    }

    public int GetHeroLostWillpower()
    {
        int HeroBV = GetHeroBattleValue();
        int CreatureBV = GetCreatureBattleValue();

        if (HeroBV == 0 || CreatureBV == 0) return 0;       // The battle values have not both been calculated, so no damage was taken yet
        else if (CreatureBV > HeroBV) return CreatureBV - HeroBV;
        else return 0;
    }

    public int GetCreatureLostWillpower()
    {
        int HeroBV = GetHeroBattleValue();
        int CreatureBV = GetCreatureBattleValue();

        if (HeroBV == 0 || CreatureBV == 0) return 0;       // The battle values have not both been calculated, so no damage was taken yet
        else if (HeroBV > CreatureBV) return HeroBV - CreatureBV;
        else return 0;
    }

    public bool IsDone()
    {
        // A round is done if all its participating heroes have finished their rolls
        foreach (Hero Hero in Participants)
        {
            // Check whether the hero already has a roll in progress
            Roll CurrentRoll;
            HeroRolls.TryGetValue(Hero, out CurrentRoll);

            if (CurrentRoll == null) return false;
            else if (!CurrentRoll.RollIsFinished()) return false;
        }
        return true;
    }

    public void FinalizeRoll(Hero Hero)
    {
        if (Hero == null) return;
        // Check whether the hero already has a roll in progress
        Roll CurrentRoll;
        HeroRolls.TryGetValue(Hero, out CurrentRoll);

        if (CurrentRoll == null) return;
        else CurrentRoll.FinalizeRoll();
    }
}
