using System;

[Serializable]
public class ChangeEnemyAttacksUntilEot : ArtifactPassive {
    public ChangeEnemyAttacksUntilEot(int effectCount) : base(effectCount, 0) { }

    protected override void OperateOnEffect(Trigger trigger) { }

    protected override void ResolveEffect(Trigger trigger) {
        if (trigger != null && trigger is OnArtifactActivate artifactActivate && artifactActivate.artifactId == artifactId) {
            foreach (Enemy enemy in EnemyController.Instance.GetEnemyList()) {
                // The Dynamic effect immediately changes the value and the static effect resets the value at EOT
                DynamicEffectController.Instance.AddEffect(new ChangeFighterAttackValue(effectCount, enemy));
                StaticEffectController.Instance.AddModifier(enemy, new ChangeFighterAttackEot(-effectCount));
            }
        }
    }
}