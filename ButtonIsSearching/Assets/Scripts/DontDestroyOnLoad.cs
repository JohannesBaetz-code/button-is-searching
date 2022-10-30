using UnityEngine;

/// <summary> The DontDestroyOnLoad is a helpful Unity Class saving objects and data over the course of loading iterations </summary>
/// <author>Fanny Weidner</author>
public class DontDestroyOnLoad : MonoBehaviour
{
    /// <summary>
    ///     Upon the creation of the object that keeps the DontDestroyOnLoad Class, the object cannot be destroyed
    ///     in the following scenes or when new ones are overwritten
    /// </summary>
    /// <param name="DontDestroyOnLoad">Keeps an object and its data across various scenes.</param>
    /// <author>Fanny Weidner</author>
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}