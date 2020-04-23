using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DiceType { Regular, Black };

public class Roll
{
    // Type of dice
    DiceType Type;

    // Number of dice
    int NumOfDice;

    // Whether the roller is an archer or has a bow (roll value is the last die rolled)
    bool BowOrArcherRoll;

    // Whether the roller is a creature or has a helm (roll value is the sum of all identical dice if it is higher than the normal value)
    bool HelmOrCreatureRoll;

    // Results of the roll
    private int[] RollValues;

    // Whether the roll has been made, and is finalized (in the case of the archer)
    bool Finalized = false;

    // Possible dice roll values
    static private int[] RegularDieValues = { 1, 2, 3, 6, 5, 4 };
    static private int[] BlackDieValues = { 6, 6, 8, 10, 10, 12 };

    // Constructor
    public Roll(DiceType Type, int NumOfDice, bool BowOrArcherRoll, bool HelmOrCreatureRoll)
    {
        this.Type = Type;
        this.NumOfDice = NumOfDice;
        this.BowOrArcherRoll = BowOrArcherRoll;
        this.HelmOrCreatureRoll = HelmOrCreatureRoll;
        RollValues = new int[NumOfDice];
    }

    // Factory method that returns a new roll that mimics an existing one (based on the input parameters extracted from the existing roll)
    public static Roll NewMimicRoll(DiceType Type, int NumOfDice, bool BowOrArcherRoll, bool HelmOrCreatureRoll, int[] RollValues)
    {
        Roll NewRoll = new Roll(Type, NumOfDice, BowOrArcherRoll, HelmOrCreatureRoll);

        NewRoll.SetValues(RollValues);

        return NewRoll;
    }

    // Picks a random value for each die based on the allowed sides
    public void RollAllDice()
    {
        int Value, Index;

        // Select which die to use
        int[] DieSides = GetDieSidesToUse();

        // Generate random values
        for (int i = 0; i < NumOfDice; i++)
        {
            Index = Random.Range(0, DieSides.Length);
            Value = DieSides[Index];
            RollValues[i] = Value;
        }

        FinalizeRoll();
    }

    // Used for the archer's special power
    public void RollOneDie()
    {
        int Value = 0;
        int Index;

        // Select which die to use
        int[] DieSides = GetDieSidesToUse();

        int i;

        // Find the first zero value (non rolled) and roll it
        for (i = 0; i < NumOfDice; i++)
        {
            if (RollValues[i] == 0)
            {
                Index = Random.Range(0, DieSides.Length);
                Value = DieSides[Index];
                RollValues[i] = Value;
                break;
            }
        }

        // Check whether all dice have been rolled to automatically finalize the roll
        if (i >= NumOfDice - 1 || Value == 0) FinalizeRoll();
    }

    // Used for the wizard's special power
    public void FlipDie(int Index)
    {
        // Validate the index
        if (Index < 0 || Index >= RollValues.Length)
        {
            Debug.LogError("Cannot flip die #" + Index + "; index is out of range.");
            return;
        }

        // Get the value to flip
        int OldValue = RollValues[Index];

        // Validate the value
        if (OldValue == 0)
        {
            Debug.LogError("Cannot flip a die that hasn't been rolled yet");
            return;
        }

        int NewValue = 0;

        if (Type == DiceType.Regular)
        {
            if (OldValue == 1) NewValue = 6;
            if (OldValue == 2) NewValue = 5;
            if (OldValue == 3) NewValue = 4;
            if (OldValue == 4) NewValue = 3;
            if (OldValue == 5) NewValue = 2;
            if (OldValue == 6) NewValue = 1;
        }
        else if (Type == DiceType.Black)
        {
            if (OldValue == 6) NewValue = 10;
            if (OldValue == 8) NewValue = 12;
            if (OldValue == 10) NewValue = 6;
            if (OldValue == 12) NewValue = 8;
        }

        // Validate result
        if (NewValue == 0)
        {
            Debug.LogError("Error flipping the die at index " + Index + " with value " + OldValue + "; invalid value.");
            return;
        }

        RollValues[Index] = NewValue;
    }

    public int GetRollValue()
    {
        int HighestValue = -1;
        int LastValue = -1;

        // Compute the standard value and archer/bow value
        foreach (int Value in RollValues)
        {
            if (Value > HighestValue) HighestValue = Value;
            if (Value != 0) LastValue = Value;
        }

        // Compute the helm or creature value
        // Sum each distinct die values. For example, three dice values of 4 gives Sum[4] = 12
        int[] Sum = new int[13];        // Die values 0 to 12

        for (int i = 0; i < Sum.Length; i++)
        {
            for (int j = 0; j < RollValues.Length; j++)
            {
                if (i == RollValues[j]) Sum[i] += i;
            }
        }

        // Find the largest sum
        int HighestSum = 0;

        for (int i = 0; i < Sum.Length; i++)
        {
            if (Sum[i] > HighestSum) HighestSum = Sum[i];
        }

        // Determine which value to return
        if (BowOrArcherRoll) return LastValue;
        else if (HelmOrCreatureRoll && HighestSum > HighestValue) return HighestSum;         // Cannot be applied to an archer or bow user
        else return HighestValue;
    }

    public DiceType GetDiceType()
    {
        return Type;
    }

    public int GetNumOfDice()
    {
        return NumOfDice;
    }

    public bool GetBowOrArcherRoll()
    {
        return BowOrArcherRoll;
    }

    public bool GetHelmOrCreatureRoll()
    {
        return HelmOrCreatureRoll;
    }

    public int[] GetValues()
    {
        return RollValues;
    }

    public void SetValues(int[] Values)
    {
        this.RollValues = Values;
    }

    // Returns the correct array of die sides to use
    private int[] GetDieSidesToUse()
    {
        int[] DieSides = null;
        switch (Type)
        {
            case DiceType.Regular:
                DieSides = RegularDieValues;
                break;
            case DiceType.Black:
                DieSides = BlackDieValues;
                break;
        }
        return DieSides;
    }

    // Returns whether this roll has been started
    public bool RollIsStarted()
    {
        foreach (int Value in RollValues)
        {
            if (Value != 0) return true;
        }
        return false;
    }

    // Returns whether the roll is finished
    public bool RollIsFinished()
    {
        return Finalized;
    }

    // Marks the roll as finished (can be called externally in the case of the archer and internally for all other heroes0
    public void FinalizeRoll()
    {
        Finalized = true;
    }
}
