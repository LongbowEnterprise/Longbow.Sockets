// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://github.com/LongbowExtensions/

namespace Longbow.Socket.DataConverters;

/// <summary>
/// Represents an attribute used to specify a custom data converter for a class. 
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DataTypeConverterAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the type of the <see cref="IDataConverter{TEntity}"/>.
    /// </summary>
    public Type? Type { get; set; }
}
