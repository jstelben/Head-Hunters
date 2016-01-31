using UnityEngine;
using System.Collections;

public static class GlobalsScript{
	public static int HeadCount = 0;
	public static float CurrentTime = 90;

	public static float GetCurrentTime(this MonoBehaviour timer) {
		return CurrentTime;
	}

	public static void SetCurrentTime(this MonoBehaviour timer, float currentTime) {
		CurrentTime = currentTime;
	}

	public static int GetHeadCount(this MonoBehaviour player) {
		return HeadCount;
	}

	public static void SetHeadCount(this MonoBehaviour player, int heads) {
		HeadCount = heads;
	}
}
