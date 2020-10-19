public static class RandomNumberGenerator {
    public static int getRandomIndexFromRange(int maxValue) {
        if (maxValue >= 0) {
            if (maxValue == 0) {
                return 0;
            }
            else {
                return UnityEngine.Random.Range(0, maxValue);
            }
        }
        else {
            return -1;
        }
    }
}