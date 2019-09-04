
using System.Collections.Generic;


namespace RPG.Stats
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifiers(Stat stat);  //  IEnumerable is the same as IEnumerator except it allows us to use it inside foreach loops
        IEnumerable<float> GetPercentageModifiers(Stat stat);
    }
}