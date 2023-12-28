using System;
using UnityEngine;

public class HanoiTowerPole : MonoBehaviour
{
    public enum Kind { Left, Middle, Right }
    public Kind kind;

    public bool isDiscAroundMe(HanoiTowerDisc disc)
    {
        Transform westCollider = disc.transform.Find("Box Colliders").Find("West Collider");
        Transform northCollider = disc.transform.Find("Box Colliders").Find("North Collider");
        Transform eastCollider = disc.transform.Find("Box Colliders").Find("East Collider");
        Transform southCollider = disc.transform.Find("Box Colliders").Find("South Collider");
        if (westCollider.position.z > transform.position.z
        && eastCollider.position.z < transform.position.z
        && northCollider.position.x > transform.position.x
        && southCollider.position.x < transform.position.x)
            return true;
        return false;
    }

    public HanoiTowerDisc getHighestDisc(HanoiTowerDisc otherThan = null)
    {
        HanoiTowerDisc highestDisc = null;
        foreach (HanoiTowerDisc disc in HanoiTowerManager.current.discs)
        {
            if (!isDiscAroundMe(disc) || (otherThan != null && ReferenceEquals(otherThan, disc)))
                continue;
            if (highestDisc == null)
                highestDisc = disc;
            if (highestDisc.transform.position.y < disc.transform.position.y)
                highestDisc = disc;
        }
        return highestDisc;
    }
}
