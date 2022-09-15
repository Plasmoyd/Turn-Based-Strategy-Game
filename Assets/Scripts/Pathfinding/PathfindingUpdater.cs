using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    private void Start()
    {
        DestructibleCrate.OnAnyCrateDestroyed += DestructibleCrate_OnAnyCrateDestroyed;
    }

    private void DestructibleCrate_OnAnyCrateDestroyed(object sender, EventArgs e)
    {
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;
        Pathfinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(), true);
    }
}
