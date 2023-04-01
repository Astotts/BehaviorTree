using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    //https://youtu.be/aR6wt5BlE-E
    public abstract class BTree : MonoBehaviour
    {

        private Node _root = null;

        protected void Start()
        {
            _root = SetupTree();
        }

        private void Update()
        {
            if (_root != null){
                _root.Evaluate();
            }
        }

        protected abstract Node SetupTree();

    }

}