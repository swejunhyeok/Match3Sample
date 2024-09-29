using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public class ObjectPool<T> where T : Match3Data
        {
            private Transform _parent;
            private Queue<T> _objectPoolQ = new Queue<T>();
            public Queue<T> ObjectPoolQ => _objectPoolQ;

            private GameObject _prefab;
            private string _name = "ObjectPool";

            private List<T> _runningObject = new List<T>();
            public List<T> RunningObject => _runningObject;

            public void Init(GameObject prefab, Transform parent)
            {
                _prefab = prefab;
                _name = _prefab.name + _name;
                _parent = new GameObject(_name).transform;
                _parent.parent = parent;
            }

            private T CreateObject() => GameObject.Instantiate(_prefab, _parent).GetComponent<T>();

            public T GetObject()
            {
                T obj = null;
                if (_objectPoolQ.Count > 0)
                {
                    obj = _objectPoolQ.Dequeue();

                    if (obj == null)
                    {
                        return GetObject();
                    }
                }
                else
                {
                    obj = CreateObject();

                    if (obj == null)
                    {
                        return GetObject();
                    }
                }

                obj.Init();

                _runningObject.Add(obj);
                return obj;
            }

            public void Dispose(T gameObject)
            {
                if (gameObject == null)
                {
                    return;
                }
                if (_objectPoolQ.Contains(gameObject))
                {
                    return;
                }
                _runningObject.Remove(gameObject);
                gameObject.gameObject.SetActive(false);
                gameObject.transform.parent = _parent;
                _objectPoolQ.Enqueue(gameObject);
            }

        }
    }
}
