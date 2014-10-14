// Type: UnityEngine.Physics
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral
// Assembly location: D:\Unity\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine {
    public class Physics {
        public const int kIgnoreRaycastLayer = 4;
        public const int kDefaultRaycastLayers = -5;
        public const int kAllLayers = -1;
        public static Vector3 gravity { get; set; }
        public static float minPenetrationForPenalty { get; set; }
        public static float bounceThreshold { get; set; }

        [Obsolete("Please use bounceThreshold instead.")]
        public static float bounceTreshold { get; set; }

        public static float sleepVelocity { get; set; }
        public static float sleepAngularVelocity { get; set; }
        public static float maxAngularVelocity { get; set; }
        public static int solverIterationCount { get; set; }
        public static float penetrationPenaltyForce { get; set; }
        public static bool Raycast(Vector3 origin, Vector3 direction, float distance);
        public static bool Raycast(Vector3 origin, Vector3 direction);
        public static bool Raycast(Vector3 origin, Vector3 direction, float distance, int layerMask);
        public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float distance);
        public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo);

        public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float distance,
                                   int layerMask);

        public static bool Raycast(Ray ray, float distance);
        public static bool Raycast(Ray ray);
        public static bool Raycast(Ray ray, float distance, int layerMask);
        public static bool Raycast(Ray ray, out RaycastHit hitInfo, float distance);
        public static bool Raycast(Ray ray, out RaycastHit hitInfo);
        public static bool Raycast(Ray ray, out RaycastHit hitInfo, float distance, int layerMask);
        public static RaycastHit[] RaycastAll(Ray ray, float distance);
        public static RaycastHit[] RaycastAll(Ray ray);
        public static RaycastHit[] RaycastAll(Ray ray, float distance, int layerMask);
        public static RaycastHit[] RaycastAll(Vector3 origin, Vector3 direction, float distance, int layermask);
        public static RaycastHit[] RaycastAll(Vector3 origin, Vector3 direction, float distance);
        public static RaycastHit[] RaycastAll(Vector3 origin, Vector3 direction);
        public static bool Linecast(Vector3 start, Vector3 end);
        public static bool Linecast(Vector3 start, Vector3 end, int layerMask);
        public static bool Linecast(Vector3 start, Vector3 end, out RaycastHit hitInfo);
        public static bool Linecast(Vector3 start, Vector3 end, out RaycastHit hitInfo, int layerMask);
        public static Collider[] OverlapSphere(Vector3 position, float radius, int layerMask);
        public static Collider[] OverlapSphere(Vector3 position, float radius);
        public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float distance);
        public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction);

        public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float distance,
                                       int layerMask);

        public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction,
                                       out RaycastHit hitInfo, float distance);

        public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction,
                                       out RaycastHit hitInfo);

        public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction,
                                       out RaycastHit hitInfo, float distance, int layerMask);

        public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo,
                                      float distance);

        public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo);

        public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo,
                                      float distance, int layerMask);

        public static bool SphereCast(Ray ray, float radius, float distance);
        public static bool SphereCast(Ray ray, float radius);
        public static bool SphereCast(Ray ray, float radius, float distance, int layerMask);
        public static bool SphereCast(Ray ray, float radius, out RaycastHit hitInfo, float distance);
        public static bool SphereCast(Ray ray, float radius, out RaycastHit hitInfo);
        public static bool SphereCast(Ray ray, float radius, out RaycastHit hitInfo, float distance, int layerMask);

        public static RaycastHit[] CapsuleCastAll(Vector3 point1, Vector3 point2, float radius, Vector3 direction,
                                                  float distance, int layermask);

        public static RaycastHit[] CapsuleCastAll(Vector3 point1, Vector3 point2, float radius, Vector3 direction,
                                                  float distance);

        public static RaycastHit[] CapsuleCastAll(Vector3 point1, Vector3 point2, float radius, Vector3 direction);
        public static RaycastHit[] SphereCastAll(Vector3 origin, float radius, Vector3 direction, float distance);
        public static RaycastHit[] SphereCastAll(Vector3 origin, float radius, Vector3 direction);

        public static RaycastHit[] SphereCastAll(Vector3 origin, float radius, Vector3 direction, float distance,
                                                 int layerMask);

        public static RaycastHit[] SphereCastAll(Ray ray, float radius, float distance);
        public static RaycastHit[] SphereCastAll(Ray ray, float radius);
        public static RaycastHit[] SphereCastAll(Ray ray, float radius, float distance, int layerMask);
        public static bool CheckSphere(Vector3 position, float radius, int layerMask);
        public static bool CheckSphere(Vector3 position, float radius);
        public static bool CheckCapsule(Vector3 start, Vector3 end, float radius, int layermask);
        public static bool CheckCapsule(Vector3 start, Vector3 end, float radius);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static void IgnoreCollision(Collider collider1, Collider collider2, bool ignore);

        public static void IgnoreCollision(Collider collider1, Collider collider2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static void IgnoreLayerCollision(int layer1, int layer2, bool ignore);

        public static void IgnoreLayerCollision(int layer1, int layer2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static bool GetIgnoreLayerCollision(int layer1, int layer2);
    }
}
