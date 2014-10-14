// Type: UnityEngine.Vector3
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral
// Assembly location: D:\Unity\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine {
    public struct Vector3 {
        public const float kEpsilon = 1E-05;
        public float x;
        public float y;
        public float z;
        public Vector3(float x, float y, float z);
        public Vector3(float x, float y);
        public float this[int index] { get; set; }
        public Vector3 normalized { get; }
        public float magnitude { get; }
        public float sqrMagnitude { get; }
        public static Vector3 zero { get; }
        public static Vector3 one { get; }
        public static Vector3 forward { get; }
        public static Vector3 back { get; }
        public static Vector3 up { get; }
        public static Vector3 down { get; }
        public static Vector3 left { get; }
        public static Vector3 right { get; }

        [Obsolete("Use Vector3.forward instead.")]
        public static Vector3 fwd { get; }

        public static Vector3 operator +(Vector3 a, Vector3 b);
        public static Vector3 operator -(Vector3 a, Vector3 b);
        public static Vector3 operator -(Vector3 a);
        public static Vector3 operator *(Vector3 a, float d);
        public static Vector3 operator *(float d, Vector3 a);
        public static Vector3 operator /(Vector3 a, float d);
        public static bool operator ==(Vector3 lhs, Vector3 rhs);
        public static bool operator !=(Vector3 lhs, Vector3 rhs);
        public static Vector3 Lerp(Vector3 from, Vector3 to, float t);
        public static Vector3 Slerp(Vector3 from, Vector3 to, float t);
        public static void OrthoNormalize(ref Vector3 normal, ref Vector3 tangent);
        public static void OrthoNormalize(ref Vector3 normal, ref Vector3 tangent, ref Vector3 binormal);
        public static Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta);

        public static Vector3 RotateTowards(Vector3 current, Vector3 target, float maxRadiansDelta,
                                            float maxMagnitudeDelta);

        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime,
                                         float maxSpeed);

        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime);

        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime,
                                         float maxSpeed, float deltaTime);

        public void Set(float new_x, float new_y, float new_z);
        public static Vector3 Scale(Vector3 a, Vector3 b);
        public void Scale(Vector3 scale);
        public static Vector3 Cross(Vector3 lhs, Vector3 rhs);
        public override int GetHashCode();
        public override bool Equals(object other);
        public static Vector3 Reflect(Vector3 inDirection, Vector3 inNormal);
        public static Vector3 Normalize(Vector3 value);
        public void Normalize();
        public override string ToString();
        public string ToString(string format);
        public static float Dot(Vector3 lhs, Vector3 rhs);
        public static Vector3 Project(Vector3 vector, Vector3 onNormal);
        public static Vector3 Exclude(Vector3 excludeThis, Vector3 fromThat);
        public static float Angle(Vector3 from, Vector3 to);
        public static float Distance(Vector3 a, Vector3 b);
        public static Vector3 ClampMagnitude(Vector3 vector, float maxLength);
        public static float Magnitude(Vector3 a);
        public static float SqrMagnitude(Vector3 a);
        public static Vector3 Min(Vector3 lhs, Vector3 rhs);
        public static Vector3 Max(Vector3 lhs, Vector3 rhs);

        [Obsolete(
            "Use Vector3.Angle instead. AngleBetween uses radians instead of degrees and was deprecated for this reason"
            )]
        public static float AngleBetween(Vector3 from, Vector3 to);
    }
}
