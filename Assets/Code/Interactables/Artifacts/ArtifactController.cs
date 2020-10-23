using System.Collections.Generic;

public class ArtifactController : BaseController {
    public static ArtifactController Instance;

    // The library of all artifacts
    private ArtifactSource artifactSource;

    // list of artifacts acquired during a run
    private List<Artifact> runArtifacts;

    protected override bool Initialize() {
        Instance = this;
        if (ElementController.Instance != null && ElementController.Instance.Initialized &&
            PlayerController.Instance != null && PlayerController.Instance.Initialized) {
            artifactSource = new ArtifactSource();

            // Create starting artifacts
            runArtifacts = new List<Artifact>();
            AddArtifact("Aegis");

            return true;
        }

        return false;
    }

    public void AddArtifact(string name) {
        if (artifactSource.allArtifacts.TryGetValue(name, out Artifact sourceArtifact)) {
            Artifact newArtifact = new Artifact(sourceArtifact);
            newArtifact.CreateVisual();
            runArtifacts.Add(newArtifact);
        }
    }
}
