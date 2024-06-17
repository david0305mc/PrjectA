using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEST
{
    public class ObjRotateTestManager : MonoBehaviour
    {
        [SerializeField] TestRotateObj rotateObj;
        [SerializeField] GameObject targetObj;

        private void FixedUpdate()
        {
            //rotateObj.transform.LookAt(targetObj.transform);
            //rotateObj.transform.LookAt2DLerp(targetObj.transform.position, 1f);
            rotateObj.transform.LookAt2D(targetObj.transform.position);
        }
    }
    public static class LookAtExtension
    {
        public static void LookAt2DLerp(this Transform transform, Vector2 dir, float lerpPercent = 0.05f)
        {
            float rotationZ = Mathf.Acos(dir.x / dir.magnitude)
                * 180 / Mathf.PI
                * Mathf.Sign(dir.y);

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.Euler(0, 0, rotationZ),
                lerpPercent
            );
        }

        public static void LookAt2D(this Transform transform, Vector2 dir)
        {
            Vector2 newPos = dir - (Vector2)transform.position;
            float rotZ = Mathf.Atan2(newPos.y, newPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);

            //float rotationZ = Mathf.Acos(dir.x / dir.magnitude)
            //        * 180 / Mathf.PI
            //        * Mathf.Sign(dir.y);
            //transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        }
    }
}

