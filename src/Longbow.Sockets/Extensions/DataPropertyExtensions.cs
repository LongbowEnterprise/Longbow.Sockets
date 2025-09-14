﻿// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://github.com/LongbowExtensions/

namespace Longbow.Sockets.DataConverters;

static class DataPropertyExtensions
{
    public static IDataPropertyConverter? GetConverter(this DataPropertyConverterAttribute attribute, Type type)
    {
        return attribute.GetConverterByType() ?? attribute.GetDefaultConverter(type);
    }

    private static IDataPropertyConverter? GetConverterByType(this DataPropertyConverterAttribute attribute)
    {
        IDataPropertyConverter? converter = null;
        var converterType = attribute.ConverterType;
        if (converterType != null)
        {
            var converterParameters = attribute.ConverterParameters;
            converter = converterType.CreateInstance<IDataPropertyConverter>(converterParameters);
        }
        return converter;
    }

    private static IDataPropertyConverter? GetDefaultConverter(this DataPropertyConverterAttribute attribute, Type type)
    {
        IDataPropertyConverter? converter = null;
        if (type == typeof(byte))
        {
            converter = new DataByteConverter();
        }
        else if (type == typeof(byte[]))
        {
            converter = new DataByteArrayConverter();
        }
        else if (type == typeof(string))
        {
            converter = new DataStringConverter(attribute.EncodingName);
        }
        else if (type.IsEnum)
        {
            converter = new DataEnumConverter(type);
        }
        else if (type == typeof(bool))
        {
            converter = new DataBoolConverter();
        }
        else if (type == typeof(short))
        {
            converter = new DataInt16BigEndianConverter();
        }
        else if (type == typeof(int))
        {
            converter = new DataInt32BigEndianConverter();
        }
        else if (type == typeof(long))
        {
            converter = new DataInt64BigEndianConverter();
        }
        else if (type == typeof(float))
        {
            converter = new DataSingleBigEndianConverter();
        }
        else if (type == typeof(double))
        {
            converter = new DataDoubleBigEndianConverter();
        }
        else if (type == typeof(ushort))
        {
            converter = new DataUInt16BigEndianConverter();
        }
        else if (type == typeof(uint))
        {
            converter = new DataUInt32BigEndianConverter();
        }
        else if (type == typeof(ulong))
        {
            converter = new DataUInt64BigEndianConverter();
        }
        return converter;
    }

    public static object? ConvertTo(this DataPropertyConverterAttribute attribute, Type type, ReadOnlyMemory<byte> data)
    {
        object? ret = null;
        var start = attribute.Offset;
        var length = attribute.Length;

        if (data.Length >= start + length)
        {
            var buffer = data.Slice(start, length);
            var converter = attribute.GetConverter(type);
            if (converter != null)
            {
                ret = converter.Convert(buffer);
            }
        }
        return ret;
    }
}
