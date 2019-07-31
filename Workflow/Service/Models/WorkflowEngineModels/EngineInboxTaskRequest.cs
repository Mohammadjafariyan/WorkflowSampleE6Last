using System;

namespace WorkflowSample.Service.Models.WorkflowEngineModels
{
    public class EngineInboxTaskRequest
    {
        public EngineInboxTaskRequest()
        {
            includeProcessVariables = true;
        }

        public string name { get; set; }
        public string nameLike { get; set; }
        public string description { get; set; }
        public string priority { get; set; }
        public int? minimumPriority { get; set; }
        public int? maximumPriority { get; set; }
        public string assignee { get; set; }
        public string assigneeLike { get; set; }
        public string owner { get; set; }
        public string ownerLike { get; set; }
        public bool? unassigned { get; set; }
        public string delegationState { get; set; }
        public string candidateUser { get; set; }
        public string candidateGroup { get; set; }
        public string candidateGroups { get; set; }
        public string involvedUser { get; set; }
        public string taskDefinitionKeyLike { get; set; }
        public long processInstanceId { get; set; }
        public string processInstanceBusinessKey { get; set; }
        public string processInstanceBusinessKeyLike { get; set; }
        public string processDefinitionId { get; set; }
        public string processDefinitionKey { get; set; }
        public string processDefinitionKeyLike { get; set; }
        public string processDefinitionName { get; set; }
        public string processDefinitionNameLike { get; set; }
        public string executionId { get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? createdBefore { get; set; }
        public DateTime? dueOn { get; set; }
        public DateTime? dueBefore { get; set; }
        public DateTime? dueAfter { get; set; }
        public bool? withrefDueDate { get; set; }
        public bool? excludeSubTasks { get; set; }
        public bool? active { get; set; }
        public bool? includeTaskLocalVariables { get; set; }
        public bool? includeProcessVariables { get; set; }
        public string tenantId { get; set; }
        public string tenantIdLike { get; set; }

        public string withrefTenantId { get; set; }
        public string candidateOrAssigned { get; set; }
        public string category { get; set; }

        /// <summary>
        /// pending , resolved
        /// </summary>
        public string taskDefinitionKey { get; set; }

        public string Username { get; set; }


        public string ToUrl()
        {
            string url = "";
            url = M(ref url, nameof(assignee), assignee);
            url = M(ref url, nameof(includeProcessVariables), includeProcessVariables);
            url = M(ref url, nameof(includeTaskLocalVariables), includeTaskLocalVariables);
            url = M(ref url, nameof(name), name);
            url = M(ref url, nameof(nameLike), nameLike);
            url = M(ref url, nameof(description), description);
            url = M(ref url, nameof(maximumPriority), maximumPriority);
            url = M(ref url, nameof(assigneeLike), assigneeLike);
            url = M(ref url, nameof(owner), owner);
            url = M(ref url, nameof(unassigned), unassigned);
            url = M(ref url, nameof(ownerLike), ownerLike);
            url = M(ref url, nameof(minimumPriority), minimumPriority);
            url = M(ref url, nameof(processInstanceBusinessKeyLike), processInstanceBusinessKeyLike);
            url = M(ref url, nameof(delegationState), delegationState);
            url = M(ref url, nameof(candidateUser), candidateUser);
            url = M(ref url, nameof(candidateGroup), candidateGroup);
            url = M(ref url, nameof(involvedUser), involvedUser);
            url = M(ref url, nameof(taskDefinitionKeyLike), taskDefinitionKeyLike);
            url = M(ref url, nameof(processInstanceId), processInstanceId==0 ? "" : processInstanceId.ToString());
            url = M(ref url, nameof(processInstanceBusinessKey), processInstanceBusinessKey);
            url = M(ref url, nameof(processDefinitionId), processDefinitionId);
            url = M(ref url, nameof(processDefinitionKey), processDefinitionKey);
            url = M(ref url, nameof(processInstanceBusinessKey), processInstanceBusinessKey);
            url = M(ref url, nameof(processDefinitionKeyLike), processDefinitionKeyLike);
            url = M(ref url, nameof(processDefinitionName), processDefinitionName);
            url = M(ref url, nameof(processDefinitionNameLike), processDefinitionNameLike);
            url = M(ref url, nameof(executionId), executionId);
            url = M(ref url, nameof(createdOn), createdOn);
            url = M(ref url, nameof(createdBefore), createdBefore);
            url = M(ref url, nameof(dueOn), dueOn);
            url = M(ref url, nameof(dueBefore), dueBefore);
            url = M(ref url, nameof(dueAfter), dueAfter);
            url = M(ref url, nameof(withrefDueDate), withrefDueDate);
            url = M(ref url, nameof(excludeSubTasks), excludeSubTasks);
            url = M(ref url, nameof(tenantId), tenantId);
            url = M(ref url, nameof(tenantIdLike), tenantIdLike);
            url = M(ref url, nameof(withrefTenantId), withrefTenantId);
            url = M(ref url, nameof(candidateOrAssigned), candidateOrAssigned);
            url = M(ref url, nameof(candidateOrAssigned), candidateOrAssigned);
            url = M(ref url, nameof(category), category);
            url = M(ref url, nameof(taskDefinitionKey), taskDefinitionKey);

            return  url;
        }


        public string M(ref string url, string name, object value)
        {
            
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return url;
            
            if (url.IndexOf("?")!=-1)
            {
                url += $@"&{name}={value.ToString().ToLower()}";
            }
            else
            {
                url += $@"?{name}={value.ToString().ToLower()}";
            }

            return url;
        }
    }
}