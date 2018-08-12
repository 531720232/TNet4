### [TNet4]()

> 

------

## 什么是 TNet

TNet是移植自scut_game的游戏框架，其运行平台是**Net Core**

### 1. 运行需求

- [x] 任一NetCore所支持的平台
- [ ] NetCore SDK >= 2.0


### 2. 修改说明[^LaTeX]

>- ①gzip替换为7zip（已撤销）
>- ②添加EndianBinaryWriter,EndianBinaryReader
>- ③添加ecs框架
>- ④添加cs_script??

### 3. 如何使用[^code]

```csharp
     var app =new  TNet.Runtime.TNServer_AppInConsole();
     app.StartApplication();

```
### 4. 已知问题

| 描述        | 错误量   |  
| --------   | -----:  | :----:  |
| HTTP方式构造httpget可能导致错误     | 大于1 |   5     |