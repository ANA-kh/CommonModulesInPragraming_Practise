using System;
using UnityEngine;

namespace BehaviorTree.Test
{
    public class Item : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (GameManager.Instance.CircleMan.MyActivity == NavigationActivity.PickupItem)
            {
                GameManager.Instance.PickupItem(this.gameObject);
            }
        }
    }
}