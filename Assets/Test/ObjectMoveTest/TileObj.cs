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
                    SetColor(Color.black);
                    break;
                case ObjectMoveTest.TileStatus.Start:
                    SetColor(Color.yellow);
                    break;
                case ObjectMoveTest.TileStatus.Block:
                    SetColor(Color.green);
                    break;
                case ObjectMoveTest.TileStatus.End:
                    SetColor(Color.red);
                    break;
                case ObjectMoveTest.TileStatus.Path:
                    SetColor(Color.blue);
                    break;
            }
        }
        public void SetColor(Color _color)
        {
            spriteRenderer.color = _color;
        }
        private void OnMouseUp()
        {
            action?.Invoke(this);
            Debug.Log(gameObject.name);
        }
    }

}
