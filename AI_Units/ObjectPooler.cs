using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
	
	[System.Serializable]
	public class Pool
	{
		public string tag;
		public GameObject prefab;
		public int Size;
	}
	public List<Pool> pools;
	public Dictionary<string, Queue<GameObject>> poolDictionary;

	#region  Singleton
	public static ObjectPooler Instance;
	private void Awake() {
		Instance = this;
		poolDictionary = new Dictionary<string, Queue<GameObject>>();

		foreach(Pool pool in pools)
		{
			Queue<GameObject> obj_pool = new Queue<GameObject>();

			for(int i = 0; i < pool.Size; i++)
			{
				GameObject obj = Instantiate(pool.prefab);
				obj.SetActive(false);
				obj.transform.SetParent(this.gameObject.transform);
				obj_pool.Enqueue(obj);
			}

			poolDictionary.Add(pool.tag, obj_pool);

		}
	}
	#endregion

	// Use this for initialization
	void Start () {}
	
	public GameObject SpawnFrom_Pool(string tag, Vector3 pos, Quaternion rot)
	{
		/*if(!poolDictionary.ContainsKey(tag))
		{
			return null;
		}*/

		GameObject objToSpwan = poolDictionary[tag].Dequeue();

		objToSpwan.SetActive(true);
		objToSpwan.transform.position = pos;
		objToSpwan.transform.rotation = rot;

		//poolDictionary[tag].Enqueue(objToSpwan);
		
		return objToSpwan;
	}

	public void AddBackToDisctionary(GameObject o, string tag1)
	{
		poolDictionary[tag1].Enqueue(o);
	}
}
