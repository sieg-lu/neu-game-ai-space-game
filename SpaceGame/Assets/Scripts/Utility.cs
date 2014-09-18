using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Utility {
	public static bool Vector3CompareXZ(Vector3 a, Vector3 b, float eps) {
//		return (va == vb);
		return (Math.Abs(a.x - b.x) < eps && Math.Abs(a.z - b.z) < eps);
	}
	
	public static bool Vector3CompareXZ(Vector3 a, Vector3 b) {
		return Vector3CompareXZ(a, b, 0.01f);
	}
}

