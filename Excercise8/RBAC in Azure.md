### Custom Roles in Azure
#### (based on https://techcommunity.microsoft.com/t5/ITOps-Talk-Blog/Step-By-Step-Enabling-Custom-Role-Based-Access-Control-in-Azure/ba-p/363668)


rolekamil.json:

```
{
        "Name": "Rola dla Kamila",
        "IsCustom": true,
        "Description": "Rola dla Kamila",
        "Actions": [
        "Microsoft.Resources/subscriptions/resourceGroups/read"
        ],
        "NotActions": [
        ],
        "DataActions": [
        ],
        "NotDataActions": [
        ],
        "AssignableScopes": [
               "/subscriptions/<SUB ID>"
        ]
}
```

then

```
az role definition create --role-definition rolekamil.json 
```

then

```
az role assignment create --role "Rola dla Kamila" --assignee -@uwr.edu.pl
```
