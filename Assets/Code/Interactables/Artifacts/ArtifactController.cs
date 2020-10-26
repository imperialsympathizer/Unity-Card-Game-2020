using System;
using System.Collections.Generic;

public class ArtifactController : BaseController {
    public static ArtifactController Instance;

    // The library of all artifacts
    private ArtifactSource artifactSource;

    // list of artifacts acquired during a run
    private List<Artifact> runArtifacts;

    protected override bool Initialize(bool reinitialize) {
        Instance = this;
        if (ElementController.Instance != null && ElementController.Instance.Initialized &&
            PlayerController.Instance != null && PlayerController.Instance.Initialized) {
            artifactSource = new ArtifactSource();
            runArtifacts = new List<Artifact>();
            if (!reinitialize) {
                // Create starting artifacts
                AddArtifact("Aegis");
            }
            else {
                foreach (Artifact artifact in ResourceController.runArtifacts) {
                    AddArtifact(artifact.name);
                }
            }

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

    internal List<Artifact> GetRunArtifacts() {
        return runArtifacts;
    }
}
