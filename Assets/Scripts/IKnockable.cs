using UnityEngine;

namespace Assets.Scripts
{
    internal interface IKnockable
    {
        public void ApplyKnockBack(Vector3 locationAfterKnockBack, float timeToArriveAtLocation);
    }
}
