// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://github.com/LongbowExtensions/

namespace UnitTestSocket;

public class DataPackageAdapterTest
{
    [Fact]
    public async Task HandlerAsync_Ok()
    {
        var actual = ReadOnlyMemory<byte>.Empty;
        var adapter = new DataPackageAdapter(new FixLengthDataPackageHandler(5))
        {
            ReceivedCallback = async data =>
            {
                actual = data;
                await Task.CompletedTask;
            }
        };
        await adapter.HandlerAsync(new byte[] { 0x01, 0x02, 0x03 });
        await adapter.HandlerAsync(new byte[] { 0x04, 0x05 });

        Assert.Equal(5, actual.Length);

        // 测试 DataPackageHandler 为空
        adapter = new DataPackageAdapter()
        {
            ReceivedCallback = async data =>
            {
                actual = data;
                await Task.CompletedTask;
            }
        };

        actual = ReadOnlyMemory<byte>.Empty;
        await adapter.HandlerAsync(new byte[] { 0x01, 0x02 });
        Assert.Equal([0x01, 0x02], actual.ToArray());
    }

    [Fact]
    public void TryConvertTo_Ok()
    {
        var adapter = new DataPackageAdapter(new FixLengthDataPackageHandler(5));
        var result = adapter.TryConvertTo(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 }, new DataConverter<MockEntity>(), out var entity);
        Assert.True(result);
        Assert.NotNull(entity);
        Assert.Equal([0x01, 0x02], entity.Header);
        Assert.Equal([0x03, 0x04, 0x05], entity.Body);
    }

    class MockEntity
    {
        [DataPropertyConverter(Offset = 0, Length = 2)]
        public byte[]? Header { get; set; }

        [DataPropertyConverter(Offset = 2, Length = 3)]
        public byte[]? Body { get; set; }
    }
}
