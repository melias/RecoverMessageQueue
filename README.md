# RecoverMessageQueue
Recover all messages inside dead letter  and resubmit queue

## Getting Started

First step to run this project is setting App.config connectionStrings and appSettings.

```c#
<connectionStrings>
    <add name="servicebus" connectionString="Endpoint=sb://[PUT_HERE_YOUR_ADDRESS].servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[PUT_HERE_YOUR_KEY]"/>
</connectionStrings>
<appSettings>
    <add key="queues" value="queuenameone|queuenametwo|queuenamethree|queuenamefour"/>
</appSettings>
  ```
  To many queues use seperator **|**
