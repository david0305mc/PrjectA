using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEST
{
    public class TileObj : MonoBehaviour
    {

        [SerializeField] private SpriteRenderer spriteRenderer;
        public ObjectMoveTest.TileStatus status { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        private System.Action<TileObj> action;

        public void InitData(System.Action<TileObj> _action)
        {
            action = _action;
            SetData(ObjectMoveTest.TileStatus.Normal);
        }

        public void SetData(ObjectMoveTest.TileStatus _status)
        {
            status = _status;
            switch (status)
            {
                case ObjectMoveTest.TileStatus.Normal:
                    spriteRenderer.color = Color.black;
                    break;
                case ObjectMoveTest.TileStatus.Start:
                    spriteRenderer.color = Color.yellow;
                    break;
                case ObjectMoveTest.TileStatus.Block:
                    spriteRenderer.color = Color.green;
                    break;
                case ObjectMoveTest.TileStatus.End:
                    spriteRenderer.color = Color.red;
                    break;
                case ObjectMoveTest.TileStatus.Path:
                    spriteRenderer.color = Color.blue;
                    break;
            }
        }

        private void OnMouseUp()
        {
            action?.Invoke(this);
            Debug.Log(gameObject.name);
        }
    }

}
