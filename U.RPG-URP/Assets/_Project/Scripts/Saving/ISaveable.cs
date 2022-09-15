/*
 * ISaveable - 
 * Created by : Allan N. Murillo
 * Last Edited : 5/23/2022
 */

namespace ANM.Saving
{
    public interface ISaveable
    {
        object CaptureState();
        void RestoreState(object state);
    }
}