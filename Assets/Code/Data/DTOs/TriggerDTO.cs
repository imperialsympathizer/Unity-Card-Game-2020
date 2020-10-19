using System;
using System.Collections.Generic;

[Serializable]
public class TriggerDTO : DTO {
    // The trigger class to instantiate
    public string triggerType;
    // The list of actions to take on the given trigger
    public List<Trigger.TriggerAction> triggerActions;
}
