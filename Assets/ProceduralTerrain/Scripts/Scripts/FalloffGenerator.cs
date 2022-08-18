using UnityEngine;
using System.Collections;

public static class FalloffGenerator {

	public static float[,] GenerateFalloffMap(int size) {
		float[,] map = new float[size,size];

		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				float x = i / (float)size * 2 - 1; //Multiplying by 2 and subtracting 1 so values can go from -1 to 1
				float y = j / (float)size * 2 - 1;

				float value = Mathf.Max (Mathf.Abs (x), Mathf.Abs (y));
				map [i, j] = Evaluate(value);
			}
		}

		return map;
	}

	//Evaluates the value like the animation curve would
	static float Evaluate(float value) {
		float a = 3;
		float b = 2.2f;

		//f(x) = x^2 / x^2 + (b-b*x)^2 = A nice looking curve for generating a falloff map

		return Mathf.Pow (value, a) / (Mathf.Pow (value, a) + Mathf.Pow (b - b * value, a));
	}
}
