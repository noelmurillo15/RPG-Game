using UnityEngine;
using RPG.Saving;
using System;


namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float expPoints = 0;

        public event Action OnExperienceGained; //  Using Action is the same as having delegate + event
        // public delegate void ExpGainedDelegate();    //  not needed for Action


        public void GainExperience(float exp)
        {
            expPoints += exp;
            OnExperienceGained?.Invoke();
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