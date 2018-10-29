using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

    public static T instance { get; private set; }

    protected virtual void Awake() {
        if (instance != null) {
            Debug.LogError("A " + name + " instance already exists");
            Destroy(this); //Or GameObject as appropriate
            return;
        }

        instance = this as T;
        DontDestroyOnLoad(instance);
    }
}