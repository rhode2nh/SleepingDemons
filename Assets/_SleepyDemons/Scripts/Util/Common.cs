public static class Common
{
	public static float NormalizeAngle(float angle)
	{
		angle %= 360f;
		if (angle > 180f) angle -= 360f;
		return angle;
	}
}
