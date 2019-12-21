using System.Collections;
using UnityEngine;




public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	public static T Instance;
        
    protected virtual void Awake()
	{
		if (Instance == null)
		{
			Instance = (T)this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

}
