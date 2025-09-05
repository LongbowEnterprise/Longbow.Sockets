// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://github.com/LongbowExtensions/

namespace UnitTestSocket;

public class DataHandlerTest
{
    [Fact]
    public async Task FixLengthDataPackageHandler_Ok()
    {
        var actual = ReadOnlyMemory<byte>.Empty;
        var handler = new FixLengthDataPackageHandler(2)
        {
            ReceivedCallBack = data =>
            {
                actual = data;
                return ValueTask.CompletedTask;
            }
        };
        await handler.HandlerAsync(new byte[] { 0x01, 0x02, 0x03 });
        Assert.Equal(2, actual.Length);
    }

    [Fact]
    public async Task DelimiterDataPackageHandler_Ok()
    {
        var actual = ReadOnlyMemory<byte>.Empty;
        var handler = new DelimiterDataPackageHandler([0xFF])
        {
            ReceivedCallBack = data =>
            {
                actual = data;
                return ValueTask.CompletedTask;
            }
        };
        await handler.HandlerAsync(new byte[] { 0x01, 0x02, 0x03 });
        await handler.HandlerAsync(new byte[] { 0x01, 0xFF, 0x03 });
        Assert.Equal(5, actual.Length);
    }

    [Fact]
    public async Task DelimiterDataPackageHandler_String_Ok()
    {
        var actual = ReadOnlyMemory<byte>.Empty;
        var handler = new DelimiterDataPackageHandler("\n")
        {
            ReceivedCallBack = data =>
            {
                actual = data;
                return ValueTask.CompletedTask;
            }
        };
        await handler.HandlerAsync(new byte[] { 0x01, 0x02, 0x03, 0x0A });
        Assert.Equal(4, actual.Length);
    }

    [Fact]
    public void DelimiterDataPackageHandler_Exception()
    {
        var ex = Assert.ThrowsAny<ArgumentNullException>(() => new DelimiterDataPackageHandler(null!));
        Assert.NotNull(ex);

        ex = Assert.ThrowsAny<ArgumentNullException>(() => new DelimiterDataPackageHandler(string.Empty));
        Assert.NotNull(ex);
    }
}
