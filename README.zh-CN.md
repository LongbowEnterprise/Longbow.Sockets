# Longbow.Sockets

`Longbow.Sockets` 是一个用于处理基于套接字（Socket）通信的高性能 .NET 库。它专注于简化二进制数据包的接收、解析和处理流程，适用于工业协议、物联网（IoT）通信、Modbus、TCP/UDP 通信等场景。

该库提供了灵活的数据包处理机制、数据转换器、属性映射、CRC 校验等功能，支持自定义协议和数据结构。

---

## 🚀 主要功能

- **数据包处理**
  - 支持定长包、分隔符包等常见数据包格式。
  - 提供基础类用于自定义数据包解析逻辑。

- **数据转换**
  - 支持基本类型（int、long、float、double、string、byte[] 等）的自动转换。
  - 支持大小端（BigEndian / LittleEndian）转换。
  - 支持枚举、自定义类型、嵌套对象的映射。
  - 支持通过特性（Attribute）定义字段偏移量、长度、编码方式等。

- **属性映射**
  - 使用 `DataPropertyConverterAttribute` 特性对类的属性进行字段级映射。
  - 支持自定义转换器（`IDataPropertyConverter`）。

- **日志支持**
  - 提供统一的日志接口，支持注入 `ILogger` 实现。

- **CRC 校验**
  - 内置 Modbus CRC16 算法，可用于数据完整性校验。

- **编码工具**
  - 提供 `BinConverter` 和 `HexConverter` 工具类，用于字节数组与字符串之间的转换。

---

## 📦 安装

你可以通过 NuGet 安装 Longbow.Sockets：

```bash
dotnet add package Longbow.Sockets
```

---

## 🛠️ 快速开始

### 定义数据模型

使用 `DataPropertyConverterAttribute` 定义数据结构：

```csharp
[DataTypeConverter(Type = typeof(DataConverter<MyDataModel>))]
public class MyDataModel
{
    [DataPropertyConverter(Offset = 0, Length = 2, Type = typeof(ushort))]
    public ushort Header { get; set; }

    [DataPropertyConverter(Offset = 2, Length = 4, Type = typeof(int))]
    public int Value { get; set; }

    [DataPropertyConverter(Offset = 6, Length = 10, Type = typeof(string), EncodingName = "utf-8")]
    public string Message { get; set; }
}
```

### 接收并解析数据包

```csharp
var handler = new FixLengthDataPackageHandler(16); // 假设每个包固定16字节
var adapter = new DataPackageAdapter();

adapter.ReceivedCallBack = async (data) =>
{
    var converter = new DataConverter<MyDataModel>();
    if (converter.TryConvertTo(data, out var model))
    {
        Console.WriteLine($"Header: {model.Header}, Value: {model.Value}, Message: {model.Message}");
    }
};

handler.ReceivedCallBack = adapter.HandlerAsync;

await handler.HandlerAsync(dataBuffer); // 接收数据
```

---

## 使用示例

### 使用分隔符处理数据包

```csharp
var handler = new DelimiterDataPackageHandler("\r\n"); // 使用 \r\n 作为分隔符
handler.ReceivedCallBack = async (data) =>
{
    var text = Encoding.UTF8.GetString(data.Span);
    Console.WriteLine("Received: " + text);
};

await handler.HandlerAsync(dataBuffer);
```

### 使用 CRC 校验

```csharp
var dataWithCrc = ModbusCrc16.Append(dataWithoutCrc);
bool isValid = ModbusCrc16.Validate(dataWithCrc);
```

---

## 🤝 贡献

欢迎贡献代码和文档！请参考 [CONTRIBUTING.md](CONTRIBUTING.md) 获取更多信息。

---

## 📄 许可证

本项目采用 [Apache License](LICENSE)，请查看 `LICENSE` 文件以获取详细信息。

## 🔗 相关链接

- [Gitee 项目主页](https://gitee.com/LongbowEnterprise/Longbow.Sockets)
- [Github 项目主页](https://github.com/LongbowEnterprise/Longbow.Sockets)
- [NuGet 包](https://www.nuget.org/packages/Longbow.Sockets)

## 📞 联系方式

如需联系开发者，请查看项目主页或提交问题到 [Gitee Issues](https://gitee.com/LongbowEnterprise/Longbow.Sockets/issues) 或者 [Github Issues](https://github.com/LongbowEnterprise/Longbow.Sockets/issues)。