using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumGameState 
{
    SpinningWheel,
    InTurn,
    BetweenTurns,
    ContinueOrRetire
}

public enum EnumSpinValues
{
    IndianOcean,
    StraitOfMalacca,
    MediterraneanSea,
    SaharaDesert,
    YourChoice,
    LoseATurn,
    GainATurn,
    DoubleGoldEarned
}

public enum EnumRegion
{
    IndianOcean,
    StraitOfMalacca,
    MediterraneanSea,
    SaharaDesert,
}

public enum EnumCardAction
{
    Gold,
    Turn,
    Nothing,
}

public enum EnumRewardFormat
{
    Gold,
    Turns,
    Text,
}