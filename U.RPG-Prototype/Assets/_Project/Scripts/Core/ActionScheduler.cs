/*
 * ActionScheduler - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using UnityEngine;

namespace ANM.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        private IAction _currentAction;


        public void StartAction(IAction action)
        {
            if (_currentAction == action) return;

            _currentAction?.Cancel();
            _currentAction = action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}