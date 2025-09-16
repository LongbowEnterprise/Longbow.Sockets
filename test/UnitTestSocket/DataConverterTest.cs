// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://github.com/LongbowExtensions/

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace UnitTestSocket;

public class DataConverterTest
{
    [Fact]
    public void TryConverter_Ok()
    {
        var converter = new DataConverter<MockConvertEntity>();
        var data = new byte[]
        {
            0x01, 0x02, 0x03, 0x04, 0x05, 0x03,
            0x04, 0x31, 0x09, 0x10, 0x40, 0x09,
            0x1E, 0xB8, 0x51, 0xEB, 0x85, 0x1F,
            0x40, 0x49, 0x0F, 0xDB, 0x23, 0x24,
            0x25, 0x26, 0x01, 0x01, 0x29
        };
        var result = converter.TryConvertTo(data, out _);
        Assert.True(result);

        // 测试内部异常情况
        var exceptionConverter = new DataConverter<MockExceptionEntity>();
        result = exceptionConverter.TryConvertTo(new byte[] { 0x01 }, out _);
        Assert.False(result);

        // 覆盖 IsNullable 单元测试
        var validConverter = new DataConverter<MockValidEntity>();
        validConverter.TryConvertTo(new byte[] { 0x01 }, out _);
        Assert.False(result);
    }

    class MockExceptionEntity
    {
        [DataPropertyConverter(Offset = 0, Length = 1, ConverterType = typeof(MockExceptionConverter))]
        public int Value { get; set; }
    }

    class MockNullConverter : IDataPropertyConverter
    {
        public object? Convert(ReadOnlyMemory<byte> data)
        {
            return null;
        }
    }

    class MockValidEntity
    {
        [DataPropertyConverter(Offset = 0, Length = 1, ConverterType = typeof(MockNullConverter))]
        public Foo Value { get; set; } = new();
    }

    class MockConvertEntity
    {
        [DataPropertyConverter(Offset = 0, Length = 5)]
        public byte[]? Header { get; set; }

        [DataPropertyConverter(Offset = 5, Length = 2)]
        public byte[]? Body { get; set; }

        [DataPropertyConverter(Offset = 7, Length = 1, EncodingName = "utf-8")]
        public string? Value1 { get; set; }

        [DataPropertyConverter(Offset = 8, Length = 1)]
        public int Value2 { get; set; }

        [DataPropertyConverter(Offset = 9, Length = 1)]
        public long Value3 { get; set; }

        [DataPropertyConverter(Offset = 10, Length = 8)]
        public double Value4 { get; set; }

        [DataPropertyConverter(Offset = 18, Length = 4)]
        public float Value5 { get; set; }

        [DataPropertyConverter(Offset = 22, Length = 1)]
        public short Value6 { get; set; }

        [DataPropertyConverter(Offset = 23, Length = 1)]
        public ushort Value7 { get; set; }

        [DataPropertyConverter(Offset = 24, Length = 1)]
        public uint Value8 { get; set; }

        [DataPropertyConverter(Offset = 25, Length = 1)]
        public ulong Value9 { get; set; }

        [DataPropertyConverter(Offset = 26, Length = 1)]
        public bool Value10 { get; set; }

        [DataPropertyConverter(Offset = 27, Length = 1)]
        public EnumEducation Value11 { get; set; }

        [DataPropertyConverter(Offset = 28, Length = 1, ConverterType = typeof(FooConverter), ConverterParameters = ["test"])]
        public Foo? Value12 { get; set; }

        [DataPropertyConverter(Offset = 7, Length = 1)]
        public string? Value14 { get; set; }

        public string? Value13 { get; set; }

        [DataPropertyConverter(Offset = 0, Length = 1)]
        public byte Value15 { get; set; }

        [DataPropertyConverter(Offset = 0, Length = 1, ConverterType = typeof(MockNullConverter))]
        public byte? Value16 { get; set; }
    }

    class MockExceptionConverter : IDataPropertyConverter
    {
        public object? Convert(ReadOnlyMemory<byte> data)
        {
            throw new Exception("just test");
        }
    }

    class FooConverter(string name) : IDataPropertyConverter
    {
        public object? Convert(ReadOnlyMemory<byte> data)
        {
            return new Foo() { Id = data.Span[0], Name = name };
        }
    }
}
