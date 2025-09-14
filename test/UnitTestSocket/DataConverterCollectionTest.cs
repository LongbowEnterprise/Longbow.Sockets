﻿// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://github.com/LongbowExtensions/

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace UnitTestSocket;

public class DataConverterCollectionTest
{
    [Fact]
    public void TryGetConverter_Inject()
    {
        var sc = new ServiceCollection();
        sc.Configure<DataConverterCollection>(options =>
        {
            options.AddPropertyConverter<MockEntity>(entity => entity.Header, new DataPropertyConverterAttribute()
            {
                Offset = 0,
                Length = 5
            });
            options.AddPropertyConverter<MockEntity>(entity => entity.Body, new DataPropertyConverterAttribute()
            {
                Offset = 5,
                Length = 2
            });

            // 为提高代码覆盖率 重复添加转换器以后面的为准
            options.AddPropertyConverter<MockEntity>(entity => entity.Header, new DataPropertyConverterAttribute()
            {
                Offset = 0,
                Length = 5
            });
        });

        var provider = sc.BuildServiceProvider();
        var service = provider.GetRequiredService<IOptions<DataConverterCollection>>();
        Assert.NotNull(service.Value);
    }

    [Fact]
    public void TryConverter_Ok()
    {
        var converter = new DataConverter<MockConvertEntity>();
        var data = new byte[] { 0x1, 0x2, 0x3, 0x4, 0x5, 0x3, 0x4, 0x31, 0x09, 0x10, 0x40, 0x09, 0x1E, 0xB8, 0x51, 0xEB, 0x85, 0x1F, 0x40, 0x49, 0x0F, 0xDB, 0x23, 0x24, 0x25, 0x26, 0x01, 0x01, 0x29 };
        var result = converter.TryConvertTo(data, out _);
        Assert.True(result);
    }

    class MockEntity
    {
        public byte[]? Header { get; set; }

        public byte[]? Body { get; set; }
    }

    class MockConvertEntity
    {
        [DataPropertyConverter(Type = typeof(byte[]), Offset = 0, Length = 5)]
        public byte[]? Header { get; set; }

        [DataPropertyConverter(Type = typeof(byte[]), Offset = 5, Length = 2)]
        public byte[]? Body { get; set; }

        [DataPropertyConverter(Type = typeof(string), Offset = 7, Length = 1, EncodingName = "utf-8")]
        public string? Value1 { get; set; }

        [DataPropertyConverter(Type = typeof(int), Offset = 8, Length = 1)]
        public int Value2 { get; set; }

        [DataPropertyConverter(Type = typeof(long), Offset = 9, Length = 1)]
        public long Value3 { get; set; }

        [DataPropertyConverter(Type = typeof(double), Offset = 10, Length = 8)]
        public double Value4 { get; set; }

        [DataPropertyConverter(Type = typeof(float), Offset = 18, Length = 4)]
        public float Value5 { get; set; }

        [DataPropertyConverter(Type = typeof(short), Offset = 22, Length = 1)]
        public short Value6 { get; set; }

        [DataPropertyConverter(Type = typeof(ushort), Offset = 23, Length = 1)]
        public ushort Value7 { get; set; }

        [DataPropertyConverter(Type = typeof(uint), Offset = 24, Length = 1)]
        public uint Value8 { get; set; }

        [DataPropertyConverter(Type = typeof(ulong), Offset = 25, Length = 1)]
        public ulong Value9 { get; set; }

        [DataPropertyConverter(Type = typeof(bool), Offset = 26, Length = 1)]
        public bool Value10 { get; set; }

        [DataPropertyConverter(Type = typeof(EnumEducation), Offset = 27, Length = 1)]
        public EnumEducation Value11 { get; set; }

        [DataPropertyConverter(Type = typeof(Foo), Offset = 28, Length = 1, ConverterType = typeof(FooConverter), ConverterParameters = ["test"])]
        public Foo? Value12 { get; set; }

        [DataPropertyConverter(Type = typeof(string), Offset = 7, Length = 1)]
        public string? Value14 { get; set; }

        public string? Value13 { get; set; }

        [DataPropertyConverter(Type = typeof(byte), Offset = 0, Length = 1)]
        public byte Value15 { get; set; }

        [DataPropertyConverter(Type = typeof(byte), ConverterType = typeof(MockNullConverter), Offset = 0, Length = 1)]
        public byte Value16 { get; set; }

        [DataPropertyConverter(Type = typeof(byte[]), Offset = 0, Length = 1)]
        public byte Value17 { get; set; }
    }

    class MockNullConverter : IDataPropertyConverter
    {
        public object? Convert(ReadOnlyMemory<byte> data)
        {
            return null;
        }
    }

    class FooConverter(string name) : IDataPropertyConverter
    {
        public object? Convert(ReadOnlyMemory<byte> data)
        {
            return new Foo() { Id = data.Span[0], Name = name };
        }
    }

    class MockLogger : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {

        }
    }
}
