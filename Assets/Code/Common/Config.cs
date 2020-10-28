using System;

public class Config {
    public static Int32 seed;

    public Config() {
        seed = DateTime.Now.Day;
    }
}