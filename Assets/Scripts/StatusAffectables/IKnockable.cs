using UnityEngine;

namespace Assets.Scripts.StatusAffectables
{
    internal interface IKnockable
    {
        public void ApplyKnockBack(Vector3 locationAfterKnockBack, float timeToArriveAtLocation);
    }
}
