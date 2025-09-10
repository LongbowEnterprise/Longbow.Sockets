// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://github.com/LongbowExtensions/

using Longbow.Sockets.Logging;
using Microsoft.Extensions.Logging;

namespace UnitTestSocket;

public class SocketLoggingTest
{
    [Fact]
    public void Logger_Ok()
    {
        SocketLogging.LogError(new Exception());
        SocketLogging.LogInformation("Information");
        SocketLogging.LogWarning("Warning");
        SocketLogging.LogDebug("Debug");

        Assert.False(SocketLogging.Inited);

        SocketLogging.Init(new MockLogger());
        Assert.True(SocketLogging.Inited);

        SocketLogging.LogError(new Exception());
        SocketLogging.LogInformation("Information");
        SocketLogging.LogWarning("Warning");
        SocketLogging.LogDebug("Debug");
    }

    class MockLogger : ILogger<SocketLoggingTest>
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {

        }
    }
}
