using System.Collections.Generic;

public class ArtifactSource {
    public Dictionary<string, Artifact> allArtifacts;

    public ArtifactSource() {
        InitializeCards();
    }

    public void InitializeCards() {
        allArtifacts = new Dictionary<string, Artifact>();

        List<Artifact> artifactList = JsonUtil.LoadArtifactsFromJson();
        foreach (Artifact artifact in artifactList) {
            allArtifacts.Add(artifact.name, artifact);
        }
    }
}