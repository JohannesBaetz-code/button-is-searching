using System;
using UnityEngine;

namespace button
{
    public class StartStopTraversing : MonoBehaviour
    {
        public event Action PauseTraversing;

        public void StartTraversing() => PauseTraversing();
    }
}
