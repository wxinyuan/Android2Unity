using UnityEngine;
using System.Collections;

/// <summary>
/// 单例组件
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    private static T _instance;
    private static bool _init;

	protected virtual void Awake() {
		_instance = this as T;
		_init = true;
	}

    protected virtual void OnDestroy()
    {
        _instance = null;
        //_init = false;
    }

    public static T instance
    {
        get
        {

            if (_init == false)
            {
                _init = true;
				GameObject managerGo = GameObject.Find("Singleton");
                if (managerGo == null) {
					managerGo = new GameObject("Singleton");
                    DontDestroyOnLoad(managerGo);
                }
                string managerName = typeof(T).ToString();
				_instance = managerGo.GetComponentInChildren<T>();
				if (_instance == null) {
					GameObject go= new GameObject(managerName);
					go.transform.parent = managerGo.transform;
					_instance = go.AddComponent<T>();
				}
			}
			else if (!hasInstance())
			{
                Debug.LogWarning("SingletonMonoBehaviour is null");
			}
            return SingletonMonoBehaviour<T>._instance;
        }
    }

    public static bool hasInstance()
    {
        return _instance != null;
    }
}
