# Longbow.Sockets

`Longbow.Sockets` is a high-performance .NET library for handling socket-based communication. It focuses on simplifying the process of receiving, parsing, and processing binary data packets, suitable for industrial protocols, IoT communication, Modbus, TCP/UDP scenarios, and more.

The library provides flexible packet handling mechanisms, data converters, property mapping, CRC validation, and supports custom protocols and data structures.

---

## 🚀 Main Features

- **Packet Handling**
  - Supports fixed-length packets, delimiter-based packets, and other common formats.
  - Provides base classes for custom packet parsing logic.

- **Data Conversion**
  - Supports automatic conversion of basic types (int, long, float, double, string, byte[], etc.).
  - Supports BigEndian / LittleEndian conversion.
  - Supports mapping of enums, custom types, and nested objects.
  - Supports defining field offset, length, encoding, etc. via attributes.

- **Property Mapping**
  - Use `DataPropertyConverterAttribute` to map class properties to fields.
  - Supports custom converters (`IDataPropertyConverter`).

- **Logging Support**
  - Provides a unified logging interface, supports injecting `ILogger` implementations.

- **CRC Validation**
  - Built-in Modbus CRC16 algorithm for data integrity checks.

- **Encoding Tools**
  - Includes `BinConverter` and `HexConverter` utility classes for converting between byte arrays and strings.

---

## 📦 Installation

You can install Longbow.Sockets via NuGet:

```bash
dotnet add package Longbow.Sockets
```

---

## 🛠️ Quick Start

### Define Data Model

Use `DataPropertyConverterAttribute` to define your data structure:

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

### Receive and Parse Packets

```csharp
var handler = new FixLengthDataPackageHandler(16); // Assume each packet is 16 bytes
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

await handler.HandlerAsync(dataBuffer); // Receive data
```

---

## Usage Examples

### Handle Packets with Delimiter

```csharp
var handler = new DelimiterDataPackageHandler("\r\n"); // Use \r\n as delimiter
handler.ReceivedCallBack = async (data) =>
{
    var text = Encoding.UTF8.GetString(data.Span);
    Console.WriteLine("Received: " + text);
};

await handler.HandlerAsync(dataBuffer);
```

### CRC Validation

```csharp
var dataWithCrc = ModbusCrc16.Append(dataWithoutCrc);
bool isValid = ModbusCrc16.Validate(dataWithCrc);
```

---

## 🤝 Contributing

Contributions are welcome! Please refer to [CONTRIBUTING.md](CONTRIBUTING.md) for more information.

---

## 📄 License

This project is licensed under the [Apache License](LICENSE). See the `LICENSE` file for details.

## 🔗 Related Links

- [Github Project Home](https://github.com/LongbowEnterprise/Longbow.Sockets?wt.mc_id=DT-MVP-5004174)
- [NuGet Package](https://www.nuget.org/packages/Longbow.Sockets?wt.mc_id=DT-MVP-5004174)

## 📞 Contact

To contact the developers, please visit the project homepage or submit issues to [Github Issues](https://github.com/LongbowEnterprise/Longbow.Sockets/issues?wt.mc_id=DT-MVP-5004174).
