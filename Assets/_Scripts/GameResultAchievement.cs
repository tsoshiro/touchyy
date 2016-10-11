using UnityEngine;
using System.Collections;

public class GameResultAchievement {
	public string achievementId;
	public int targetCount;

	public GameResultAchievement (string pAchievementId, int pTargetCount) {
		achievementId = pAchievementId;
		targetCount = pTargetCount;
	}

	public float getProgress (int pScore) {
		float progress = (float)pScore / (float)targetCount;
		return progress;
	}
}
