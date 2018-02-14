// Allan Murillo : Unity RPG Core Test Project


namespace RPG {

    public enum CharacterTypes
    {
        PLAYER, //  Me
        NPC,    //  Advanced Ai
        ENEMY   //  Basic Ai 
    }

    public enum CharacterRanks
    {               //  AI Behaviours
        MINION,     //  Green,  Lv 1-5      Scared of all other Ranks, fight amongst themselves
        COMMON,     //  Grey,   Lv 6-10     Hunt MINION & COMMON, Scared of ELITE, LEGENDARY, UBER & BOSS        
        ELITE,      //  Orange, Lv 11-15    Can have group of COMMON & MINION, Hunts LEGENDARY, ELITE, Scared of UBER & BOSS
        LEGENDARY,  //  Yellow, Lv 16-20    Loner, fight amongst selves, waits for UBER to be low hp and attempts to Kill, Tactics, Scared of BOSS
        UBER,       //  Purple, Lv 21-25    Work in teams to take down BOSS, all for glory, if no BOSS, Hunt selves, Ignore rest
        BOSS        //  Black,  Lv 26-30    Wrecks Havoc
    }

    public enum CharacterClasses
    {               // Based on Attribute Distribution
        NONE,
        FIGHTER,    // + 10 in Strength
        SORCERER,   // + 10 in Wisdom
        DEFENDER,   // + 10 in Endurance
        ASSASSIN,   // FIGHTER +10 in any other stat or Lv 26  
        WARLOCK,    // SORCERER +10 in any other stat or Lv 26  
        TANK        // DEFENDER +10 in any other stat or Lv 26    
    }

    public enum CharacterPhysique
    {               //  AI Stats
        HEALTH,     //  Hit pts  ~ Affected when Hungry, Thirsty & Tired
        MANA       //  Mana Pts ~ Affected when Hungry & Thirsty
        //  TODO : implement physical status feature (Physique)
        //STAMINA,    //  How long you can sprint for
        //HUNGER,     //  0-100, Affects STRENGTH, WISDOM  
        //THIRST,     //  0-100, Affects ENDURANCE, STAMINA 
        //SLEEP,      //  0-100, Affects STAMINA recharge   
    }

    public enum CharacterAttributes
    {               //  AI Permanent Buffs  //  start with 0pts, each lv gain 1 pt
        STRENGTH,   //  Affects : Physical dmg, HP
        WISDOM,     //  Affects : Magic dmg, Mana
        ENDURANCE,  //  Affects : Dmg resist, Stamina
        LUCK        //  Affects : Critical Dmg , More Exp, Killed In Sleep Less
    }
}