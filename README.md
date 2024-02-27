# CloudLogger
CloudLogger enables you to perform logging operations easily and quickly in your C#, Visual Basic and .NET projects. This package facilitates logging operations by sending requests to CloudLogger service running on the server side.


## Project Secret
To use CloudLogger, you will need a Project secret. Obtain your project secret from __[CloudLogger Website](https://cloudlogger.app)__.


## Configuration

### C#
```cs
// Create an instance of CloudLogger and configure it
CloudLogger cloudLogger = CloudLogger.Create("your_project_secret");
```

### Visual Basic
```vbnet
// Create an instance of CloudLogger and configure it
Dim cloudLogger As CloudLogger = CloudLogger.Create("your_project_secret")
```

## Methods and Parameters

### CloudLogger.Create(projectSecret: string, throwExceptionOnFailure: Nullable<_bool_>)
```cs
CloudLogger.Create("your_project_secret", throwExceptionOnFailure: true );
```
Creates a new instance of CloudLogger with the provided project secret and options.

#### projectSecret _(string)_
Your CloudLogger project secret. Obtain your project secret from [CloudLogger Website](https://cloudlogger.app).

#### options _(CloudLoggerOptions)_
| Parameter               | Type    | Default Value | Description                                                                                                                                                                                                            |
|-------------------------|------------------|---------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| throwExceptionOnFailure | Nullable<_bool_> | false         | Specifies throwing an exception in case of failure. If __throwExceptionOnFailure__ set to __true__, an exception is thrown when the logging operation fails. If set to __false__, an error will be written in console. |

### CloudLogger.Log(logItems: CloudLoggerItem[], throwExceptionOnFailure: Nullable<_bool_>)
Performs the logging operation.
#### Example 1
```cs
// Perform the logging operation
await cloudLogger.LogAsync([
    new CloudLoggerItem("Date", "22-10-1994"),
    new CloudLoggerItem("Country", "Netherlands")
]);
```
#### Example 2
```cs
// Prepare the items you want to log
List<CloudLoggerItem> cloudLoggerItems =
[
    new CloudLoggerItem("Date", "22-10-1994"),
    new CloudLoggerItem("Country", "Netherlands")
];

// Perform the logging operation
await cloudLogger.LogAsync(cloudLoggerItems);
```
#### Example 3
```cs
// Perform the logging operation and throw an exception if it fails, disregarding global throwExceptionOnFailure setting.
await cloudLogger.LogAsync([
    new CloudLoggerItem("Date", "22-10-1994"),
    new CloudLoggerItem("Country", "Netherlands")
], true);
```
#### Example 4
```cs
// Perform the logging operation and don't throw an exception if it fails, disregarding global throwExceptionOnFailure setting.
await cloudLogger.LogAsync([
    new CloudLoggerItem("Date", "22-10-1994"),
    new CloudLoggerItem("Country", "Netherlands")
], false);
```
#### logItems _(List<_CloudLoggerItem_>)_
| Parameter | Type   | Description                                                                                                                                                                              |
|-----------|--------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Name      | string | The name of the column in the project where the data will be logged. Ensure that the provided name matches the column name exactly as defined in the project.                            |
| Value     | any    | The data to be logged into the specified column of the project. It is imperative to ensure that the data logged aligns precisely with the designated data type specified for the column. |

#### throwExceptionOnFailure _(Nullable<_bool_>)_
| Parameter               | Type    | Default Value                              | Description                                                                                                                                                                                                            |
|-------------------------|------------------|--------------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| throwExceptionOnFailure | Nullable<_bool_> | Global throwExceptionOnFailure | Specifies throwing an exception in case of failure. If __throwExceptionOnFailure__ set to __true__, an exception is thrown when the logging operation fails. If set to __false__, an error will be written in console. |


### CloudLogger.UpdateProjectSecret(projectSecret: string)
Updates the project secret for the CloudLogger instance, enabling logging to a different project.

```cs
// Update the project secret
cloudLogger.UpdateProjectSecret("your_project_secret");
```