using UnityEngine;
using System.Collections;


/// <summary>
/// The indestructible game object is one which does not get destroyed when transitioning between
/// scenes. It also forbids any creation of more than one instance of itself.
/// </summary>
public class Indestructible : MonoBehaviour 
{
	void Awake()
    {
        if (GameObject.FindObjectsOfType<Indestructible>().Length > 1)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        foreach (var child in this.gameObject.GetComponentsInChildren<Transform>())
            DontDestroyOnLoad(child.gameObject);
    }
}
