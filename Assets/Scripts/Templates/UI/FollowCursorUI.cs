using UnityEngine;

namespace Template.UI
{
    public class FollowCursor2DUI : MonoBehaviour
    {
        public virtual void Initialize()
        {
            //this.LateUpdateAsObservable().Subscribe(_ => FollowCursor()).AddTo(this);
        }

        private void FollowCursor()
        {
            this.transform.position = Input.mousePosition;
        }
    }
}
