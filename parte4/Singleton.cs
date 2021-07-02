using UnityEngine;
using System.Collections;

public class Singleton<Instance> : MonoBehaviour where Instance : Singleton<Instance>
{
	public static Instance main;

	private static object _lock = new object();

	public bool isPersistant;

	public virtual void MyAwake() {}

	public virtual void Awake()
	{
		if(isPersistant)
		{
			if(!main)
			{
				main = this as Instance;
			}
			else
			{
				DestroyObject(gameObject);
			}
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			main = this as Instance;
		}
		StartCoroutine(WaitAwake());
	}

	IEnumerator WaitAwake()
	{
		yield return 0;
		// print("Awake singleton"+this.name);
		MyAwake();
	}

	public static Instance Instancer
	{
		get
		{
			if (applicationIsQuitting) {
				Debug.LogWarning("[Singleton] Instance '"+ typeof(Instance) +
					"' already destroyed on application quit." +
					" Won't create again - returning null.");
				return null;
			}

			lock(_lock)
			{
				if (main == null)
				{
					main = (Instance) FindObjectOfType(typeof(Instance));

					if ( FindObjectsOfType(typeof(Instance)).Length > 1 )
					{
						Debug.LogError("[Singleton] Something went really wrong " +
							" - there should never be more than 1 singleton!" +
							" Reopenning the scene might fix it.");
						return main;
					}

					if (main == null)
					{
						GameObject singleton = new GameObject();
						main = singleton.AddComponent<Instance>();
						singleton.name = "~"+ typeof(Instance).ToString();

						DontDestroyOnLoad(singleton);

						Debug.Log("[Singleton] An instance of " + typeof(Instance) + 
							" is needed in the scene, so '" + singleton +
							"' was created with DontDestroyOnLoad.");
					} else {
						Debug.Log("[Singleton] Using instance already created: " +
							main.gameObject.name);
					}
				}

				return main;
			}
		}
	}

	private static bool applicationIsQuitting = false;
	/// <summary>
	/// When Unity quits, it destroys objects in a random order.
	/// In principle, a Singleton is only destroyed when application quits.
	/// If any script calls Instance after it have been destroyed, 
	///   it will create a buggy ghost object that will stay on the Editor scene
	///   even after stopping playing the Application. Really bad!
	/// So, this was made to be sure we're not creating that buggy ghost object.
	/// </summary>
	public void OnDestroy ()
	{
		applicationIsQuitting = true;
	}

}