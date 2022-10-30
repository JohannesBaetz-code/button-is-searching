using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FlagCollection
{
    /// <summary>
    /// Animates the FlagObject on Map.
    /// </summary>
    /// <author> Jannick Mitsch </author>
    /// <date>07.01.2022</date>
    public class FlagAnimator : MonoBehaviour
    {
        private SpriteRenderer renderer;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private float duration;
        private int index = 0;
        private float timer = 0;

        void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if ((timer += Time.deltaTime) >= (duration / sprites.Length))
            {
                timer = 0;
                renderer.sprite = sprites[index];
                index = (index + 1) % sprites.Length;
            }
        }
    }
}