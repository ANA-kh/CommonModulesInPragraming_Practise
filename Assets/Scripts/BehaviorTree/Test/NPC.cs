using System;
using System.Collections;
using BehaviorTree.Action;
using BehaviorTree.Composite;
using BehaviorTree.Condition;
using BehaviorTree.Decorator;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree.Test
{
    public enum NavigationActivity
    {
        Waypoint, 
        PickupItem
    }

    public class NPC : MonoBehaviour
    {
        public float speed = 50;
        private Vector3 _targetPos;
        private Rigidbody2D _rigidbody2D;
        public NavigationActivity MyActivity { get; set; }
        
        private Coroutine m_BehaviorTreeRoutine;
        private YieldInstruction _waitTime = new WaitForSeconds(.1f);


        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            MyActivity = NavigationActivity.Waypoint;
            _targetPos = transform.position;

            GenerateBehaviorTree();
    
            if (m_BehaviorTreeRoutine == null && BehaviorTree != null)
            {
                m_BehaviorTreeRoutine = StartCoroutine(RunBehaviorTree());
            }
        }

        private void FixedUpdate()
        {
            if (Vector3.Distance(_targetPos, transform.position) > 0.1f)
            {
                _rigidbody2D.velocity = (_targetPos - transform.position).normalized * (speed * Time.fixedDeltaTime);
            }
            else
            {
                _rigidbody2D.velocity = Vector2.zero;
            }
        }

        public NodeBase BehaviorTree { get; set; }

        public void SetDestination(Vector3 pos)
        {
            _targetPos = pos;
        }
        private void GenerateBehaviorTree()
        {
            BehaviorTree = 
                new Selector("Control NPC",
                    new Sequence("Pickup Item",
                        new IsNavigationActivityTypeOf(NavigationActivity.PickupItem),
                        new Selector("Look for or move to items",
                            new Sequence("Look for items",
                                new Inverter("Inverter",
                                    new AreItemsNearBy(4f)),
                                new SetNavigationActivityTo(NavigationActivity.Waypoint)),
                            new Sequence("Navigate to Item",
                                new NavigateToDestination()))),
                    new Sequence("Move to Waypoint",
                        new IsNavigationActivityTypeOf(NavigationActivity.Waypoint),
                        new NavigateToDestination(),
                        new Timer(2f,
                            new SetNavigationActivityTo(NavigationActivity.PickupItem))));
        }
        
        private IEnumerator RunBehaviorTree()
        {
            while (enabled)
            {
                if (BehaviorTree == null)
                {
                    Debug.LogError($"{this.GetType().Name} is missing Behavior Tree. Did you set the BehaviorTree property?");
                    continue;
                }

                (BehaviorTree as NodeBase).Run();

                yield return _waitTime;
            }
        }

        private void OnDestroy()
        {
            if (m_BehaviorTreeRoutine != null)
            {
                StopCoroutine(m_BehaviorTreeRoutine);
            }
        }
                
    }
}