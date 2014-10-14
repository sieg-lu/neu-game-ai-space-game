// Type: UnityEngine.Transform
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral
// Assembly location: D:\Unity\Editor\Data\Managed\UnityEngine.dll

using System.Collections;
using System.Runtime.CompilerServices;

namespace UnityEngine {
    public sealed class Transform : Component, IEnumerable {
        public Vector3 position { get; set; }
        public Vector3 localPosition { get; set; }
        public Vector3 eulerAngles { get; set; }
        public Vector3 localEulerAngles { get; set; }
        public Vector3 right { get; set; }
        public Vector3 up { get; set; }
        public Vector3 forward { get; set; }
        public Quaternion rotation { get; set; }
        public Quaternion localRotation { get; set; }
        public Vector3 localScale { get; set; }
        public Transform parent { get; set; }
        public Matrix4x4 worldToLocalMatrix { get; }
        public Matrix4x4 localToWorldMatrix { get; }
        public Transform root { get; }
        public int childCount { get; }
        public Vector3 lossyScale { get; }
        public bool hasChanged { get; set; }

        #region IEnumerable Members

        public IEnumerator GetEnumerator();

        #endregion

        public void Translate(Vector3 translation);
        public void Translate(Vector3 translation, Space relativeTo);
        public void Translate(float x, float y, float z);
        public void Translate(float x, float y, float z, Space relativeTo);
        public void Translate(Vector3 translation, Transform relativeTo);
        public void Translate(float x, float y, float z, Transform relativeTo);
        public void Rotate(Vector3 eulerAngles);
        public void Rotate(Vector3 eulerAngles, Space relativeTo);
        public void Rotate(float xAngle, float yAngle, float zAngle);
        public void Rotate(float xAngle, float yAngle, float zAngle, Space relativeTo);
        public void Rotate(Vector3 axis, float angle);
        public void Rotate(Vector3 axis, float angle, Space relativeTo);
        public void RotateAround(Vector3 point, Vector3 axis, float angle);
        public void LookAt(Transform target);
        public void LookAt(Transform target, Vector3 worldUp);
        public void LookAt(Vector3 worldPosition, Vector3 worldUp);
        public void LookAt(Vector3 worldPosition);
        public Vector3 TransformDirection(Vector3 direction);
        public Vector3 TransformDirection(float x, float y, float z);
        public Vector3 InverseTransformDirection(Vector3 direction);
        public Vector3 InverseTransformDirection(float x, float y, float z);
        public Vector3 TransformPoint(Vector3 position);
        public Vector3 TransformPoint(float x, float y, float z);
        public Vector3 InverseTransformPoint(Vector3 position);
        public Vector3 InverseTransformPoint(float x, float y, float z);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public void DetachChildren();

        [MethodImpl(MethodImplOptions.InternalCall)]
        public Transform Find(string name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public bool IsChildOf(Transform parent);

        public Transform FindChild(string name);
        public void RotateAround(Vector3 axis, float angle);
        public void RotateAroundLocal(Vector3 axis, float angle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public Transform GetChild(int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public int GetChildCount();
    }
}
