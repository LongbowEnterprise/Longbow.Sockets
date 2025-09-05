// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://github.com/LongbowExtensions/

using System.Buffers.Binary;

namespace Longbow.Socket.DataConverters;

/// <summary>
/// Socket 数据转换为 ushort 数据大端转换器
/// </summary>
public class DataUInt16BigEndianConverter : IDataPropertyConverter
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="data"></param>
    public object? Convert(ReadOnlyMemory<byte> data)
    {
        ushort ret = 0;
        if (data.Length <= 2)
        {
            Span<byte> paddedSpan = stackalloc byte[2];
            data.Span.CopyTo(paddedSpan[(2 - data.Length)..]);
            if (BinaryPrimitives.TryReadUInt16BigEndian(paddedSpan, out var v))
            {
                ret = v;
            }
        }
        return ret;
    }
}
