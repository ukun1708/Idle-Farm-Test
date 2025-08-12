using UnityEngine;

public interface ITargetable
{
    public void TargetEnter(Transform user);

    public void TargetLeave();
}
