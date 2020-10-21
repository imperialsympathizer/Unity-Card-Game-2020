public abstract class BaseController {
    public bool Initialized { get; private set; } = false;

    public bool InitializeController() {
        if (!Initialized) {
            Initialized = Initialize();
            return Initialized;
        }

        return true;
    }

    public bool ReinitializeController() {
        Initialized = false;
        return InitializeController();
    }

    protected abstract bool Initialize();
}
