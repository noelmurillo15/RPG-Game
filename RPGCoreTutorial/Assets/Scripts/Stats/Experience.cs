/*
 * Experience - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using System;
using ANM.Saving;
using UnityEngine;

namespace ANM.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float expPoints = 0;
        public event Action ExperienceGainedEvent;

        
        public void GainExperience(float exp)
        {
            expPoints += exp;
            ExperienceGainedEvent?.Invoke();
        }

        public float GetExperiencePts()
        {
            return expPoints;
        }

        #region Interface
        public object CaptureState()
        {
            return expPoints;
        }    //    ISaveable

        public void RestoreState(object state)
        {
            expPoints = (float)state;
        }    //    ISaveable
        #endregion
    }
}