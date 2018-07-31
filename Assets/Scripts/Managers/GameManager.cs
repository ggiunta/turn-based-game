using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

namespace SA.TB
{
    public class GameManager : MonoBehaviour
    {
        public Transform currentUnit;
        public bool movingPlayer;
        bool hasPath;

        Node unitNode;
        Node currentNode;
        Node previousNode;

        List<Node> path;

        LineRenderer pathVis;
        GridBase grid;

        public void Init()
        {
            grid = GridBase.singleton;

            Vector3 worldPos = grid.GetWorldCoordinatesFromNode(5, 0, 6);
            currentUnit.transform.position = worldPos;

            GameObject go = new GameObject();
            go.name = "line vis";
            pathVis = go.AddComponent<LineRenderer>();
            pathVis.startWidth = 0.2f;
            pathVis.endWidth = 0.2f;
        }

        void Update()
        {
            if (GridBase.singleton.isInit == false)
                return;

            FindNode();

            if(unitNode == null && currentUnit != null)
            {
                unitNode = grid.GetNodeFromWorldPosition(currentUnit.transform.position);
            }

            if (unitNode == null)
            {
                return;
            }

            if (previousNode == currentNode)
            {
                PathfindMaster.GetInstance().RequestPathfind(unitNode, currentNode, PathfinderCallback);
            }

            previousNode = currentNode;

            if (hasPath && path != null)
            {
                if (path.Count > 0)
                {
                    pathVis.positionCount = path.Count;

                    for (int i = 0; i < path.Count; i++)
                    {
                        Node n = path[i];
                        Vector3 p = grid.GetWorldCoordinatesFromNode(n.x, n.y, n.z);
                        pathVis.SetPosition(i, p);
                    }
                }
            }
        }

        void PathfinderCallback(List<Node> p)
        {
            path = p;
            hasPath = true;
        }

        void FindNode()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //Debug.Log("before if ray");
            if (Physics.Raycast(ray,out hit, Mathf.Infinity))
            {
                currentNode = GridBase.singleton.GetNodeFromWorldPosition(hit.point);
                //Debug.Log("inside if ray");
            }
            //Debug.Log("after if ray");
        }

        public static GameManager singleton;
        private void Awake()
        {
            singleton = this;
        }
    }
}

