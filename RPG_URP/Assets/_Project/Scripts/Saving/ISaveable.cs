/*
 * ISaveable - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

namespace ANM.Saving
{
    public interface ISaveable
    {
        object CaptureState();
        void RestoreState(object state);
    }
}