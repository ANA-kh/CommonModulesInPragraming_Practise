using System.Collections.Generic;
using System.Linq;
using Singleton;
using UnityEngine;

namespace BehaviorTree.Test
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public NPC CircleMan { get; private set; }
        private List<GameObject> _waypoints = new List<GameObject>();
        private List<GameObject> _items = new List<GameObject>();
        protected override void OnInit()
        {
            _waypoints = GameObject.FindGameObjectsWithTag("Waypoint").ToList();
            _items = GameObject.FindGameObjectsWithTag("Item").ToList();

            CircleMan = FindObjectOfType<NPC>();
        }

        protected override void OnCleanup()
        {
            
        }
        
        public GameObject GetNextWayPoint()
        {
            if (_waypoints != null && _waypoints.Count > 0)
            {
                int i = Random.Range(0, _waypoints.Count());
                GameObject result = _waypoints[i];
                _waypoints.RemoveAt(i);
                return result;
            }
            
            return null;
        }
        
        public GameObject GetClosestItem()
        {
            return _items.OrderBy(x => Vector3.Distance(x.transform.position, CircleMan.transform.position)).FirstOrDefault();
        }
        
        public void PickupItem(GameObject item)
        {
            _items.Remove(item);

            Destroy(item);
        }
        
    }
}