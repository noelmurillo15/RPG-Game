using UnityEngine;
using RPG.Saving;
using System;


namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float expPoints = 0;

        public event Action onExperienceGained; //  Using Action is the same as having delegate + event
        // public delegate void ExpGainedDelegate();    //  not needed for Action


        public void GainExperience(float exp)
        {
            expPoints += exp;
            onExperienceGained();
        }

        public float GetExperiencePts()
        {
            return expPoints;
        }

        #region Interface
        public object CaptureState()
        {
            return expPoints;
        }

        public void RestoreState(object state)
        {
            expPoints = (float)state;
        }
        #endregion
    }
}