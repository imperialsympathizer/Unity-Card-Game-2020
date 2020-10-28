using System;

public class RandomNumberGenerator : BaseController {
    public static RandomNumberGenerator Instance;

    private static Random rng;
    // tracks the total number of times Random is used to maintain the state of the Random with saves/states
    private static int sampleCount;

    protected override bool Initialize(bool reinitialize) {
        Instance = this;
        rng = ResourceController.rand;
        return true;
    }

    public static Random GetRng() {
        return rng;
    }

    public static int GetSampleCount() {
        return sampleCount;
    }

    public int GetRandomIntFromRange(int maxValue) {
        if (maxValue >= 0) {
            if (maxValue == 0) {
                return 0;
            }
            else {
                sampleCount++;
                return rng.Next(0, maxValue);
            }
        }
        else {
            return -1;
        }
    }

    public float GetRandomFloat() {
        sampleCount++;
        return (float)rng.NextDouble();
    }
}