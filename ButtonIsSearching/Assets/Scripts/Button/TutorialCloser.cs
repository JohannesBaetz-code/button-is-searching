using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Button
{
    /// <summary>
    /// OnClickHandler for Tutorial Close Button.
    /// </summary>
    /// <author> Raphael Mueller </author>
    /// <date>07.01.2022</date>
    public class TutorialCloser : MonoBehaviour
    {
        [SerializeField] private GameObject tutorial;
    
        public void CloseTutorial()
        {
            Debug.Log("Close!");
            Destroy(tutorial.gameObject);
        }
        
    }
}
