using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour {

    public static StatusManager instance;

    public List<StatusEntry> statusEntries = new List<StatusEntry>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void Initialize()
    {

    }

    private void Update()
    {
        for (int i = 0; i < statusEntries.Count; i++)
        {
            statusEntries[i].ManagedUpdate();
        }
    }

    public static void AddStatus(GameObject target, Status status)
    {
        int count = instance.statusEntries.Count;
        StatusEntry targetEntry = null;

        for (int i = 0; i < count; i++)
        {
            if (instance.statusEntries[i].target == target)
            {
                targetEntry = instance.statusEntries[i];
                break;
            }
        }

        if (targetEntry != null)
        {
            targetEntry.AddStatus(status);
            return;
        }


        StatusEntry newStatus = new StatusEntry(target, new StatusContainer(status));
        instance.statusEntries.Add(newStatus);
    }

    public static void RemoveStatus(GameObject target, Status targetStatus)
    {
        int count = instance.statusEntries.Count;
        StatusEntry targetEntry = null;

        for (int i = 0; i < count; i++)
        {
            if (instance.statusEntries[i].target == target)
            {
                targetEntry = instance.statusEntries[i];
                //statusManager.statusEntries.Remove(statusManager.statusEntries[i]);
                break;
            }
        }

        if (targetEntry != null)
        {
            targetEntry.RemoveStatus(targetStatus);
            if (targetEntry.GetStatusCount() < 1)
            {
                instance.statusEntries.Remove(targetEntry);
            }
        }
    }

    public static bool IsTargetAlreadyAffected(GameObject target, Status status)
    {
        int count = instance.statusEntries.Count;
        StatusEntry targetEntry = null;

        for (int i = 0; i < count; i++)
        {
            if (instance.statusEntries[i].target == target)
            {
                targetEntry = instance.statusEntries[i];
                //statusManager.statusEntries.Remove(statusManager.statusEntries[i]);
                break;
            }
        }

        if (targetEntry != null)
        {
            return targetEntry.IsTargetAlreadyAffected(target, status);
        }

        return false;

    }


    [System.Serializable]
    public class StatusEntry {
        public GameObject target;
        private StatusContainer statusContainer;

        public StatusEntry(GameObject target, StatusContainer statusContainer)
        {
            this.target = target;
            this.statusContainer = statusContainer;
        }

        public void ManagedUpdate()
        {
            statusContainer.ManagedUpdate();
        }

        public int GetStatusCount()
        {
            return statusContainer.activeStatusList.Count;
        }

        public bool IsTargetAlreadyAffected(GameObject target, Status status)
        {
            if (this.target != target)
                return false;

            List<Status> existingStatus = statusContainer.GetStatusListByType(status.statusType);
            int count = existingStatus.Count;

            if (count < 1)
                return false;

            for (int i = 0; i < count; i++)
            {
                if (existingStatus[i].IsFromSameSource(status.SourceAbility))
                {
                    return true;
                }
            }

            return false;
        }


        public void AddStatus(Status status)
        {
            List<Status> existingStatus = statusContainer.GetStatusListByType(status.statusType);

            int count = existingStatus.Count;

            if (existingStatus.Count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    if (StackStatus(status, existingStatus[i], status.SourceAbility) == true)
                        return;
                }
            }

            statusContainer.AddStatus(status);
        }

        private bool StackStatus(Status status, Status existingStatus, Ability sourceAbility)
        {
            if (existingStatus.IsFromSameSource(sourceAbility))
            {
                switch (status.stackMethod)
                {
                    case Constants.EffectStackingMethod.None: //TODO: Move this out of the check for same source
                    case Constants.EffectStackingMethod.StacksWithOtherAbilities:
                        existingStatus.RefreshDuration();
                        return true;

                    case Constants.EffectStackingMethod.LimitedStacks:
                        if (existingStatus.StackCount < existingStatus.MaxStack)
                        {
                            existingStatus.Stack();
                        }
                        else
                        {
                            existingStatus.RefreshDuration();
                        }
                        return true;
                }
            }

            return false;
        }

        public void RemoveStatus(Status status)
        {
            statusContainer.RemoveStatus(status);
        }

    }


    [System.Serializable]
    public class StatusContainer {
        public List<Status> activeStatusList = new List<Status>();

        public StatusContainer(Status initialStatus)
        {
            AddStatus(initialStatus);
        }

        public void AddStatus(Status status)
        {
            activeStatusList.Add(status);
        }

        public void RemoveStatus(Status status)
        {
            if (activeStatusList.Contains(status))
            {
                activeStatusList.Remove(status);
            }
        }

        public void ManagedUpdate()
        {
            for (int i = 0; i < activeStatusList.Count; i++)
            {
                activeStatusList[i].ManagedUpdate();
            }
        }

        public List<Status> GetStatusListByType(Constants.StatusType type)
        {
            List<Status> results = new List<Status>();

            int count = activeStatusList.Count;

            for (int i = 0; i < count; i++)
            {
                if (activeStatusList[i].statusType == type)
                {
                    results.Add(activeStatusList[i]);
                }
            }

            return results;
        }


    }

}
