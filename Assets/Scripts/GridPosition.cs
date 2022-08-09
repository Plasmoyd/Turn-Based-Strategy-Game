using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GridPosition
{
    private int x;
    private int z;

    public GridPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override string ToString()
    {
        return $" x : {x}, z : {z}";
    }
}
