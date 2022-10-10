using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.General;

//https://www.sebaslab.com/zero-allocation-code-in-unity/
namespace _Scripts._Game.General.Managers
{   
    public abstract class PoolBehaviourManager<T> : Singleton<PoolBehaviourManager<T>> where T : Behaviour
    {
        private readonly Stack<T> m_Pool = new Stack<T>();
        private readonly LinkedList<T> m_Inuse = new LinkedList<T>();
        private readonly Stack<LinkedListNode<T>> m_NodePool = new Stack<LinkedListNode<T>>();

        private int m_lastCheckFrame = -1;
        [SerializeField]
        protected int m_PoolCount;

        protected new void Awake()
        {
            base.Awake();
            Debug.Log("PoolBehaviourManager<T> awake check");
            for (int i = 0; i < m_PoolCount; ++i)
            {
                T comp = gameObject.AddComponent(typeof(T)) as T;
                m_Pool.Push(comp);
            }
        }

        private void CheckPools()
        {
            LinkedListNode<T> node = m_Inuse.First;
            while (node != null)
            {
                LinkedListNode<T> current = node;
                node = node.Next;

                if (!IsActive(current.Value))
                {
                    current.Value.gameObject.SetActive(false);
                    m_Pool.Push(current.Value);
                    m_Inuse.Remove(current);
                    m_NodePool.Push(current);
                }
            }
        }

        protected T GetPooledComponent()
        {
            T item;

            // check to update nodes in pool
            if (m_lastCheckFrame != Time.frameCount)
            {
                m_lastCheckFrame = Time.frameCount;
                CheckPools();
            }

            if (m_Pool.Count == 0)
                //item = GameObject.Instantiate(prefab);
                return null;
            else
                item = m_Pool.Pop();

            if (m_NodePool.Count == 0)
                m_Inuse.AddLast(item);
            else
            {
                var node = m_NodePool.Pop();
                node.Value = item;
                m_Inuse.AddLast(node);
            }

            item.gameObject.SetActive(true);

            return item;
        }

        protected abstract bool IsActive(T component);
    }
    
}
