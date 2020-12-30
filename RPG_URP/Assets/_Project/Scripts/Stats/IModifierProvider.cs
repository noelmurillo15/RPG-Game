/*
 * IModifierProvider - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using System.Collections.Generic;

namespace ANM.Stats
{
    public interface IModifierProvider
    {
        //  IEnumerable is the same as IEnumerator except it allows us to use it inside foreach loops
        IEnumerable<float> GetAdditiveModifiers(Stat stat);  
        IEnumerable<float> GetPercentageModifiers(Stat stat);
    }
}