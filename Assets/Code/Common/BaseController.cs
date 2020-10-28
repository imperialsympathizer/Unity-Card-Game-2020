public abstract class BaseController {
    public bool Initialized { get; set; } = false;

    public bool InitializeController(bool reinitialize = false) {
        if (!Initialized) {
            Initialized = Initialize(reinitialize);
            return Initialized;
        }

        return true;
    }

    protected abstract bool Initialize(bool reinitialize);
}
